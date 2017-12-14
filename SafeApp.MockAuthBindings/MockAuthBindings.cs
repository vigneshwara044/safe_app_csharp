#if !NETSTANDARD1_2 || __DESKTOP__

using System;
using System.Runtime.InteropServices;
using SafeApp.Utilities;

#if __IOS__

#endif

namespace SafeApp.MockAuthBindings {
  internal class MockAuthBindings : IMockAuthBindings {
    #region TestCreateApp

    public IntPtr TestCreateApp() {
      var ret = TestCreateAppNative(out var appPtr);
      if (ret != 0) {
        throw new InvalidOperationException();
      }
      return appPtr;
    }

#if __IOS__
    [DllImport("__Internal", EntryPoint = "test_create_app")]
#else
    [DllImport("safe_app", EntryPoint = "test_create_app")]
#endif
    private static extern int TestCreateAppNative(out IntPtr appPtr);

    #endregion
  }
}

#endif
