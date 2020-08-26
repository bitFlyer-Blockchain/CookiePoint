using System;
using System.Threading.Tasks;
using Miyabi.Asset.Models;
using Miyabi.ClientSdk;
using Miyabi.Common.Models;
using Miyabi.Cryptography;

namespace CookiePoint
{
    class Program
    {
        const string TOKEN_NAME = "CookiePoint";
        const string SDK_CONFIG_PATH = "miyabisdkconfig.json";
        static String TABLE_ADMIN_PRIVATE_KEY;

        static void Main(string[] args)
        {
            // Register Asset Module
            AssetTypesRegisterer.RegisterTypes();
            
            // Initialize client
            var miyabiClient = new Client(SdkConfig.Load(SDK_CONFIG_PATH));

            // Create services
            var transactionService = new TransactionService(miyabiClient);

            // This line needs to be replace by a valid table admin key
            TABLE_ADMIN_PRIVATE_KEY = "input_your_key_here";
           
            // Create Alice wallet for the demo
            var aliceWallet = new WalletService(miyabiClient, transactionService);

            var demo = new Demo(transactionService, aliceWallet);
            demo.Run();
        }

        class Demo
        {
            private readonly TransactionService _transactionService;
            private readonly WalletService _aliceWallet;
            
            public Demo(TransactionService transactionService, WalletService aliceWallet)
            {
                _transactionService = transactionService;
                _aliceWallet = aliceWallet;
            }

            public void Run()
            {
                Console.WriteLine("Start table creation");
                var tx = CreateTable().Result;;

                if (_transactionService.GetTransactionResult(tx).Result.ResultCode !=
                    TransactionResultCode.Success)
                {
                    return;
                }
  
                Console.WriteLine($"Table created with tx {tx}");
                Console.WriteLine("Start generating asset");
                tx = GenerateAsset().Result;;
                
                if (_transactionService.GetTransactionResult(tx).Result.ResultCode !=
                    TransactionResultCode.Success)
                {
                    return;
                }
                Console.WriteLine($"Asset created with tx {tx}");
                Console.WriteLine($"Alice's public address : {_aliceWallet.GetAddress()}");
                Console.WriteLine($"Alice's balance : {_aliceWallet.GetBalance(TOKEN_NAME).Result}");
            }

            /**
             * Create the Asset table for Alice.
             */
            async Task<string> CreateTable()
            {
                var aliceAddress = _aliceWallet.GetAddress();
                
                // Create a new asset table with alice's address set as a owner.
                // A Table admin key is required to send this transaction.
                var assetTable = new CreateTable(new AssetTableDescriptor(
                    TOKEN_NAME, false, false, new[] {aliceAddress}));
                var memo = new MemoEntry(new[] {"Point system for Alice's shops"});
                
                var tx = TransactionCreator.SimpleSignedTransaction(
                    new ITransactionEntry[] {assetTable, memo},
                    new[] {PrivateKey.Parse(TABLE_ADMIN_PRIVATE_KEY)});

                await _transactionService.SendTransaction(tx);

                return tx.Id.ToString();
            }

            /**
             * Mint asset to Alice address
             */
            async Task<string> GenerateAsset()
            {
                var aliceAddress = _aliceWallet.GetAddress();
                var generateAsset = new AssetGen(TOKEN_NAME, 1000000m, aliceAddress);
                var memo = new MemoEntry(new[] {"Generate 1000000m CookiePoints."});

                var tx = TransactionCreator.SimpleSignedTransaction(
                    new ITransactionEntry[] {generateAsset, memo},
                    new[] {_aliceWallet.GetPrivateKey()});

                await _transactionService.SendTransaction(tx);

                return tx.Id.ToString();
            }

        }

    }
}