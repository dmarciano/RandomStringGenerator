# RSG Benchmarking

## Introduction
To monitor the performance of the RSG library, and ensure it is fast enough for the vast majority of applications, various benchmarks have been completed.  These benchmarks are designed to demonstrate the performance
of the library for various use cases including creating strings from simple patterns, secure password, dummy data (e.g. for testing/databases), specific language features (e.g. cultures), as well as to test the various
random number generators that are included with the library (System.Random, RNGCryptoServiceProvider, and Mersenne Twister).  As new features/RNGs are added, additional benchmarks will be added.

In order to try to get accurate real-world benchmark results:
1. [BenchmarkDotNet](https://github.com/dotnet/BenchmarkDotNet) library for all benchmark testing as this is pretty much the standard bechmarking framework for .NET
2. The benchmarking was done in Release mode
3. Benchmarking was conducted on a computer that was in use running other applications as the same time, like Chrome, Outlook, etc.  This was done since most applications run in an environment where
other applications are running as well so it was felt conducting the benchmarks like this provider values closer to what would be found in a deployed application vs. benchmarks that are done on a machine
doing nothing else at the same time.

The benchmarking was conducted on system with the follow specification using BenchmarkDotNot v0.12.0:
|Spec|Value|
|---|---|
|Device|Surface Pro|
|OS|Windows 10.0.18362|
|CPU|Intel Core i7-6650 2.20 GHZ (Skylake)|
|Cores| 1 CPU, 2 physical cores, 4 logical cores|
|RAM|16.0 GB (15.8 GB usable)|

A few other things to note about the benchmark results:
1. All the results for the RyuJitX64 JIT are provided in this document.  However, benchmarking was done for a total of three JITs, specifically RyJitX64, LegacyJitX64, and LegacyJitX86.  The complete sets of results
are available in the ```raw``` folder.
2. The bechmarks were also completed using the three built-in random number generators.  These include the basic System.Random RNG, a cryptographic RNG based on System.Random and RNGCryptoServiceProvider, as well as 
a Mersenne Twister RNG.
3. Where possible, the benchmarks were also run for the same pattern using the [Xeger](https://github.com/crdoconnor/xeger) library, which is another .NET library for generating random strings from regular expression.
Not all benchmarks could be compared against this library as RSG supports features that regular expressions don't, such as embedded functions and languages/cultures.
4. The benchmarks represent the time it takes to generate a string based on a parsed pattern.  A pattern is only parsed a single time and therefore does not add significantly to the time, especially when generating large
amounts of data.
5. The chart images were created using Microsoft Excel from the BechmarkDotNet results

Below is a list of the benchmarks currently available.  Click on a specific benchmark to jump to that section.
- [Social Security Numbers](#social-security-numbers)
- [Passwords](#passwords)
- [Selection From Group](#selection-from-group)
- [Postal Addresses](#postal-addresses)
- [Culture](#culture)
- [Functions](#functions)

## Benchmarks
The various benchmarks have been separated into their own sections in order to make it easier to analyze them as well as to provide any additional information for that specific benchmark.

### Social Security Numbers

### Passwords

### Selection From Group

### Postal Addresses

### Culture

### Functions


## Feedback
The tests that have been done were designed to either test specific features of the RSG library or simulate real-world data.  If there are additional benchmarks that you believe we should include on this page,
please feel free to create an issue.  If the RSG team feels that it will provide a benefit to other users, the benchmark will be developed and included on this page.