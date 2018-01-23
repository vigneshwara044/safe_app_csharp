using System;
using System.Collections.Generic;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings {

  // ReSharper disable MemberCanBePrivate.Global
  // ReSharper disable UnunsedMember.Global
  public struct RegisteredApp {
    public AppExchangeInfo AppInfo;
    // ReSharper disable FieldCanBeMadeReadOnly.Global
    public List<ContainerPermissions> Containers;

    internal RegisteredApp(RegisteredAppNative native) {
      AppInfo = native.AppInfo;
      Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, (int)native.ContainersLen);
    }
    // ReSharper disable once UnusedMember.Global
    internal RegisteredAppNative ToNative() {
      return new RegisteredAppNative {
        AppInfo = AppInfo,
        ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
        ContainersLen = (ulong)(Containers?.Count ?? 0),
        ContainersCap = 0
      };
    }
  }

  internal struct RegisteredAppNative {
    public AppExchangeInfo AppInfo;
    public IntPtr ContainersPtr;
    public ulong ContainersLen;

    // ReSharper disable once NotAccessedField.Compiler
    public ulong ContainersCap;
    // ReSharper disable once UnusedMember.Global
    internal void Free() {
      BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
    }
  }
}
