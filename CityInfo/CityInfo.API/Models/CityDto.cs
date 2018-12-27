using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{

    //what is returned from or accepted by an api is not the same as the entities used by the underlying data store
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();
        public int NumberOfPointsOfInterest { get { return PointsOfInterest.Count; } }
    }
}
