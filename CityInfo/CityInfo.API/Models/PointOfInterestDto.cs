using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Models
{
    public class PointOfInterestDto
    {
        public int Id { get; set; }//has an id which is something the server is responsible for and not useful in a post
        public string Name { get; set; }
        public string Description { get; set; }    
    }
}
