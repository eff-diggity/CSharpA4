using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{
    public class AddJobViewModel
    {//data being SENT to form, not validating data
        [Required] //(ErrorMessage = "XXX is required.")] //whats required? do i need this?
        public int EmployerId { get; set; }
        public string Name { get; set; }

        public List<SelectListItem/*type*/> Employers { get; set; }

        public AddJobViewModel(List<Employer> employers)//constructor
        {   
            Employers = new List<SelectListItem>();

            foreach (var employer in employers)//looping the parameter from 16
            {
                Employers.Add(
                new SelectListItem
                {
                    Value = employer.Id.ToString(),
                    Text = employer.Name
                }
                );
            }
        }
        public AddJobViewModel()
        {

        }
    }
}
