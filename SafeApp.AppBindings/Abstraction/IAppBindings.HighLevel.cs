using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using SafeApp.Core;

namespace SafeApp.AppBindings
{
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public partial interface IAppBindings
    {
        void SafeConnect(
            string appId,
            string authCredentials,
            Action<FfiResult, IntPtr, GCHandle> oCb);

        #region Keys

        Task<BlsKeyPair> GenerateKeyPairAsync(ref IntPtr app);

        Task<(string, BlsKeyPair?)> CreateKeysAsync(ref IntPtr app, string from, string preloadAmount, string pk);

        Task<(string, BlsKeyPair)> KeysCreatePreloadTestCoinsAsync(ref IntPtr app, string preloadAmount);

        Task<string> KeysBalanceFromSkAsync(ref IntPtr app, string sk);

        #endregion Keys

        #region Wallet

        Task<string> WalletCreateAsync(ref IntPtr app);

        Task<string> WalletInsertAsync(ref IntPtr app, string keyUrl, string name, bool setDefault, string secretKey);

        Task<string> WalletBalanceAsync(ref IntPtr app, string url);

        Task<(WalletSpendableBalance, ulong)> WalletGetDefaultBalanceAsync(ref IntPtr app, string url);

        Task<ulong> WalletTransferAsync(ref IntPtr app, string from, string to, string amount, ulong id);

        Task<WalletSpendableBalances> WalletGetAsync(ref IntPtr app, string url);

        #endregion

    }
}
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
