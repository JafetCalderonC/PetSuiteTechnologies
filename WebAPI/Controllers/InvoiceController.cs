using CoreApp;
using CoreApp.Others;
using DTOs;
using DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using Microsoft.Win32;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Invoice invoice)
        {
            try
            {
                var invoiceManager = new InvoiceManager();
                invoiceManager.Create(invoice);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("Update")]

        public async Task<IActionResult> Update(Invoice invoice)
        {
            try
            {
                var invoiceManager = new InvoiceManager();
                invoiceManager.Update(invoice);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("Delete")]

        public async Task<IActionResult> Delete(BaseDTO dto)
        {
            try
            {
                var invoiceManager = new InvoiceManager();
                invoiceManager.Delete(dto.Id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveById")]
        public async Task<IActionResult> RetrieveById(int id)
        {
            try
            {
                var invoiceManager = new InvoiceManager();
                var invoice = invoiceManager.RetrieveById(id);
                return Ok(invoice);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("RetrieveAll")]
        public async Task<IActionResult> RetrieveAll()
        {
            try
            {
                var invoiceManager = new InvoiceManager();
                var invoices = invoiceManager.RetrieveAll();
                return Ok(invoices);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
