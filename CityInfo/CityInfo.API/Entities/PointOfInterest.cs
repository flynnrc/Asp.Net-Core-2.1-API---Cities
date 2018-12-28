using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CityInfo.API.Entities
{
    public class PointOfInterest
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [Required]
        [MaxLength(200)]
        public string Description { get; set; }

        //Navigation property to signal relationship
        [ForeignKey("CityId")]
        public City City { get; set; }
        public int CityId { get; set; }

        //Convention based approach : Declare explicitly 
        //EF will imply non-scalar references however a way to explicitly tell it that this is related to city is to include it below
        //not required to explicitly define as it should refer to the primary key of the class, but being explicit is nice for readability sometimes

        //in package manager use "Add-Migration [Namethemigrationhere]" and it will add
    }
}