using System;
using System.IO;
using System.Reflection;
#if __ANDROID__
using Android.App;
#endif
using NUnit.Framework;
using SafeApp.MockAuthBindings;

namespace SafeApp.Tests {
  [TestFixture]
  class MiscTest {

    [Test, Ignore("")]
    public void RustLoggerTest() {
#if __IOS__
      var configPath = Environment.GetFolderPath(Environment.SpecialFolder.Resources);
      using (var reader = new StreamReader(Path.Combine(".", "log.toml"))) {
#elif __ANDROID__
      var configPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
      using (var reader = new StreamReader(Application.Context.Assets.Open("log.toml"))) {
#else
      var configPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
      Directory.CreateDirectory(configPath);
      using (var reader = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("SafeApp.Tests.log.toml"))) {
#endif
        using (var writer = new StreamWriter(Path.Combine(configPath, "log.toml"))) {
          writer.Write(reader.ReadToEnd());
          writer.Close();
        }

        reader.Close();
      }

      Assert.DoesNotThrowAsync(async () => await Session.InitLoggingAsync(configPath));
      Assert.ThrowsAsync<Exception>(async () => await Session.DecodeIpcMessageAsync("Some Random Invalid String"));
      Assert.IsFalse(string.IsNullOrEmpty(File.ReadAllText(Path.Combine(configPath, "Client.log"))));
    }

    [Test]
    public void IsMockBuildTest() {
      Assert.AreEqual(true, Authenticator.IsMockBuild());
    }
  }
}
