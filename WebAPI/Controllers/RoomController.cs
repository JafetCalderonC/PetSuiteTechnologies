using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RoomController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        [Authorize]
        public async Task<IActionResult> Create(Room room)
        {
            try
            {
                var manager = new RoomManager();
                manager.Create(room);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> Update(Room room)
        {
            try
            {
                var manager = new RoomManager();
                manager.Update(room);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(BaseDTO dto)
        {
            try
            {
                var manager = new RoomManager();
                manager.Delete(dto.Id);
                return Ok();
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpGet]
        [Route("RetrieveById")]
        public async Task<IActionResult> RetrieveById(int id)
        {
            try
            {
                var manager = new RoomManager();
                var room = manager.RetrieveById(id);
                return Ok(room);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpGet]
        [Route("RetrieveAll")]
        public async Task<IActionResult> RetrieveAll()
        {
            try
            {
                var manager = new RoomManager();
                var rooms = manager.RetrieveAll();
                return Ok(rooms);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }

        [HttpGet]
        [Route("RetrieveAvailable")]
        public async Task<IActionResult> RetrieveAvailable()
        {
            try
            {
                var manager = new RoomManager();
                var rooms = manager.RetrieveAvailable();
                return Ok(rooms);
            }
            catch (ValidationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception)
            {
                return BadRequest("Lo sentimos, algo salió mal. Inténtalo de nuevo más tarde o comunícate soporte técnico.");
            }
        }
    }
}
