# safe_app_csharp [![NuGet](https://img.shields.io/nuget/v/MaidSafe.SafeApp.svg)](https://www.nuget.org/packages/MaidSafe.SafeApp)

.NET wrapper package for [safe_app](https://github.com/maidsafe/safe_client_libs/tree/master/safe_app). 
> [safe_app](https://github.com/maidsafe/safe_client_libs/tree/master/safe_app) is a native library which exposes low level API for application development on SAFE Network. It exposes APIs for authorisation and to manage data on the network.

safe_app_csharp API documentation is available at [docs.maidsafe.net/safe_app_csharp](https://docs.maidsafe.net/safe_app_csharp/).


**Maintainer:** Krishna Kumar (krishna.kumar@maidsafe.net)

## Build Status

| Build Server | Platform                             | Status                                                                                                                                                                            |
| ------------ | ------------------------------------ | --------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
| Azure DevOps | .NET Core MacOS, Android x86_64, iOS | [![Build status](https://dev.azure.com/maidsafe/SafeApp/_apis/build/status/SafeApp-Mobile-CI)](https://dev.azure.com/maidsafe/SafeApp/_build/latest?definitionId=7)               |
| AppVeyor     | .NET Core Windows                    | [![Build status](https://ci.appveyor.com/api/projects/status/x3m722rvosw2coao/branch/master?svg=true)](https://ci.appveyor.com/project/MaidSafe-QA/safe-app-csharp/branch/master) |  |


## Supported Platforms
* Xamarin.Android ( >=4.1.2. ABI: armeabi-v7a, x86_64)
* Xamarin.iOS ( >= 1.0, ABI: ARM64, x64)
* netstandard1.3 (for usage via portable libs)
* netcoreapp1.0 (for use via .NET Core targets. Runtime support limited to x64)
* netframework46 (for use via classic .NET Framework targets. Platform support limited to x64)

## Usage

To develop desktop and mobile apps for the SAFE Network install the latest [MaidSafe.SafeApp](https://www.nuget.org/packages/MaidSafe.SafeApp/) package from NuGet.

This package provides support for mock and non-mock APIs. By default non-mock APIs are used in the package.

#### Using Mock API
- Mock APIs can be used by adding a `SAFE_APP_MOCK` flag in **Properties > Build > conditional compilation symbols**.
- When the mock feature is used, a local mock vault file is generated which simulates network operations used to store and retrieve data. The app will then interface with this file rather than the live SAFE network.

#### Authentication
- Applications must be authenticated via the SAFE Authenticator to work with the SAFE Network. 
- The desktop authenticator is packed and shipped with [safe_browser](https://github.com/maidsafe/safe_browser/releases/latest). 
- The mobile authenticator can be found [here](https://github.com/maidsafe/safe-authenticator-mobile/releases/latest).

You can access tutorials and other learning materials to develop applications for the SAFE Network in the resources below:

* [Safe email app](https://github.com/maidsafe/safe-email-app-csharp)
* [Safe tutorial desktop and mobile apps](https://github.com/maidsafe/safe-getting-started-dotnet)


## Building safe_app_csharp

If building on Visual Studio 2017, you will need the following workloads installed:

* Xamarin
* .NET Core
* .NET Framework

To generate a local copy of API docs website follow the DocFX
installation instruction [here](https://dotnet.github.io/docfx/tutorial/docfx_getting_started.html#2-use-docfx-as-a-command-line-tool). Once downloaded run following command:
```
docfx .\docs\docfx.json
```

## Further Help

Get your developer related questions clarified on [SAFE Dev Forum](https://forum.safedev.org/). If you're looking to share any other ideas or thoughts on the SAFE Network you can reach out on [SAFE Network Forum](https://safenetforum.org/)


## Contribution

Copyrights in the SAFE Network are retained by their contributors. No copyright assignment is required to contribute to this project.


## License

This SAFE Network library is dual-licensed under the Modified BSD ([LICENSE-BSD](LICENSE-BSD) https://opensource.org/licenses/BSD-3-Clause) or the MIT license ([LICENSE-MIT](LICENSE-MIT) https://opensource.org/licenses/MIT) at your option.
