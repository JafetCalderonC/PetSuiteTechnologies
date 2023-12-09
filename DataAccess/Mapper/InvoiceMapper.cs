using System;
using DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Mapper
{
    public class InvoiceMapper : Mapper<Invoice>
    {
        public override Invoice BuildObject(Dictionary<string, object> row)
        {
            var invoice = new Invoice
            {
                Id = Convert.ToInt32(row["invoice_id"]),
                InvoiceNumber = Convert.ToString(row["invoice_number"]),
                IssueDate = Convert.ToDateTime(row["issue_date"]),
                DueDate = Convert.ToDateTime(row["due_date"]),
                UserId = Convert.ToInt32(row["user_id"]),
                ReservationId = Convert.ToInt32(row["reservation_id"]),
                TotalAmount = Convert.ToDecimal(row["total_amount"]),
                Status = Convert.ToInt32(row["status"]),
                TaxAmount = Convert.ToDecimal(row["tax_amount"]),
                DiscountCode = Convert.ToString(row["discount_code"]),
                DiscountAmount = Convert.ToDecimal(row["discount_amount"]),
            };
            return invoice;
        }
    }


}
