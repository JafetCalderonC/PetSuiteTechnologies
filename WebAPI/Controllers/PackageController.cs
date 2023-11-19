using CoreApp;
using DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PackageController : ControllerBase
    {
        ///Controlador de mantenimiento del usuario.
        ///C -> Create
        ///R -> Retrieve
        ///U -> Update
        ///D -> Delete

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Create(Package package)
        {
            var um = new PackageManager();

            try
            {
                um.Create(package);
                return Ok(package);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        //[HttpPut]
        //[Route("Update")]
        //public async Task<IActionResult> Update(Package city)
        //{
        //    var um = new PackageManager();

        //    try
        //    {
        //        um.Update(city);
        //        return Ok(city);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}

        //[HttpDelete]
        //[Route("Delete")]
        //public async Task<IActionResult> Delete(Package city)
        //{
        //    var um = new PackageManager();

        //    try
        //    {
        //        um.Delete(city);
        //        return Ok(city);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }
        //}


        //[HttpGet]
        //[Route("RetrieveById")]
        //public async Task<IActionResult> RetrieveById(int id)
        //{
        //    try
        //    {
        //        var um = new PackageManager();
        //        var city = new Package { Id = id };

        //        return Ok(um.RetrieveById(city));
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex.Message);
        //    }

        //}

        [HttpGet]
        [Route("RetrieveAll")]
        public async Task<IActionResult> RetrieveAll()
        {
            try
            {
                var um = new PackageManager();

                return Ok(um.RetrieveAll());
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }

        }
    }
}
