using System.ComponentModel.DataAnnotations;

namespace UniPlanner.Models
{
    public class MajorsOffered
    {

        //Child table of UniProgrammes
        [Key]
        [Display(Name = "Majors Offered ID")]
        public int MajorsOfferedID { get; set; }
        [Display(Name = "Programmes Offered ID")]
        public int UniProgrammeID { get; set; }
        [Display(Name = "Major Name")]
        public  string Name { get; set; }
        [Display(Name = "Major Link")]
        [RegularExpression(@"((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)", ErrorMessage = "Not a valid website URL")]
        public string Link { get; set; }
       

        //Foreign Key linking to Parent table of UniProgrammes
        public UniProgramme UniProgramme { get; set; }
    }
}
