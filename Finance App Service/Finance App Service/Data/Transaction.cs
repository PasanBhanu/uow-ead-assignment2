using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Finance_App_Service.Data
{
    [Table("Transactions")]
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public bool IsReccuring { get; set; }
        public DateTime Date { get; set; }
        public int CategoryId { get; set; }
        [NotMapped]
        public Category Category { get; set; }
    }
}
