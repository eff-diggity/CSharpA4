using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{//You will need properties for the job’s name, the selected employer’s ID, and a list of all employers as SelectListItem.
    public class AddJobViewModel
    {
        public int EmployerId { get; set; }

        public string Name { get; set; }

        public List<SelectListItem> Employers { get; set; } //<===???
    }
}
