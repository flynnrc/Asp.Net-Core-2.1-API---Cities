using AutoMapper;
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

            var cityEntities = _cityInfoRepository.GetCities();

            var results = Mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities);

            /*
             * //way of manually mapping before switching to automapper
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
           */

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
                var cityResult = Mapper.Map<CityDto>(city);
                return Ok(cityResult);
            }

            var CityWithoutPointOfInterestDtoResults = Mapper.Map<CityWithoutPointOfInterestDto>(city);

            return Ok(CityWithoutPointOfInterestDtoResults);
        }
    }
}
