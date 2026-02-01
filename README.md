# ThrottleX - Advanced Rate Limiting Library for .NET

> ⚠️ **Work in Progress** - This library is under active development as part of my effort build something for community to learn and use. Follow along for updates!

## 🎯 Vision

ThrottleX is a comprehensive, production-ready rate limiting library for .NET applications with built-in support for distributed systems, dynamic configuration, and real-time observability. This library aims to provide developers with an easy-to-use yet powerful solution for rate limiting.

## ✨ Planned Features

### Core Algorithms
- ✅ Fixed Window Counter
- 🔄 Token Bucket
- 🔄 Sliding Window Counter
- 📋 Leaky Bucket (future)

### ASP.NET Core Integration
- 🔄 Middleware for automatic request throttling
- 🔄 Per-endpoint rate limiting with attributes (`[RateLimit("policy-name")]`)
- 🔄 Configuration-based policy management
- 🔄 Hot-reload configuration without restart

### Distributed Systems Support
- 🔄 Redis-backed distributed rate limiting
- 🔄 Atomic operations with Lua scripts
- 🔄 Multi-instance synchronization
- 📋 Support for other distributed stores (future)

### Observability & Monitoring
- 🔄 Real-time metrics API (JSON endpoints)
- 🔄 Per-endpoint throttle statistics
- 🔄 Success/rejection ratio tracking
- 📋 Grafana dashboard support (future)
- 📋 Web UI for monitoring (v2.0)

### Advanced Features
- 🔄 Dynamic limit adjustments at runtime
- 🔄 Time-based conditional limits (business hours vs off-hours)
- 🔄 Endpoint grouping and shared buckets
- 🔄 Per-user AND per-endpoint combined limits

**Legend**: ✅ Complete | 🔄 In Progress | 📋 Planned

## 🚀 Quick Start (Preview)

> **Note**: These examples show the planned API. Implementation is in progress.

### Installation
```bash
dotnet add package ThrottleX
```

## 📅 Development Roadmap

### Phase 1: Foundation (Weeks 1-2) - February 2026
- [x] Project structure and architecture
- [x] Core abstractions and interfaces
- [ ] Fixed Window algorithm implementation
- [ ] Basic unit tests

### Phase 2: Core Algorithms (Weeks 3-4) - February 2026
- [ ] Token Bucket implementation
- [ ] Sliding Window implementation
- [ ] Performance benchmarks
- [ ] Comprehensive test coverage

### Phase 3: ASP.NET Core Integration (Weeks 5-6) - March 2026
- [ ] Middleware implementation
- [ ] Per-endpoint policy system
- [ ] Attribute and configuration support
- [ ] Sample API project

### Phase 4: Distributed Support (Weeks 7-9) - March 2026
- [ ] Redis connection abstraction
- [ ] Distributed Token Bucket
- [ ] Distributed Sliding Window
- [ ] Race condition handling

### Phase 5: Hot Configuration & Metrics (Weeks 10-13) - April 2026
- [ ] Hot-reload configuration system
- [ ] Metrics API endpoints
- [ ] Documentation and examples
- [ ] First stable release (v0.1.0)

### Phase 6: Advanced Features (Weeks 14-16) - May 2026
- [ ] Dynamic limit adjustments
- [ ] Time-based conditional policies
- [ ] Enhanced observability
- [ ] v1.0.0 release

### Future (v2.0+)
- [ ] Web-based monitoring dashboard
- [ ] Additional distributed store support
- [ ] Advanced analytics and reporting

## 🏗️ Architecture

```
┌─────────────────────────────────────────┐
│         ASP.NET Core Application        │
└────────────────┬────────────────────────┘
                 │
         ┌───────▼────────┐
         │  ThrottleX     │
         │  Middleware    │
         └───────┬────────┘
                 │
    ┌────────────┼────────────┐
    │            │            │
┌───▼────┐  ┌───▼────┐  ┌───▼────┐
│ Token  │  │ Fixed  │  │Sliding │
│ Bucket │  │ Window │  │ Window │
└───┬────┘  └───┬────┘  └───┬────┘
    │            │            │
    └────────────┼────────────┘
                 │
         ┌───────▼────────┐
         │  Storage Layer │
         │ (In-Memory/    │
         │   Redis)       │
         └────────────────┘
```

## 🛠️ Technology Stack

- **.NET 9** - Target framework
- **C# 13** - Language features
- **Redis** - Distributed storage (via StackExchange.Redis)
- **xUnit** - Testing framework
- **BenchmarkDotNet** - Performance testing

## 🤝 Contributing

Contributions, suggestions, and feedback are welcome.

### How to Contribute
1. Star ⭐ this repo to follow progress
2. Open issues for bugs or feature suggestions
3. Submit PRs for improvements
4. Share your use cases and requirements

### Development Setup
```bash
# Clone repository
git clone https://github.com/RounakJoshi09/ThrottleX.git

# Restore dependencies
dotnet restore

# Run tests
dotnet test

# Run benchmarks
dotnet run --project benchmarks/ThrottleX.Benchmarks -c Release
```

## 📄 License

MIT License - feel free to use this in your projects!

## 🙏 Acknowledgments

- Built with guidance from the .NET community
- Inspired by existing solutions like AspNetCoreRateLimit
- Special thanks to everyone who provides feedback

***

**⭐ Star this repo to follow the development journey!**

**📧 Questions?** Open an issue or start a discussion - I'm learning too, and your input helps!

*Last Updated: January 31, 2026*
*Current Status: Week 0 - Foundation Setup*
