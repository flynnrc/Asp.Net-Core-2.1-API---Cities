using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
	[Route("api/cities")]
	public class PointsOfInterestController : Controller
	{
		[HttpGet("{cityId}/pointsofinterest")]
		public IActionResult GetPointsOfInterest(int cityId)
		{
			var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
			if(city == null)
			{
				return NotFound();
			}

			return Ok(city.PointsOfInterest);
		}

		[HttpGet("{cityId}/pointsofinterest/{id}", Name = "GetPointOfInterest")]
		public IActionResult GetPointOfInterest(int cityId, int id)
		{
			var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
			if (city == null)
			{
				return NotFound();
			}

			var pointOfInterest = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
			if (pointOfInterest == null)
			{
				return NotFound();
			}

			return Ok(pointOfInterest);
		}

		[HttpPost("{cityId}/pointsofinterest")]
		public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDto pointOfInterest)
		{
			/*
			 * use [FromBody] because the PointOfInterestForCreationDto data we want to deserialize the data into is in the body
			 * the default is to read simple data from the url and complex data from the body
			 * because we're reading simple data from the body which is not default, we'll have to specify were to look with [FromBody]
			*/
			if (pointOfInterest == null)//cannot deserialize what the consumer sent, so let them know it's a bad request
			{
				return BadRequest();
			}

			if(pointOfInterest.Description == pointOfInterest.Name)
			{
				ModelState.AddModelError("Description", "The provided description should be different from the name.");
			}
			/*
			 * instead of a fully manually writing data validation, tags can be put on the PointOfInterestForCreationDto, but manual checks can and should still be done, for example checking null
			 * notice validation is being checked in two spots now, which could be a concern, although it certainly works there are other options
			 * something like fluenetvalidation might be worth looking into for a more complicated project as it addresses some of the aformentioned concerns.
			 */
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

			if(city == null)//city doesn't exist so cannot add a point of interest
			{
				return NotFound();
			}

			//demo purposes - to be improved:
			//mapping errors could happen
			//searching all beforehand is not performant at scale
			//doesn't take race conditions into account yet
			var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(p => p.Id);

			//map to dto
			var finalPointOfInterest = new PointOfInterestDto()
			{
				Id = ++maxPointOfInterestId,
				Name = pointOfInterest.Name,
				Description = pointOfInterest.Description
			};

			//send to data store
			city.PointsOfInterest.Add(finalPointOfInterest);

			return CreatedAtRoute("GetPointOfInterest", new { cityId, id = finalPointOfInterest.Id}, finalPointOfInterest);
		}

		[HttpPut("{cityId}/pointsofinterest/{id}")]//put should fully update, all parameters according to the specification, patch is for partial updates
		public IActionResult UpdatePointOfInterest(int cityId, int id, [FromBody] PointOfInterestForUpdateDto pointOfInterest)
		{
			/*
			* use [FromBody] because the PointOfInterestForCreationDto data we want to deserialize the data into is in the body
			* the default is to read simple data from the url and complex data from the body
			* because we're reading simple data from the body which is not default, we'll have to specify were to look with [FromBody]
		   */
			if (pointOfInterest == null)//cannot deserialize what the consumer sent, so let them know it's a bad request
			{
				return BadRequest();
			}

			if (pointOfInterest.Description == pointOfInterest.Name)
			{
				ModelState.AddModelError("Description", "The provided description should be different from the name.");
			}
			/*
			 * instead of a fully manually writing data validation, tags can be put on the PointOfInterestForCreationDto, but manual checks can and should still be done, for example checking null
			 * notice validation is being checked in two spots now, which could be a concern, although it certainly works there are other options
			 * something like fluenetvalidation might be worth looking into for a more complicated project as it addresses some of the aformentioned concerns.
			 */
			if (!ModelState.IsValid)
			{
				return BadRequest();
			}

			var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);

			if (city == null)//city doesn't exist so cannot add a point of interest
			{
				return NotFound();
			}

			var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);

			if(pointOfInterestFromStore == null)
			{
				return NotFound();
			}

			//put should fully update, all parameters according to the specification
			pointOfInterestFromStore.Name = pointOfInterest.Name;
			pointOfInterestFromStore.Description = pointOfInterest.Description;

			return NoContent();//could return 200 but this is common for update as well because usually no new information is needed after the update
		}
		
		[HttpPatch("{cityId}/pointsofinterest/{id}")]
		public IActionResult PartiallyUpdatePointOfInterest(int cityId, int id, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDto> patchDoc)
		{
			/*json patch - patch is for a partial update
			*https://tools.ietf.org/html/rfc6902
			*describes a document structure for a expressing a sequence of opertations to apply to a json document
			*list of operations add replace etc...
			*make sure patch doc doesn't try to change the id, slightly safer to use update dto as type instead of main dto
			* 
			* Example patch request to update name only
			* [
				{
					"op": "replace",
					"path":"/name",
					"value": "Updated - Central Park"
				}
			]
			*/

			if (patchDoc == null)
			{
				return BadRequest();
			}

			var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
			if (city == null)
			{
				return NotFound();
			}

			var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
			if (pointOfInterestFromStore == null)
			{
				return NotFound();
			}

			//convert PointOfInterestDto from store to => PointOfInterestForUpdateDto
			var pointOfInterestToPatch = new PointOfInterestForUpdateDto()
			{
				Name = pointOfInterestFromStore.Name,
				Description = pointOfInterestFromStore.Description
			};

			patchDoc.ApplyTo(pointOfInterestToPatch, ModelState);//the overload that takes in a ModelState can be used to verify like so...
			if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

            if (pointOfInterestToPatch.Description == pointOfInterestToPatch.Name)
            {
                ModelState.AddModelError("Description", "The provided description should be different from the name.");
            }

            //have to check again for any errors added manually and after the patch was applied to see if it is still valid. 
            TryValidateModel(pointOfInterestToPatch);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //finally patch the stored value and return IActionResult
            pointOfInterestFromStore.Name = pointOfInterestToPatch.Name;
			pointOfInterestFromStore.Description = pointOfInterestToPatch.Description;

			return NoContent();
		}

        [HttpDelete("{cityId}/pointsofinterest/{id}")]
        public IActionResult DeletePointOfInterest(int cityId, int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return NotFound();
            }

            var pointOfInterestFromStore = city.PointsOfInterest.FirstOrDefault(p => p.Id == id);
            if (pointOfInterestFromStore == null)
            {
                return NotFound();
            }

            city.PointsOfInterest.Remove(pointOfInterestFromStore);
            return NoContent();
        }
    }
}

