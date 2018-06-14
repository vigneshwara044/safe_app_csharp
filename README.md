# safe_app_csharp

|NuGet Package|CI - NetCore master|
|:-----------:|:-----------------:|
|[![NuGet](https://img.shields.io/nuget/v/MaidSafe.SafeApp.svg)](https://www.nuget.org/packages/MaidSafe.SafeApp)|[![Build status](https://ci.appveyor.com/api/projects/status/x3m722rvosw2coao/branch/master?svg=true)](https://ci.appveyor.com/project/MaidSafe-QA/safe-app-csharp/branch/master)|

safe_app CSharp Library. Currently supports
- Xamarin.Android ( >=4.1.2. ABI: armeabi-v7a, x86)
- Xamarin.iOS ( >= 1.0, ABI: ARM64, x64)
- netstandard1.3 (for usage via portable libs)
- netcoreapp1.0 (for use via NET Core targets. Runtime support limited to x64)
- netframework46 (for use via classic NET Framework targets. Platform support limited to x64)

| [MaidSafe website](https://maidsafe.net) | [SAFE Dev Forum](https://forum.safedev.org) | [SAFE Network Forum](https://safenetforum.org) |
|:----:|:----:|:----:|


## Build Setup
Before building the solution you should download the native libs and add to the corresponding app projects for each platform. You can use following commands in root directory of repo.
#### Windows
Run this command in powershell to download and extract native libs in respective platform projects
```
.\build.ps1 -target Unzip-Libs
```
In case if you see some policy issue
```
powershell -ExecutionPolicy ByPass -File build.ps1 -target "Unzip-Libs"
```

#### Mac
To be able to execute the bash script on macOS you should give the owner of the script permission to execute it using following command.
```
chmod +x build.sh
```
Then run this command to download and extract native libs in respective platform projects
```
.\build.sh --target=Unzip-Libs
```
if you face access denied issues, then run same command with sudo
```
sudo .\build.sh --target=Unzip-Libs
```

Once this process is completed. Restore nuget packages and your solution is ready for build.


# TODO
- [x] Extend Native API Scope to full Alpha-2 client APIs.
- [ ] Tests
  - [x] Setup test targets in currently supported platforms
  - [x] Setup basic CI suite - currently supports NetCore
  - [ ] Expand test cases to cover the Managed API scope
- [ ] Mock integration
  - [x] Support mock network via `SAFE_APP_MOCK` conditional symbol
  - [ ] Publish package including mock network support.
- [ ] Add other compatible target frameworks
  - [x] NETCore
  - [x] NetFramework
  - [ ] Xamarin.Mac
- [ ] Managed API Docs.
- [ ] Code Samples.

## License

This SAFE Network library is dual-licensed under

* the Modified BSD ([LICENSE-BSD](https://opensource.org/licenses/BSD-3-Clause)) or
* the MIT license ([LICENSE-MIT](http://opensource.org/licenses/MIT))

at your option.

## Contribution

Copyrights in the SAFE Network are retained by their contributors. No copyright assignment is required to contribute to this project.
