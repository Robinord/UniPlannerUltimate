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
        public  string Name { get; set; }
        [Display(Name = "Major Link")]
        public string Link { get; set; }
       

        //Foreign Key linking to Parent table of UniProgrammes
        public UniProgramme UniProgramme { get; set; }
    }
}
