# Contributing to BrawlLib / BrawlBox

## Requirements

There are some software requirements to set up the solution:

1. [Visual Studio](https://visualstudio.microsoft.com/vs/)
2. .NET Framework v4.0 targeting pack
3. .NET Framework v4.5.1 targeting pack
4. .NET Framework v4.5.2 targeting pack
5. .NET Framework v4.6.1 targeting pack

The .NET Framework targeting packs required can be installed through the Visual Studio installer or downloaded from [this link](https://dotnet.microsoft.com/download/visual-studio-sdks).

Building the project on non-Windows operating systems is not officially supported at this time.

## Build

The main project to build is "BrawlBox".

Make sure NuGet dependencies are installed before building the project. This should be automatically done during the first build, but if it's not done automatically, you will get build errors regarding "IronPython" dependencies are missing.

Please select "x86" CPU architecture when building the solution. The default selection is "Any CPU" and this could lead to the project being built in x64 architecture, which may not work properly (due to BrawlBox code that assumes 32-bit pointers.)
