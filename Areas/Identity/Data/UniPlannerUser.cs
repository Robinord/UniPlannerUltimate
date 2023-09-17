using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace UniPlanner.Areas.Identity.Data;

// Add profile data for application users by adding properties to the UniPlannerUser class
public class UniPlannerUser : IdentityUser
{
    //Name fields manually added for registeration
public string FirstName { get; set; }
public string LastName { get; set; }
}

