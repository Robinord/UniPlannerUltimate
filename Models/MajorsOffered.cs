using System.ComponentModel.DataAnnotations;

namespace UniPlanner.Models
{
    public class MajorsOffered
    {
        [Key]
        [Display(Name = "MajorsOfferedID")]
        public int MajorsOfferedID { get; set; }
        [Display(Name = "UniProgrammeID")]
        public int UniProgrammeID { get; set; }   
        public  string Name { get; set; }
        public string Link { get; set; }
        [Display(Name = "UniProgramme ID")]
        public UniProgramme UniProgramme { get; set; }
    }
}
