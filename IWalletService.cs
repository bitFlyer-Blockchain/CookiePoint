using System.Threading.Tasks;
using Miyabi.Common.Models;

namespace CookiePoint
{
    /**
     * IWalletService provides high level function for user's interaction with asset table in miyabi.
     */
    public interface IWalletService
    {
        /**
         * Returns wallet public address.
         */
        Address GetAddress();
 
        /**
         * Send a specific amount from the wallet to a destination address.
         */
        Task Send(decimal amount, Address to);
        
        /**
         * Print wallet current balance.
         */
        Task<decimal> GetBalance(string TokenName);
    }
}