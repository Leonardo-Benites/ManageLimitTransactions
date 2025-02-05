using Application.Interfaces;
using Domain.Models;
using Application.Dtos;
using Application.Responses;
using AutoMapper;
using Infrastructure;
using Amazon.DynamoDBv2;
using Amazon.Runtime;

namespace Application.Services
{
    public class ClientAccountService : IClientAccountService
    {
        private readonly IClientAccountRepository _accountRepository;
        readonly IMapper _mapper;

        public ClientAccountService(IMapper mapper, IClientAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<ApiResponse<ClientAccountDto>> GetByAccountAsync(string? accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Message = $"A conta não foi enviada na requisição, tente novamente.",
                    Code = 400,
                    Success = false
                };
            }

            var model = await _accountRepository.GetByAccountAsync(accountNumber);

            if (model == null)
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Message = "Conta não encontrada.",
                    Code = 404,
                    Success = false
                };
            }

            var dto = _mapper.Map<ClientAccountDto>(model);

            return ApiResponse<ClientAccountDto>.SuccessResponse(dto, "Dados da conta encontrados com sucesso");
        }

        //Cadastrar Conta e Limite
        public async Task<ApiResponse<ClientAccountDto>> CreateAsync(ClientAccountDto dto)
        {
            if (!IsRequiredFieldsFulfilled(dto))
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Data = null,
                    Message = "Verifique os dados enviados e tente novamente.",
                    Code = 400,
                    Success = false
                };
            }

            var model = _mapper.Map<ClientAccount>(dto);
            var result = await _accountRepository.CreateAsync(model);
            if (!result)
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Data = null,
                    Message = "Houve um erro ao salvar os dados da conta.",
                    Code = 400,
                    Success = false
                };
            }

            return ApiResponse<ClientAccountDto>.SuccessResponse(null, "Dados da conta e Limite criados com sucesso", 201);
        }

        public async Task<ApiResponse<ClientAccountDto>> PixAsync(TransactionDto dto)
        {
            if (!IsRequiredFieldsFulfilled(dto))
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Data = null,
                    Message = "Verifique os dados enviados e tente novamente.",
                    Code = 400,
                    Success = false
                };
            }

            var clientAccount = await _accountRepository.GetByAccountAsync(dto.Account);
            if (clientAccount == null)
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Data = null,
                    Message = "A conta informada não foi encontrada.",
                    Code = 404,
                    Success = false
                };
            }

            if (clientAccount.TransactionLimit < dto.TransactionValue)
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Data = null,
                    Message = "A conta não possui limite o suficiente para realizar essa transação.",
                    Code = 400,
                    Success = false
                };
            }

            var newLimit = clientAccount.TransactionLimit - dto.TransactionValue;

            var result = await _accountRepository.UpdateLimitAsync(dto.Account, newLimit);
            if (!result)
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Data = null,
                    Message = "Houve um erro inesperado duranto pagamento.",
                    Code = 500,
                    Success = false
                };
            }

            return ApiResponse<ClientAccountDto>.SuccessResponse(null, $"Transação realizada com sucesso, o novo limite disponível é R${newLimit}", 201);
        }

        public async Task<ApiResponse<ClientAccountDto>> UpdateLimitAsync(string? accountNumber, int newLimit)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Message = $"A conta não foi enviada na requisição, tente novamente.",
                    Code = 400,
                    Success = false
                };
            }

            var clientAccount = await _accountRepository.GetByAccountAsync(accountNumber);
            if (clientAccount == null)
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Message = "Conta informada não encontrada.",
                    Code = 404,
                    Success = false
                };
            }

            var result = await _accountRepository.UpdateLimitAsync(accountNumber, newLimit);
            if (!result)
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Data = null,
                    Message = "Houve um erro ao ajustar o limite da conta.",
                    Code = 500,
                    Success = false
                };
            }

            return ApiResponse<ClientAccountDto>.SuccessResponse(null, "Limite atualizado com sucesso");
        }

        public async Task<ApiResponse<ClientAccountDto>> DeleteAsync(string? accountNumber)
        {
            if (string.IsNullOrEmpty(accountNumber))
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Message = $"A conta não foi enviada na requisição, tente novamente.",
                    Code = 400,
                    Success = false
                };
            }

            var model = await _accountRepository.GetByAccountAsync(accountNumber);

            if (model == null)
            {
                return new ApiResponse<ClientAccountDto>
                {
                    Message = "Conta não encontrada.",
                    Code = 404,
                    Success = false
                };
            }

            await _accountRepository.DeleteAsync(accountNumber);

            return ApiResponse<ClientAccountDto>.SuccessResponse(null, "Conta e limite removidos com sucesso");
        }

        private bool IsRequiredFieldsFulfilled(ClientAccountDto dto)
        {
            return !(dto == null || string.IsNullOrEmpty(dto.Cpf) || dto.Agency == 0 || string.IsNullOrEmpty(dto.Account));
        }
        private bool IsRequiredFieldsFulfilled(TransactionDto dto)
        {
            return !(dto == null || dto.TransactionValue == 0 || string.IsNullOrEmpty(dto.Account));
        }
    }
}
