using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace ECommerce.Models.Entities
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { set; get; }

        public DateTime TransactionDate { set; get; }

        public string ExternalTransactionId { set; get; }

        public decimal Amount { set; get; }

        public decimal Fee { set; get; }

        public string Result { set; get; }

        public string Message { set; get; }

        public TransactionStatus Status { set; get; }

        public string Provider { set; get; }

        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}