using System.ComponentModel.DataAnnotations;

namespace UniPlanner.Models
{
    public class UniversityInfo
    {
        public int UniversityInfoID { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        // Display added for ease of use
        [Display(Name ="THE Rank")]
        public int THErank { get; set; }
        [Display(Name = "QS Rank")]
        public int QSrank { get; set; }
        [Display(Name = "ARWU Rank")]
        public int ARWUrank { get; set; }

        public ICollection<UniProgramme> UniProgrammes { get; set; }
    }
}
