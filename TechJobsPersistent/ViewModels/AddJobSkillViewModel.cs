using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechJobsPersistent.Models;

namespace TechJobsPersistent.ViewModels
{
    public class AddJobSkillViewModel
    {
        [Required(ErrorMessage = "Job is required")]
        public int JobId { get; set; }
        public Job Job { get; set; }

        public List<SelectListItem> Skills { get; set; }
        [Required(ErrorMessage = "Skill is required")]
        public int SkillId { get; set; }//value of selection from dropdown

        public AddJobSkillViewModel(Job theJob, List<Skill> possibleSkills)
        {//convigures Job object & Skills collection(list) 
         //VM built in controller that passes skill to job and list of possible skills
         //retreives them from db becasue VM/s !have access to DbContext

            Skills = new List<SelectListItem>();

            foreach (var skill in possibleSkills)
            {//adds SelectListItem w/ each loop to Skills list^
                Skills.Add(new SelectListItem
                {
                    Value = skill.Id.ToString(),
                    Text = skill.Name//text displayed in dropdown
                });
            }

            Job = theJob;//ie Job line 13 = theJob line 19
        }

        public AddJobSkillViewModel()
        {
        }
    }
}
