using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class CitiesController : Controller //derive from controller since it is a controller
    {
        //[HttpGet("api/cities")]
        [HttpGet()]
        public IActionResult GetCities()
        {
            ////json result works, however, we can add flexibilty by using IActionResult and response methods
            //var temp = new JsonResult(CitiesDataStore.Current.Cities);
            //temp.StatusCode = 200;
            //return new JsonResult(CitiesDataStore.Current.Cities);

            return Ok(CitiesDataStore.Current.Cities);//
        }

        //[HttpGet("api/cities/{id}")]
        [HttpGet("{id}")]//since base route is defined above this can be simplified down to id
        public IActionResult GetCity(int id)
        {
            //return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id));
            var cityToReturn = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            if(cityToReturn == null)
            {
                return NotFound();
            }

            return Ok(cityToReturn);
        }
    }
}
