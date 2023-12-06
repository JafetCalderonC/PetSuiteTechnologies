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
    public class ReservationController : ControllerBase
    {
        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Reservation reservation)
        {
            try
            {
                var reservationManager = new CoreApp.ReservationManager();
                reservationManager.Create(reservation);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPut]
        [Route("Update")]

        public async Task<IActionResult> Update(Reservation reservation)
        {
            try
            {
                var reservationManager = new CoreApp.ReservationManager();
                reservationManager.Update(reservation);
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
                var reservationManager = new CoreApp.ReservationManager();
                reservationManager.Delete(dto.Id);
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
                var reservationManager = new CoreApp.ReservationManager();
                var reservation = reservationManager.RetrieveById(id);
                return Ok(reservation);
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
                var reservationManager = new CoreApp.ReservationManager();
                var reservations = reservationManager.RetrieveAll();
                return Ok(reservations);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

     
    }
}
