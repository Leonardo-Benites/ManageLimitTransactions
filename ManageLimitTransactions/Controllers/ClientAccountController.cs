using Application.Dtos;
using Application.Interfaces;
using Application.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("Api/[controller]")]
    [ApiController]
    public class ClientAccountController : ControllerBase
    {
        public IClientAccountService _clientService;
        public ClientAccountController(IClientAccountService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("Get")]
        public async Task<ActionResult<ApiResponse<ClientAccountDto>>> Get([FromQuery]string accountNumber)
        {
            try
            {
                var response = await _clientService.GetByAccountAsync(accountNumber);

                if (!response.Success)
                {
                    return StatusCode(response.Code, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<ClientAccountDto>.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ApiResponse<bool>>> Create([FromBody] ClientAccountDto dto)
        {
            try
            {
                var response = await _clientService.CreateAsync(dto);

                if (!response.Success)
                {
                    return StatusCode(response.Code, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(ex.Message);
            }
        }

        [HttpPost("Pix")]
        public async Task<ActionResult<ApiResponse<bool>>> Pix([FromBody] TransactionDto dto)
        {
            try
            {
                var response = await _clientService.PixAsync(dto);

                if (!response.Success)
                {
                    return StatusCode(response.Code, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(ex.Message);
            }
        }

        [HttpPut("UpdateLimit/{accountNumber}")]
        public async Task<ActionResult<ApiResponse<bool>>> Put(string accountNumber, int newLimit)
        {
            try
            {
                var response = await _clientService.UpdateLimitAsync(accountNumber, newLimit);

                if (!response.Success)
                {
                    return StatusCode(response.Code, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(ex.Message);
            }
        }

        [HttpDelete("Delete")]
        public async Task<ActionResult<ApiResponse<bool>>> Delete(string account)
        {
            try
            {
                var response = await _clientService.DeleteAsync(account);

                if (!response.Success)
                {
                    return StatusCode(response.Code, response);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.ErrorResponse(ex.Message);
            }
        }
    }
}
