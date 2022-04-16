using System;

namespace Finance_App.Models
{
    internal class Transaction
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public bool IsReccuring { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
