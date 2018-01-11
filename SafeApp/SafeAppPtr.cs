using System;

namespace SafeApp {
  internal class SafeAppPtr {
    private IntPtr _value;

    public IntPtr Value {
      get {
        if (_value == IntPtr.Zero) {
          throw new ArgumentNullException(nameof(Value));
        }

        return _value;
      }
      set => _value = value;
    }

    public SafeAppPtr(IntPtr appPtr) {
      Value = appPtr;
    }

    public static implicit operator IntPtr(SafeAppPtr obj) {
      return obj.Value;
    }
  }
}
