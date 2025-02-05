using System.Text.Json.Serialization;

namespace Application.Dtos
{
    public class ClientAccountDto
    {
        public string Account { get; set; } //hash - pk 
        public string? Cpf { get; set; }
        public int Agency { get; set; }
        public decimal TransactionLimit { get; set; } //limite disponível
    }
}
