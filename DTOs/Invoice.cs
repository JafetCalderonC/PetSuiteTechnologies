using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class Invoice : BaseDTO
    {
        public string? InvoiceNumber {  get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public int UserId { get; set; }
        public int ReservationId { get; set; }
        public decimal TotalAmount { get; set; }
        public int Status { get; set; }

        public decimal? TaxAmount { get; set; }

        public string? DiscountCode { get; set; }
        public decimal? DiscountAmount { get; set; }
    }
}
