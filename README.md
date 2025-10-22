# Learn-to-Code: Karel-inspired C# Tutor

This repository contains a small, opinionated C# project inspired by the [Karel the Robot Learns Python](https://compedu.stanford.edu/karel-reader/docs/python/en/intro.html) teaching environment. It is designed as an introductory, hands-on playground to help students at Synalima learn C# programming concepts in an exploratory way.

Contents

- src/Karel: the main library and a tiny console demo that wires components with dependency injection
- test/Karel.Tests: xUnit tests (unit + integration) demonstrating how to test components

Goals

- Keep the surface area small: interfaces for `IMap`, `IRobot`, `IRule` and `IScenario`, base classes and simple starter implementations.
- Show dependency injection using `Microsoft.Extensions.DependencyInjection`.
- Provide unit and integration tests so students learn testing early.

Requirements

- .NET 6 SDK (the project targets `net6.0` to match common environments)

Quickstart

Build the project:

```powershell
cd src\Karel
dotnet build
```

Run the small demo program (it demonstrates DI and a few robot actions):

```powershell
dotnet run
```

Run the tests:

```powershell
cd test\Karel.Tests
dotnet test
```

Coverage badges (committed to the repository)

This workflow generates SVG badges and commits them into `docs/coverage-badges/` on the `main` branch. After a successful run on `main` the badges will be available in the repo and can be referenced directly.

Example markdown for the badge:

[![Branch coverage](https://raw.githubusercontent.com/Synalima/learn-to-code/main/docs/coverage-badges/badge_branchcoverage.svg)](https://raw.githubusercontent.com/Synalima/learn-to-code/main/docs/coverage-badges/badge_branchcoverage.svg)

The CI step that commits badges runs only for pushes to `main` and uses `[skip ci]` in the commit message to avoid triggering additional workflow runs.
