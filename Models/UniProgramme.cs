using System.ComponentModel.DataAnnotations;

namespace UniPlanner.Models
{
    public class UniProgramme
    {
        [Display(Name = "UniProgramme ID")]
        public int UniProgrammeID { get; set; }
        [Display(Name = "UniversityInfo ID")]
        public int UniversityInfoID { get; set; }
        [Display(Name = "Programme ID")]
        public  int ProgrammeID { get; set; }
        public  string Link { get; set; }
        [Range(0, 320, ErrorMessage = "Please enter correct value")]
        [Display(Name = "Rank Score")]
        public int? RankScore { get; set; }


        [Display(Name = "University Name")]
        public UniversityInfo UniversityInfo { get; set; }
        [Display(Name = "Programme Name")]
        public Programme Programme { get; set; } 
        public  ICollection<MajorsOffered> Majors { get; set; }
    }
}
