using System.Text.Json.Serialization;

namespace Domain.Models
{
    public class ClientAccount
    {
        [JsonPropertyName("accountNumber")]
        public string Account { get; set; } //hash - pk 

        [JsonPropertyName("cpf")]
        public string? Cpf { get; set; }

        [JsonPropertyName("agency")]
        public int Agency { get; set; }

        [JsonPropertyName("transactionLimit")]
        public decimal TransactionLimit { get; set; } //limite pix

    }
}



