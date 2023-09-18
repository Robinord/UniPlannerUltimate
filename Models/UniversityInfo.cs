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
        // validation for rank
        [Display(Name ="THE Rank")]
        [Range(1, int.MaxValue,  ErrorMessage =" Enter Valid rank" )] //validation for rank
        public int THErank { get; set; }
        [Display(Name = "QS Rank")]
        [Range(1, int.MaxValue, ErrorMessage = " Enter Valid rank")]
        public int QSrank { get; set; }
        [Display(Name = "ARWU Rank")]
        [Range(1, int.MaxValue, ErrorMessage = " Enter Valid rank")]
        public int ARWUrank { get; set; }

        public ICollection<UniProgramme> UniProgrammes { get; set; }
    }
}
