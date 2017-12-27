using System;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings {
    public interface IMockAuthBindings {
        IntPtr TestCreateApp();
        IntPtr TestCreateAppWithAccess(ContainerPermissions[] accessInfo);
    }
}