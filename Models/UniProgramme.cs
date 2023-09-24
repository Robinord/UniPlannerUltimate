using System.ComponentModel.DataAnnotations;

namespace UniPlanner.Models
{
    public class UniProgramme
    {
        //Display names added so that the actual names still follow conventions
        [Display(Name = "UniProgramme ID")]
        public int UniProgrammeID { get; set; }
        [Display(Name = "UniversityInfo ID")]
        public int UniversityInfoID { get; set; }
        [Display(Name = "Programme ID")]
        public  int ProgrammeID { get; set; }
        [Display(Name = "Programme Link")]
        public  string Link { get; set; }
        //RankScore can not be more than 320 or less than 0
        [Range(0, 320, ErrorMessage = "Please enter correct value")]
        [Display(Name = "Rank Score")]
        public int? RankScore { get; set; }


        //Foreign Keys linking to parent Tables
        [Display(Name = "University Name")]
        public UniversityInfo UniversityInfo { get; set; }
        [Display(Name = "Programme Name")]
        public Programme Programme { get; set; }

        //Foreign Key linking to Child Table MajorsOffered
        public ICollection<MajorsOffered> Majors { get; set; }
    }
}
