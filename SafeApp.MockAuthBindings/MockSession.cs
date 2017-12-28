using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings {
  public class MockSession : Session {
    protected MockSession(IntPtr appPtr, Action disconnectedAction) : base(appPtr, disconnectedAction) { }

    public static Task<Session> CreateTestApp() {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<Session>();
          var appPtr = MockAuthResolver.Current.TestCreateApp();

          try {
            var session = new MockSession(appPtr, null);
            tcs.SetResult(session);
          } catch (Exception e) {
            tcs.SetException(e);
          }
          return tcs.Task;
        });
    }

    public static Task<Session> CreateTestAppWithAccess(List<ContainerPermissions> containerPermissions) {
      return Task.Run(
        () => {
          var tcs = new TaskCompletionSource<Session>();
          var appPtr = MockAuthResolver.Current.TestCreateAppWithAccess(containerPermissions.ToArray());

          try {
            var session = new MockSession(appPtr, null);
            tcs.SetResult(session);
          } catch (Exception e) {
            tcs.SetException(e);
          }
          return tcs.Task;
        });
    }
  }
}
