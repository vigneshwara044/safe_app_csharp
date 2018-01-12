using System;

namespace SafeApp {
  internal class SafeAppPtr {
    public static readonly SafeAppPtr Zero = new SafeAppPtr(IntPtr.Zero);

    private IntPtr _value;

    public IntPtr Value {
      get {
        if (_value == IntPtr.Zero) {
          throw new ArgumentNullException(nameof(Value));
        }

        return _value;
      }
      private set => _value = value;
    }

    public SafeAppPtr(IntPtr appPtr) {
      Value = appPtr;
    }

    public void Clear() {
      Value = IntPtr.Zero;
    }

    public static implicit operator IntPtr(SafeAppPtr obj) {
      return obj.Value;
    }
  }
}
