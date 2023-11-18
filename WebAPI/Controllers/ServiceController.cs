using DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(DTOs.Service service)
        {
            try
            {
                var serviceManager = new CoreApp.ServiceManager();
                serviceManager.Create(service);
                return Ok();
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        [Route("Update")]

        public async Task<IActionResult> Update(DTOs.Service service)
        {
            try
            {
                var serviceManager = new CoreApp.ServiceManager();
                serviceManager.Update(service);
                return Ok();
            }
            catch (System.Exception ex)
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
                var serviceManager = new CoreApp.ServiceManager();
                serviceManager.Delete(dto);
                return Ok();
            }
            catch (System.Exception ex)
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
                var serviceManager = new CoreApp.ServiceManager();
                var service = serviceManager.RetrieveById(id);
                return Ok(service);
            }
            catch (System.Exception ex)
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
                var serviceManager = new CoreApp.ServiceManager();
                var services = serviceManager.RetrieveAll();
                return Ok(services);
            }
            catch (System.Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
