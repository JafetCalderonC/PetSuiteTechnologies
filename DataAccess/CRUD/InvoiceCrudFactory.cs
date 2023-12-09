using DTOs;
using System;
using DataAccess.Mapper;
using DataAccess.DAOs;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.CRUD
{
    public class InvoiceCrudFactory : CrudFactory<Invoice>
    {
        private readonly InvoiceMapper _mapper;
        protected SqlDao _dao;
        public InvoiceCrudFactory()
        {
            _mapper = new InvoiceMapper();
            _dao = SqlDao.GetInstance();
        }
        public override void Create(Invoice dto)
        {
            var sqlOperation = new SqlOperation("CREATE_INVOICE_PR");
            sqlOperation.AddParameter("@P_INVOICE_NUMBER", dto.InvoiceNumber);
            sqlOperation.AddParameter("@P_ISSUE_DATE", dto.IssueDate);
            sqlOperation.AddParameter("@P_DUE_DATE", dto.DueDate);
            sqlOperation.AddParameter("@P_USER_ID", dto.UserId);
            sqlOperation.AddParameter("@P_RESERVATION_ID", dto.ReservationId);
            sqlOperation.AddParameter("@P_TOTAL_AMOUNT", dto.TotalAmount);
            sqlOperation.AddParameter("@P_STATUS", dto.Status);
            sqlOperation.AddParameter("@P_TAX_AMOUNT", dto.TaxAmount);
            sqlOperation.AddParameter("@P_DISCOUNT_CODE", dto.DiscountCode);
            sqlOperation.AddParameter("@P_DISCOUNT_AMOUNT", dto.DiscountAmount);
            _dao.ExecuteProcedure(sqlOperation, out int id);
            dto.Id = id;

        }
        public override void Update(Invoice dto)
        {
            var sqlOperation = new SqlOperation("UPDATE_INVOICE_PR");
            sqlOperation.AddParameter("@P_INVOICE_ID", dto.Id);
            sqlOperation.AddParameter("@P_INVOICE_NUMBER", dto.InvoiceNumber);
            sqlOperation.AddParameter("@P_ISSUE_DATE", dto.IssueDate);
            sqlOperation.AddParameter("@P_DUE_DATE", dto.DueDate);
            sqlOperation.AddParameter("@P_USER_ID", dto.UserId);
            sqlOperation.AddParameter("@P_RESERVATION_ID", dto.ReservationId);
            sqlOperation.AddParameter("@P_TOTAL_AMOUNT", dto.TotalAmount);
            sqlOperation.AddParameter("@P_STATUS", dto.Status);
            sqlOperation.AddParameter("@P_TAX_AMOUNT", dto.TaxAmount);
            sqlOperation.AddParameter("@P_DISCOUNT_CODE", dto.DiscountCode);
            sqlOperation.AddParameter("@P_DISCOUNT_AMOUNT", dto.DiscountAmount);
            _dao.ExecuteProcedure(sqlOperation);
        }
        public override void Delete(int id)
        {
            var sqlOperation = new SqlOperation("DELETE_INVOICE_PR");
            sqlOperation.AddParameter("@P_INVOICE_ID", id);
            _dao.ExecuteProcedure(sqlOperation);
        }
        public override Invoice RetrieveById(int id)
        {
            var sqlOperation = new SqlOperation("RETRIEVE_INVOICE_BY_ID_PR");
            sqlOperation.AddParameter("@P_INVOICE_ID", id);
            var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResult.Count > 0)
            {
                var dto = _mapper.BuildObject(lstResult[0]);
                return dto;
            }
            else
            {
                throw new Exception("Invoice not found");
            } 
        }
        public override List<Invoice>? RetrieveAll()
        {
            var sqlOperation = new SqlOperation("RETRIEVE_ALL_INVOICES_PR");
            var lstResult = _dao.ExecuteQueryProcedure(sqlOperation);
            if (lstResult.Count > 0)
            {
                var lstServices = _mapper.BuildObjects(lstResult);
                return lstServices;
            }
            else
            {
                throw new Exception("Invoice not found");
            }
        }
    }
}
