using System.ComponentModel.DataAnnotations;
using UniPlanner.Models;


namespace UniPlanner.Models
{
    public class Programme
    {
        //List of all programmes tha exist

        [Display(Name = "Programme ID")]
        public int ProgrammeID { get; set; }
        public string Name { get; set; }

        //Foreign Key linking to Child Table UniProgrammes
        public ICollection<UniProgramme> UniProgrammes { get; set; }
    }
}
