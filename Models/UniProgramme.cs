using System.ComponentModel.DataAnnotations;

namespace UniPlanner.Models
{
    public class UniProgramme
    {
        public int UniProgrammeID { get; set; }
        public int UniversityInfoID { get; set; }
        public  int ProgrammeID { get; set; }
        public  string Link { get; set; }
        [Range(0, 320, ErrorMessage = "Please enter correct value")]
        public int? RankScore { get; set; }


        
        public UniversityInfo UniversityInfo { get; set; }
        public Programme Programme { get; set; } 
        public  ICollection<MajorsOffered> Majors { get; set; }
    }
}
