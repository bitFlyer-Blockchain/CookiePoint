using System;
using System.Threading;
using System.Threading.Tasks;
using Miyabi;
using Miyabi.ClientSdk;
using Miyabi.Common.Models;

namespace CookiePoint
{
    public class TransactionService
    {
        readonly GeneralApi _generalApi;
        
        public TransactionService(IClient miyabiClient)
        {
            _generalApi = new GeneralApi(miyabiClient);
        }
        public async Task SendTransaction(Transaction tx)
        {
            try
            {
                var send = await _generalApi.SendTransactionAsync(tx);
                var result_code = send.Value;

                if (result_code != TransactionResultCode.Success
                    && result_code != TransactionResultCode.Pending)
                {
                    // Log failure here
                    Console.WriteLine($"Transaction failed with result code {result_code}");
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception thrown at SendTransaction : {e}");
            }
        }

        public async Task<TransactionResult> GetTransactionResult(string txId)
        {
            
            TransactionResult result;
            
            while ((result = _generalApi.GetTransactionResultAsync(ByteString.Parse(txId)).Result.Value).ResultCode
                   == TransactionResultCode.Pending)
            {
                // Waiting the tx to be include in block
                Thread.Sleep(1000);
            }
            
            if (result.ResultCode != TransactionResultCode.Success)
            {
                // Log failure here
                Console.WriteLine($"Transaction failed with result code {result.ResultCode}");
            }

            return result;
        }
    }
}