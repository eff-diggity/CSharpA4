using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;
using TechJobsPersistent.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace TechJobsPersistent.Controllers
{
    public class HomeController : Controller
    {
        private JobDbContext context;

        public HomeController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            List<Job> jobs = context.Jobs.Include(j => j.Employer).ToList();
            //"grabs all jobs from the db that have an employer"
            //but all jobs HAVE to have an employer-double verification, kinda overkill
            return View(jobs);
        }

        //You next need to update HomeController so that skills data is being shared with the
        //form and that the user’s skill selections are properly handled.

        //In the AddJob() method, update the AddJobViewModel object so that you pass all of
        //the Skill objects in the database to the constructor.


        [HttpGet("/Add")]
        //takes in id of job to add skill to, find Job obj in db, collect list of
        //all skills from db to show in dropdown, creates VM, renders VM
        public IActionResult AddJob()
        {
            List<Employer> employers = context.Employers.ToList();

            List<Skill> skills = context.Skills.ToList();

            AddJobViewModel addJobViewModel = new AddJobViewModel(employers, skills);
            //AddJobSkillVM (19) constructor needs theJob and possible Skills


            return View(addJobViewModel);
        }

        [HttpPost]
        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel, string[] selectedSkills)//array of string type id's
        { //VM has data from form submission- JobId and SkillId to merge objects

            if (ModelState.IsValid)
            {//validation is used for most POST requests

                Job newJob = new Job
                {
                    Name = addJobViewModel.Name,
                    EmployerId = addJobViewModel.EmployerId
                };

                foreach (string skillId in selectedSkills)
                {
                    JobSkill jobSkill = new JobSkill
                    {
                        JobId = newJob.Id,
                        SkillId = int.Parse(skillId),
                        Job = newJob
                    };
                    context.JobSkills.Add(jobSkill);
                }

                context.Jobs.Add(newJob);
                context.SaveChanges();
                return Redirect("/Home");//POST requsest usually do redirect

            }
            return View("Add", addJobViewModel);
        }

        public IActionResult Detail(int id)
        {
            Job theJob = context.Jobs
                .Include(j => j.Employer)
                .Single(j => j.Id == id);

            List<JobSkill> jobSkills = context.JobSkills
                .Where(js => js.JobId == id)
                .Include(js => js.Skill)
                .ToList();

            JobDetailViewModel viewModel = new JobDetailViewModel(theJob, jobSkills);
            return View(viewModel);
        }
    }
}
