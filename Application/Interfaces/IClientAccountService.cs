using Application.Dtos;
using Application.Responses;

namespace Application.Interfaces
{
    public interface IClientAccountService
    {
        public Task<ApiResponse<ClientAccountDto>> GetByAccountAsync(string accountNumber);
        public Task<ApiResponse<ClientAccountDto>> CreateAsync(ClientAccountDto obj);
        public Task<ApiResponse<ClientAccountDto>> PixAsync(TransactionDto obj);
        public Task<ApiResponse<ClientAccountDto>> UpdateLimitAsync(string? accountNumber, int newLimit);
        public Task<ApiResponse<ClientAccountDto>> DeleteAsync(string? accountNumber);
    }
}
