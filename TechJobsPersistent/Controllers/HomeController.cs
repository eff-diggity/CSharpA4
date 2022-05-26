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

        //Two action methods in HomeController, AddJob() and ProcessAddJobForm(),
        //will work together to return the view that contains the form and handle
        //form submission.
        [HttpGet("/Add")]
        public IActionResult AddJob()
        {//In AddJob() pass an instance of AddJobViewModel to the view.
            List<Employer> employers = context.Employers.ToList();
            AddJobViewModel addJobViewModel = new AddJobViewModel(employers); //FG <=== ??

            return View(addJobViewModel); //FG <=== ??
        }

        [HttpPost]
        public IActionResult ProcessAddJobForm(AddJobViewModel addJobViewModel)
        {
            if (ModelState.IsValid)
            {
                Employer newEmployer = new Employer
                {
                    Name = addJobViewModel.Name,
                    Id = addJobViewModel.EmployerId
                };
                context.Employers.Add(newEmployer);
                context.SaveChanges();
                return Redirect("/Employer");

            }
            return View("Add", addJobViewModel);
        }


        //[HttpPost]
        //public IActionResult ProcessAddEmployerForm(AddEmployerViewModel addEmployerViewModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Employer newEmployer = new Employer
        //        {
        //            Name = addEmployerViewModel.Name,
        //            Location = addEmployerViewModel.Location,

        //        };
        //        context.Employers.Add(newEmployer);
        //        context.SaveChanges();
        //        return Redirect("/Employer");
        //    }
        //    return View("Add", addEmployerViewModel);

        //}

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
