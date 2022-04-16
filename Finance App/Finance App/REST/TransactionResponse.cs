using Finance_App.Models;

namespace Finance_App.REST
{
    internal class TransactionResponse
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public Transaction Data { get; set; }
    }
}
