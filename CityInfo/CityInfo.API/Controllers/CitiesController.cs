using CityInfo.API.Models;
using CityInfo.API.Services;
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
        private ICityInfoRepository _cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            _cityInfoRepository = cityInfoRepository;
        }

        //[HttpGet("api/cities")]
        [HttpGet()]
        public IActionResult GetCities()
        {
            ////json result works, however, we can add flexibilty by using IActionResult and response methods
            //var temp = new JsonResult(CitiesDataStore.Current.Cities);
            //temp.StatusCode = 200;
            //return new JsonResult(CitiesDataStore.Current.Cities);

            //return Ok(CitiesDataStore.Current.Cities);//

            var cityEntities = _cityInfoRepository.GetCities();

            var results = new List<CityWithoutPointOfInterestDto>();
            foreach(var cityEntity in cityEntities)
            {
                results.Add(new CityWithoutPointOfInterestDto()
                {
                    Id = cityEntity.Id,
                    Description = cityEntity.Description,
                    Name = cityEntity.Name
                });
            }

            return Ok(results);
        }

        //[HttpGet("api/cities/{id}")]
        [HttpGet("{id}")]//since base route is defined above this can be simplified down to id
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = _cityInfoRepository.GetCity(id, includePointsOfInterest);

            if(city == null)
            {
                return NotFound();
            }

            if (includePointsOfInterest)
            {
                var cityResult = new CityDto()
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                };

                foreach(var poi in city.PointsOfInterest)
                {
                    cityResult.PointsOfInterest.Add(new PointOfInterestDto()
                    {
                        Id = poi.Id,
                        Name = poi.Name,
                        Description = poi.Description
                    });
                }
                return Ok(cityResult);
            }

            var CityWithoutPointOfInterestDtoResults = new CityWithoutPointOfInterestDto()
            {
                Id = city.Id,
                Description = city.Description,
                Name = city.Name
            };

            return Ok(CityWithoutPointOfInterestDtoResults);
        }
    }
}
