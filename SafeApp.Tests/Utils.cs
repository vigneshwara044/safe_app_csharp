using System;
using System.Linq;
using System.Runtime.InteropServices;
using SafeApp.MockAuthBindings;

namespace SafeApp.Tests {
  internal static class Utils {
    private static readonly Random Random = new Random();

    public static string GetRandomString(int length) {
      const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
      return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
    }

    public static Session RandomSession() {
      var session = new Session();
      session.Init(MockAuthResolver.Current.TestCreateApp(), new GCHandle());
      return session;
    }
  }
}
