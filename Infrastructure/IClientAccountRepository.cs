using Domain.Models;

namespace Infrastructure
{
    public interface IClientAccountRepository
    {
        public Task<ClientAccount> GetByAccountAsync(string accountNumber);
        public Task<bool> CreateAsync(ClientAccount obj);
        public Task<bool> UpdateLimitAsync(string accountNumber, decimal newTransactionLimit); 
        public Task<bool> DeleteAsync(string accountNumber);
    }
}
