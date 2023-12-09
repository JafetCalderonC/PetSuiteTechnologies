using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IoTPetController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(IoTPet ioTPet)
        {
            try
            {
                var ioTPetManager = new CoreApp.IoTPetManager();
                ioTPetManager.Create(ioTPet);
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
                var ioTPetManager = new CoreApp.IoTPetManager();
                ioTPetManager.Delete(dto.Id);
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
                var ioTPetManager = new CoreApp.IoTPetManager();
                var ioTPet = ioTPetManager.RetrieveById(id);
                return Ok(ioTPet);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetIoTPetById")]
        public async Task<IActionResult> GetIoTPetById(int id)
        {
            try
            {
                var ioTPetManager = new CoreApp.IoTPetManager();
                var ioTPet = ioTPetManager.RetrieveByPetId(id);
                return Ok(ioTPet);
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
                var ioTPetManager = new CoreApp.IoTPetManager();
                var ioTPets = ioTPetManager.RetrieveAll();
                return Ok(ioTPets);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }       

    }
}
