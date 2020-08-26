using System.Threading.Tasks;
using Miyabi.Asset.Client;
using Miyabi.Asset.Models;
using Miyabi.ClientSdk;
using Miyabi.Common.Models;
using Miyabi.Cryptography;
using Miyabi.Cryptography.Secp256k1;

namespace CookiePoint
{
    public class WalletService : IWalletService
    {
        private ICryptographicService _cryptographicService;
        private TransactionService _transactionService;
        private KeyPair _keyPair;
        static AssetClient _assetClient;

        public WalletService(Client miyabiClient, TransactionService transactionService)
        {
            _cryptographicService = new Secp256k1EccService();
            _transactionService = transactionService;
            var privateKey = _cryptographicService.CreatePrivateKey();
            _keyPair = new KeyPair(privateKey);
            _assetClient = new AssetClient(miyabiClient);
        }

        public Address GetAddress()
        {
            return new PublicKeyAddress(_keyPair.PublicKey);
        }
        
        /**
         * Returns wallet private key. This method should not be exposed to tier party.
         */
        public PrivateKey GetPrivateKey()
        {
            return _keyPair.PrivateKey;
        }
        
        public async Task Send(decimal amount, Address to)
        {
            var from = GetAddress();
            var moveCoin = new AssetMove("CookiePoint", amount, from, to);
            var tx = TransactionCreator.SimpleSignedTransaction(
                new ITransactionEntry[] {moveCoin},
                new [] {_keyPair.PrivateKey});
            await _transactionService.SendTransaction(tx);
        }
        
        public async Task<decimal> GetBalance(string TokenName)
        {            
            var myAddress = GetAddress();
            var result = await _assetClient.GetAssetAsync(TokenName, myAddress);
            return result.Value;
        }
    }
}