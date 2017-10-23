using System.Reflection;
using Android.App;
using Android.OS;
using Xamarin.Android.NUnitLite;

namespace SafeApp.Tests.Android {
  [Activity(Label = "SafeApp.Tests.Android", MainLauncher = true, Icon = "@drawable/icon")]
  public class MainActivity : TestSuiteActivity {
    protected override void OnCreate(Bundle bundle) {
      AddTest(Assembly.GetExecutingAssembly());

      base.OnCreate(bundle);
    }
  }
}
