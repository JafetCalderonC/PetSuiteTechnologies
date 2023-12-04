using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PetController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Pet pet)
        {
            try
            {
                var petManager = new CoreApp.PetManager();
                petManager.Create(pet);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        [Route("Update")]

        public async Task<IActionResult> Update(Pet pet)
        {
            try
            {
                var petManager = new CoreApp.PetManager();
                petManager.Update(pet);
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
                var petManager = new CoreApp.PetManager();
                petManager.Delete(dto.Id);
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
                var petManager = new CoreApp.PetManager();
                var pet = petManager.RetrieveById(id);
                return Ok(pet);
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
                var petManager = new CoreApp.PetManager();
                var pet = petManager.RetrieveAll();
                return Ok(pet);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet]
        [Route("RetrieveByUserID")]
        public async Task<IActionResult> RetrieveByUserId(int id)
        {
            try
            {
                var petManager = new CoreApp.PetManager();
                var pet = petManager.RetrieveByUserId(id);
                return Ok(pet);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}