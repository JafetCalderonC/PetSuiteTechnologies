using System;
using CoreApp;
using DTOs;
using DataAccess.CRUD;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreApp.Utilities;

namespace CoreApp
{
    public class InvoiceManager
    {
        private InvoiceCrudFactory _crud;

        public InvoiceManager()
        {
            _crud = new InvoiceCrudFactory();
        }

        private void EnsureGeneralValidation(Invoice invoice, bool isNewInvoice)
        {
            if (invoice == null)
            {
                throw new Exception("La factura es nula");
            }

            if (invoice.IssueDate == null)
            {
                throw new Exception("La fecha de emisión es requerida");
            }

            if (invoice.DueDate == null)
            {
                throw new Exception("La fecha de vencimiento es requerida");
            }

            if (invoice.IssueDate > invoice.DueDate)
            {
                throw new Exception("La fecha de emisión no puede ser mayor a la fecha de vencimiento");
            }

            if (invoice.UserId <= 0)
            {
                throw new Exception("El usuario es requerido");
            }

            if (invoice.ReservationId <= 0)
            {
                throw new Exception("La reservación es requerida");
            }

            if (invoice.TotalAmount <= 0)
            {
                throw new Exception("El monto total es requerido");
            }

            if (isNewInvoice)
            {
                if (_crud.RetrieveAll().Any(x => x.InvoiceNumber == invoice.InvoiceNumber))
                {
                    throw new Exception("La factura ya existe");
                }
            }
        }
        public void Create(Invoice invoice)
        {
            invoice.NormalizerDTO();
            EnsureGeneralValidation(invoice, true);

            _crud.Create(invoice);
        }

        public void Update(Invoice invoice)
        {
            invoice.NormalizerDTO();
            EnsureGeneralValidation(invoice, false);
            var currentInvoice = _crud.RetrieveById(invoice.Id);
            if (currentInvoice == null)
            {
                throw new Exception("La factura no existe");
            }
            currentInvoice.InvoiceNumber = invoice.InvoiceNumber;
            currentInvoice.IssueDate = invoice.IssueDate;
            currentInvoice.DueDate = invoice.DueDate;
            currentInvoice.UserId = invoice.UserId;
            currentInvoice.ReservationId = invoice.ReservationId;
            currentInvoice.TotalAmount = invoice.TotalAmount;
            currentInvoice.Status = invoice.Status;
            currentInvoice.TaxAmount = invoice.TaxAmount;
            currentInvoice.DiscountCode = invoice.DiscountCode;
            currentInvoice.DiscountAmount = invoice.DiscountAmount;
            _crud.Update(currentInvoice);
        }

        public void Delete(int id)
        {
            var currentInvoice = _crud.RetrieveById(id);
            if (currentInvoice == null)
            {
                throw new Exception("La factura no existe");
            }

            _crud.Delete(id);
        }

        public Invoice RetrieveById(int id)
        {
            var currentInvoice = _crud.RetrieveById(id);
            if (currentInvoice == null)
            {
                throw new Exception("La factura no existe");
            }

            return currentInvoice;
        }
        public List<Invoice> RetrieveAll()
        {
            var invoices = new List<Invoice>();

            foreach (var invoice in _crud.RetrieveAll())
            {
                invoices.Add(invoice);
            }
            return invoices;
        }
    } 
}