using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

using SafeApp.Utilities;

namespace SafeApp.Tests {
  [TestFixture]
  internal class AuthTest {
        private const string AuthUri =
            "safe-bmv0lm1hawrzywzllmv4yw1wbgvzlm1hawx0dxrvcmlhba:AQAAAPN-m2AAAAAAAAAAACAAAAAAAAAAMxWYyOUEdIA2VNGJTmAjKFvg9yCrceomf_dZC3AdguAgAAAAAAAAALXtHOU3NtIxjIfIX4hk-pTxOeDxMiQnrXxBOqF8sp70IAAAAAAAAACjjbSMVLfM6fKxHaUwOE9ToK0mKMCLNjpstT3TqZP9KkAAAAAAAAAAbAl79zGmd3Dq5vTSVusCT8JKtr_1yYu320p24viIDG2jjbSMVLfM6fKxHaUwOE9ToK0mKMCLNjpstT3TqZP9KiAAAAAAAAAA661BaoE9k13thtGd8MVVR9gLutkSM_NUq67dnmuGBEMgAAAAAAAAAFBM4tDw2fjP-O3PryNeEf2tu56-CA1LdTidE2GIMHKYAAAAAAAAAAAAAAAAAAAAAKtRs_fd7jF0mYyq8bNyDN86wCCiowPQRQi7Db3tfRpRmDoAAAAAAAAYAAAAAAAAACcJjCl2_amfKTMP53W_gMNUxq_YCzVH0wIAAAAAAAAAJwAAAAAAAABhcHBzL25ldC5tYWlkc2FmZS5leGFtcGxlcy5tYWlsdHV0b3JpYWxPDqh1LvjMfruqH92sGoOBa9pKbeoyCyQm8HElHtfbR5g6AAAAAAAAASAAAAAAAAAA_ZaHRjRZ9gfbqCd_ZKjNUYewymCf5sNybZKM5-cT0qgYAAAAAAAAAAn89-_PuZAMk57uoVXS1YNbHR6o3uv-RAAFAAAAAAAAAAAAAAABAAAAAgAAAAMAAAAEAAAADAAAAAAAAABfcHVibGljTmFtZXMjGtQ2VkRH4VMMsrpUChdxTK6U41KgA-FmGue5Z2-dMZg6AAAAAAAAASAAAAAAAAAAFuKBLte-nkyuuXV6ovGbsfaZRBr_OFf2CRLNZ2dyRrcYAAAAAAAAAGnXgCQRN9rBO-Eh8HNZ-SLeokyCPkBMegACAAAAAAAAAAAAAAABAAAA"
          ;

        private AppExchangeInfo GetExchangeInfo() {
          return new AppExchangeInfo {Id = "net.maidsafe.example", Name = "Test App", Vendor = "Maidsafe Ltd."};
        }

        [Test]
        public async Task ConnectAsRegisteredApp() {
          var result = (AuthIpcMsg) await Session.DecodeIpcMessageAsync(AuthUri.Split(':')[1]);
          await Session.AppRegisteredAsync("bmv0lm1hawrzywzllmv4yw1wbgvzlm1hawx0dxrvcmlhba", result.AuthGranted, null);
          // TODO implement symmetric encryption after the API is expanded
        }


        [Test]
        public void EncodeAuthRequestWithEmptyContainers() {
          var authReq = new AuthReq {AppContainer = false, App = GetExchangeInfo(), Containers = new List<ContainerPermissions>() };
          Assert.DoesNotThrowAsync(async () => await Session.EncodeAuthReqAsync(authReq));
        }

        [Test]
        public void EncodeAuthRequestWithEmptyContainersAndAppContainer() {
          var authReq = new AuthReq {AppContainer = true, App = GetExchangeInfo(), Containers = new List<ContainerPermissions>() };
          Assert.DoesNotThrowAsync(async () => await Session.EncodeAuthReqAsync(authReq));
        }

//        [Test]
//        public void EncodeAuthRequestWithInvalidExchangeInfoThrowsException() {
//          var authReq = new AuthReq {
//            AppContainer = false,
//            App = new AppExchangeInfo(),
//            Containers = new List<ContainerPermissions>()
//          };
//          Assert.CatchAsync(async () => await Session.EncodeAuthReqAsync(authReq));
//        }

  }
}
