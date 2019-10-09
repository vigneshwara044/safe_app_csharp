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
        /// Initializes an Wallet object for the Session instance.
        /// The app pointer is required to perform network operations.
        /// </summary>
        /// <param name="safeAppPtr">SafeApp pointer.</param>
        internal Wallet(SafeAppPtr safeAppPtr)
            => _appPtr = safeAppPtr;

        /// <summary>
        /// Create an empty Wallet and return its XOR-URL.
        /// </summary>
        /// <returns>XOR-URL of the created wallet.</returns>
        public Task<string> WalletCreateAsync()
            => AppBindings.WalletCreateAsync(_appPtr);

        /// <summary>
        /// Add a SafeKey to a Wallet to make it spendable, and returns the friendly name set for it
        /// </summary>
        /// <param name="keyUrl">The SafeKey's safe://xor-url to verify it matches/corresponds to the secret key provided.</param>
        /// <param name="name">The name to give the spendable balance.</param>
        /// <param name="setDefault">To make this wallet as the default.</param>
        /// <param name="secretKey">The secret key needed to make the balance spendable.</param>
        /// <returns></returns>
        public Task<string> WalletInsertAsync(
            string keyUrl,
            string name,
            bool setDefault,
            string secretKey)
            => AppBindings.WalletInsertAsync(_appPtr, keyUrl, name, setDefault, secretKey);

        /// <summary>
        /// Check the total balance of a Wallet found at a given XOR-URL.
        /// </summary>
        /// <param name="url">The XOR-URL of the wallet.</param>
        /// <returns>The balance of the wallet with the given XOR-URL.</returns>
        public Task<string> WalletBalanceAsync(string url)
            => AppBindings.WalletBalanceAsync(_appPtr, url);

        /// <summary>
        /// Check the balance of the default wallet ????.
        /// </summary>
        /// <param name="url">???./</param>
        /// <returns></returns>
        public Task<(WalletSpendableBalance, ulong)> WalletGetDefaultBalanceAsync(string url)
            => AppBindings.WalletGetDefaultBalanceAsync(_appPtr, url);

        /// <summary>
        /// Transfer safecoins from one Wallet to another.
        /// </summary>
        /// <param name="from">The XOR-URL of the sender's wallet.</param>
        /// <param name="to">The XOR-URL of the recipient's wallet.</param>
        /// <param name="amount">The amount of SafeCoin.</param>
        /// <param name="id">??? SAFE ID of the sender</param>
        /// <returns>Returns the transaction ID of the transfer</returns>
        public Task<ulong> WalletTransferAsync(
            string from,
            string to,
            string amount,
            ulong id)
            => AppBindings.WalletTransferAsync(_appPtr, from, to, amount, id);

        /// <summary>
        /// Get the spendable balance of the wallet.
        /// </summary>
        /// <param name="url">The XOR-URL of the wallet</param>
        /// <returns></returns>
        public Task<WalletSpendableBalances> WalletGetAsync(string url)
            => AppBindings.WalletGetAsync(_appPtr, url);
    }
}
