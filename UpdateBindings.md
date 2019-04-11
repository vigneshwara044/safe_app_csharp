# Updating safe_app_csharp bindings from safe_client_libs

* Introduction to [Safe Client Libs](https://github.com/maidsafe/safe_client_libs/wiki/Introduction-to-Client-Libs).
* [Building SCL and generating bindings](https://github.com/maidsafe/safe_client_libs/wiki/Building-Client-Libs).
* Difference between [mock & non-mock](https://github.com/maidsafe/safe_client_libs/wiki/Mock-vs.-non-mock) libraries.
* Updating `safe_app` bindings in `safe_app_csharp`
  * Generate bindings for safe_app.
  * Update bindings in `SafeApp.AppBindings` project with new generated safe_app bindings.
  * Update manual files in `SafeApp.AppBindings` project.
  * Update utility files in `SafeApp.Utilities` project.
* Updating `safe_authenticator` bindings in `safe_app_csharp`
  * Generate bindings for safe_authenticator.
  * Update bindings in `SafeApp.MockAuthBindings/AuthBindings.cs` file with new generated safe_authenticator bindings.
    * Update namespace from `SafeAuth.AuthBindings` to `SafeApp.MockAuthBindings`
    * Update using from `SafeAuth.Utilities` to `Safeapp.Utilities`
    * Update DllName to `safe_app`
  * Update manual files in `SafeApp.MockAuthBindings` project.
    * Make sure namespace is set to `SafeApp.MockAuthBindings`
    * Update using from `SafeAuth.Utilities` to `Safeapp.Utilities`

***Note:** Make sure the changes made in the manual files in `safe_app_csharp` are synced with `safe_client_libs` and vice versa.*