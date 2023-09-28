using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace UniPlanner.Models
{
    public class UniProgramme
    {
        //Display names added so that the actual names still follow conventions
        [Display(Name = "Programmes Offered ID")]
        public int UniProgrammeID { get; set; }
        [Display(Name = "UniversityInfo ID")]
        public int UniversityInfoID { get; set; }
        [Display(Name = "Programme ID")]
        public  int ProgrammeID { get; set; }
        [Display(Name = "Programme Link")]
        [RegularExpression(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", ErrorMessage = "Not a valid website URL")]
        public  string Link { get; set; }
        //RankScore can not be more than 320 or less than 0
        [Range(0, 320, ErrorMessage = "Please enter correct value")]
        [Display(Name = "Rank Score")]
        public int? RankScore { get; set; }


        //Foreign Keys linking to parent Tables
        
        public UniversityInfo UniversityInfo { get; set; }
        
        public Programme Programme { get; set; }

        //Foreign Key linking to Child Table MajorsOffered
        public ICollection<MajorsOffered> Majors { get; set; }
    }
}
