# Contributing to BrawlCrateLib / BrawlCrate

## Requirements

There are some software requirements to set up the solution:

1.  [Visual Studio (2017 or later)](https://visualstudio.microsoft.com/vs/)
2.  [.NET Framework v4.7.2 Developer Pack](https://dotnet.microsoft.com/download/thank-you/net472-developer-pack)
3.  [Rustup](https://win.rustup.rs/)

Building the project on non-Windows operating systems is not officially supported at this time.

## Build

The main project to build is "BrawlCrate".

Make sure NuGet dependencies are installed before building the project. This should be automatically done during the first build, but if it's not done automatically, you will get build errors regarding "IronPython" dependencies are missing.

Please select "x86" CPU architecture when building the solution. The default selection is "Any CPU" and this could lead to the project being built in x64 architecture, which may not work properly (due to BrawlBox code that assumes 32-bit pointers.)
