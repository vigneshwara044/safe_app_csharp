using System;
using System.Runtime.InteropServices;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings {
  internal partial class MockAuthBindings : IMockAuthBindings {
#if __IOS__
    private const string DllName = "__Internal";
    #else
    private const string DllName = "safe_app";
#endif

    [DllImport(DllName, EntryPoint = "test_create_app")]
    private static extern int TestCreateAppNative(out IntPtr oApp);

    [DllImport(DllName, EntryPoint = "test_create_app_with_access")]
    private static extern int TestCreateAppWithAccessNative(
      [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 1)]
      ContainerPermissions[] accessInfo,
      ulong accessInfoLen,
      out IntPtr oApp);
  }
}
