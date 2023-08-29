using System.ComponentModel.DataAnnotations;

namespace UniPlanner.Models
{
    public class UniversityInfo
    {
        public int UniversityInfoID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public int THErank { get; set; }
        public int QSrank { get; set; }
        public int ARWUrank { get; set; }

        public ICollection<UniProgramme> UniProgrammes { get; set; }
    }
}
