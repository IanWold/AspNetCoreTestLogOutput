# ASP.NET Core Test Log Output

This repository gives a simple example as to how to test log output in an integration test for an ASP.NET Core API. It is a working example for my blog post [Testing Logging in ASP.NET Core](https://ian.wold.guru/Posts/testing_logging_in_asp_net_core.html), which gives a background and explanation of the code in this repository.

The `Api` project contains a simple API setup with one `/test` endpoint that logs an error level. The `Test` project contains an `ILoggerProvider` implementation to capture logs and expose them in a way that the tests can easily read. The single `Test.TestErrorLogged` test demonstrates how to inject the test logger into the host when setting up a `WebApplicationFactory`.

## Running

In order to run, you'll need to ensure you have [the .NET 9 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) installed.

To run in Visual Studio, open the SLN, build, navigate to the Test Explorer, and execute the test.

To run in VS Code, I recommend installing the [C# Dev Kit extension](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit). With that extension, you can open the folder containing the project, build with dotnet, then navigate to the Testing view to execute the test.