using System.Threading.Tasks;
using SafeApp.AppBindings;
using SafeApp.Core;

namespace SafeApp.API
{
    /// <summary>
    /// Wallet API.
    /// </summary>
    public class Wallet
    {
        static readonly IAppBindings AppBindings = AppResolver.Current;
        readonly SafeAppPtr _appPtr;

        /// <summary>
        /// Initialise a Wallet object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="safeAppPtr">SafeApp pointer.</param>
        internal Wallet(SafeAppPtr safeAppPtr)
            => _appPtr = safeAppPtr;

        /// <summary>
        /// Creates an empty Wallet and return its XorUrl.
        /// </summary>
        /// <returns>XorUrl of the newly created wallet.</returns>
        public Task<string> WalletCreateAsync()
            => AppBindings.WalletCreateAsync(_appPtr);

        /// <summary>
        /// Add a SafeKey to a Wallet to make it spendable, and returns the friendly name set for it
        /// </summary>
        /// <param name="keyUrl">The SafeKey's safe:// url to verify it matches/corresponds to the secret key provided.</param>
        /// <param name="name">The name to give the spendable balance.</param>
        /// <param name="setDefault">To set this wallet as default.</param>
        /// <param name="secretKey">The secret key needed to make the balance spendable.</param>
        /// <returns>Name used for the wallet.</returns>
        public Task<string> WalletInsertAsync(
            string keyUrl,
            string name,
            bool setDefault,
            string secretKey)
            => AppBindings.WalletInsertAsync(_appPtr, keyUrl, name, setDefault, secretKey);

        /// <summary>
        /// Check the total balance of a Wallet found at a given XorUrl.
        /// </summary>
        /// <param name="url">The XorUrl of a wallet.</param>
        /// <returns>The balance of the wallet for the given XorUrl.</returns>
        public Task<string> WalletBalanceAsync(string url)
            => AppBindings.WalletBalanceAsync(_appPtr, url);

        /// <summary>
        /// Check the default spendable balance for a wallet XorUrl.
        /// </summary>
        /// <param name="url">The XorUrl of a wallet.</param>
        /// <returns>SpendableWalletBalance instance and it's version</returns>
        public Task<(WalletSpendableBalance, ulong)> WalletGetDefaultBalanceAsync(string url)
            => AppBindings.WalletGetDefaultBalanceAsync(_appPtr, url);

        /// <summary>
        /// Transfer SafeCoins from one Wallet to the another.
        /// </summary>
        /// <param name="from">The XorUrl of the sender's wallet.</param>
        /// <param name="to">The XorUrl of the recipient's wallet.</param>
        /// <param name="amount">The amount of SafeCoins.</param>
        /// <param name="id">Transaction id, a random id will be generated if not provided.</param>
        /// <returns>Returns the transaction id of the transfer.</returns>
        public Task<ulong> WalletTransferAsync(
            string from,
            string to,
            string amount,
            ulong id)
            => AppBindings.WalletTransferAsync(_appPtr, from, to, amount, id);

        /// <summary>
        /// Get the spendable wallet balances for a wallet XorUrl.
        /// </summary>
        /// <param name="url">The XorUrl of the wallet.</param>
        /// <returns>
        /// New instance of WalletSpendableBalances which contains the list of all spendable balances.
        /// </returns>
        public Task<WalletSpendableBalances> WalletGetAsync(string url)
            => AppBindings.WalletGetAsync(_appPtr, url);
    }
}
