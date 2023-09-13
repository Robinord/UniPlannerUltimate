using System.ComponentModel.DataAnnotations;
using UniPlanner.Models;


namespace UniPlanner.Models
{
    public class Programme
    {
        [Display(Name = "Programme ID")]
        public int ProgrammeID { get; set; }
        public string Name { get; set; }

        public ICollection<UniProgramme> UniProgrammes { get; set; }
    }
}
