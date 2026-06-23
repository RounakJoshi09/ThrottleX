<#
.SYNOPSIS
    Creates ThrottleX GitHub issues from docs/github-issues/*.md

.DESCRIPTION
    Reads issue definition files, extracts title (first H1), labels, and body,
    then creates issues on RounakJoshi09/ThrottleX via GitHub CLI.

.NOTES
    Prerequisites:
      1. Unarchive the repository on GitHub (Settings → Danger Zone → Unarchive)
      2. Install GitHub CLI: https://cli.github.com/
      3. Authenticate: gh auth login
      4. Run from repo root or any path (script resolves repo root)

.EXAMPLE
    .\scripts\create-github-issues.ps1
    .\scripts\create-github-issues.ps1 -DryRun
    .\scripts\create-github-issues.ps1 -SkipLabels
#>

[CmdletBinding()]
param(
    [string] $Owner = "RounakJoshi09",
    [string] $Repo = "ThrottleX",
    [switch] $DryRun,
    [switch] $SkipLabels
)

$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$repoRoot = Split-Path -Parent $scriptDir
$issuesDir = Join-Path $repoRoot "docs\github-issues"

if (-not (Test-Path $issuesDir)) {
    Write-Error "Issues directory not found: $issuesDir"
}

# Ensure gh is available
$gh = Get-Command gh -ErrorAction SilentlyContinue
if (-not $gh -and -not $DryRun) {
    Write-Error "GitHub CLI (gh) not found. Install from https://cli.github.com/ or use -DryRun to preview."
}

function Get-IssueMeta {
    param([string] $FilePath)

    $raw = Get-Content -Path $FilePath -Raw -Encoding UTF8
    $lines = Get-Content -Path $FilePath -Encoding UTF8

    $title = ($lines | Where-Object { $_ -match '^#\s+(.+)$' } | Select-Object -First 1)
    if ($title) {
        $title = $title -replace '^#\s+', ''
    } else {
        $title = [System.IO.Path]::GetFileNameWithoutExtension($FilePath)
    }

    $labels = @()
    $labelLine = $lines | Where-Object { $_ -match '^\*\*Labels:\*\*\s*(.+)$' } | Select-Object -First 1
    if ($labelLine -match '^\*\*Labels:\*\*\s*(.+)$') {
        $labels = $Matches[1] -split ',' | ForEach-Object { $_.Trim().Trim('`') } | Where-Object { $_ }
    }

    # Body = full file minus the first H1 line (gh issue body is separate from title)
    $bodyLines = New-Object System.Collections.Generic.List[string]
    $skippedTitle = $false
    foreach ($line in $lines) {
        if (-not $skippedTitle -and $line -match '^#\s+') {
            $skippedTitle = $true
            continue
        }
        $bodyLines.Add($line)
    }
    $body = ($bodyLines -join "`n").Trim() + "`n"

    [PSCustomObject]@{
        File   = $FilePath
        Title  = $title
        Labels = $labels
        Body   = $body
    }
}

# Optional: create labels if missing (best-effort)
$desiredLabels = @(
    @{ Name = "epic"; Color = "6f42c1"; Description = "Epic / tracking issue" },
    @{ Name = "phase-1"; Color = "0e8a16"; Description = "Phase 1: Foundation" },
    @{ Name = "phase-2"; Color = "1d76db"; Description = "Phase 2: Core algorithms" },
    @{ Name = "phase-3"; Color = "fbca04"; Description = "Phase 3: ASP.NET Core" },
    @{ Name = "phase-4"; Color = "d93f0b"; Description = "Phase 4: Distributed / Redis" },
    @{ Name = "phase-5"; Color = "5319e7"; Description = "Phase 5: Hot config & metrics" },
    @{ Name = "phase-6"; Color = "006b75"; Description = "Phase 6: Advanced & v1.0" },
    @{ Name = "future"; Color = "c5def5"; Description = "Future / v2.0+" },
    @{ Name = "foundation"; Color = "bfd4f2"; Description = "Foundation work" },
    @{ Name = "algorithm"; Color = "f9d0c4"; Description = "Rate limit algorithm" },
    @{ Name = "aspnetcore"; Color = "512bd4"; Description = "ASP.NET Core integration" },
    @{ Name = "redis"; Color = "dc382d"; Description = "Redis / distributed" },
    @{ Name = "testing"; Color = "e4e669"; Description = "Tests & benchmarks" },
    @{ Name = "enhancement"; Color = "a2eeef"; Description = "New feature or request" },
    @{ Name = "chore"; Color = "fef2c0"; Description = "Chore / scaffolding" },
    @{ Name = "documentation"; Color = "0075ca"; Description = "Documentation" },
    @{ Name = "observability"; Color = "d4c5f9"; Description = "Metrics & monitoring" },
    @{ Name = "release"; Color = "b60205"; Description = "Release / packaging" }
)

if (-not $SkipLabels -and -not $DryRun) {
    Write-Host "Ensuring labels exist on $Owner/$Repo ..." -ForegroundColor Cyan
    foreach ($lbl in $desiredLabels) {
        $exists = gh label list --repo "$Owner/$Repo" --search $lbl.Name --json name --jq ".[].name" 2>$null
        if ($exists -notcontains $lbl.Name) {
            gh label create $lbl.Name --repo "$Owner/$Repo" --color $lbl.Color --description $lbl.Description 2>$null
            if ($LASTEXITCODE -eq 0) {
                Write-Host "  Created label: $($lbl.Name)" -ForegroundColor Green
            }
        }
    }
}

$files = Get-ChildItem -Path $issuesDir -Filter "*.md" | Sort-Object Name
if ($files.Count -eq 0) {
    Write-Error "No issue files found in $issuesDir"
}

Write-Host ""
Write-Host "Found $($files.Count) issue definition(s) in docs/github-issues/" -ForegroundColor Cyan
Write-Host "Target: $Owner/$Repo" -ForegroundColor Cyan
if ($DryRun) { Write-Host "DRY RUN - no issues will be created" -ForegroundColor Yellow }
Write-Host ""

$created = @()
$failed = @()

foreach ($file in $files) {
    $meta = Get-IssueMeta -FilePath $file.FullName
    $labelArg = if ($meta.Labels.Count -gt 0) { ($meta.Labels -join ",") } else { $null }

    Write-Host "→ $($meta.Title)" -ForegroundColor White
    if ($meta.Labels.Count -gt 0) {
        Write-Host "  Labels: $($meta.Labels -join ', ')" -ForegroundColor DarkGray
    }

    if ($DryRun) {
        $created += $meta.Title
        continue
    }

    $bodyFile = [System.IO.Path]::GetTempFileName()
    try {
        [System.IO.File]::WriteAllText($bodyFile, $meta.Body, [System.Text.UTF8Encoding]::new($false))

        $ghArgs = @(
            "issue", "create",
            "--repo", "$Owner/$Repo",
            "--title", $meta.Title,
            "--body-file", $bodyFile
        )
        if ($labelArg) {
            $ghArgs += @("--label", $labelArg)
        }

        $url = & gh @ghArgs 2>&1
        if ($LASTEXITCODE -eq 0) {
            Write-Host "  Created: $url" -ForegroundColor Green
            $created += [PSCustomObject]@{ Title = $meta.Title; Url = "$url" }
        } else {
            Write-Host "  FAILED: $url" -ForegroundColor Red
            $failed += $meta.Title
        }
    }
    finally {
        Remove-Item -Path $bodyFile -ErrorAction SilentlyContinue
    }
}

Write-Host ""
Write-Host "Done. Created: $($created.Count)  Failed: $($failed.Count)" -ForegroundColor Cyan
if ($failed.Count -gt 0) {
    Write-Host "Failed titles:" -ForegroundColor Red
    $failed | ForEach-Object { Write-Host "  - $_" }
    exit 1
}

Write-Host ""
Write-Host "Tip: Open the epic issue and paste links to child issues for tracking." -ForegroundColor DarkGray
