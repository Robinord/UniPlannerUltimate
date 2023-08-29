using System.ComponentModel.DataAnnotations;

namespace UniPlanner.Models
{
    public class MajorsOffered
    {
        [Key]
        public int MajorsOfferedID { get; set; }
        public int UniProgrammeID { get; set; }   
        public  string Name { get; set; }
        public string Link { get; set; }
        public UniProgramme UniProgramme { get; set; }
    }
}
