using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechJobsPersistent.Data;
using TechJobsPersistent.Models;
using TechJobsPersistent.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobsPersistent.Controllers
{
    public class SkillController : Controller
    {
        private JobDbContext context;

        public SkillController(JobDbContext dbContext)
        {
            context = dbContext;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            List<Skill> skills = context.Skills.ToList();
            return View(skills);
        }

        public IActionResult Add()
        {
            Skill skill = new Skill();
            return View(skill);
        }

        [HttpPost]
        public IActionResult Add(Skill skill)
        {
            if (ModelState.IsValid)
            {
                context.Skills.Add(skill);
                context.SaveChanges();
                return Redirect("/Skill/");
            }

            return View("Add", skill);
        }

        public IActionResult AddJob(int id)
        {
            Job theJob = context.Jobs.Find(id);
            List<Skill> possibleSkills = context.Skills.ToList();
            AddJobSkillViewModel viewModel = new AddJobSkillViewModel(theJob, possibleSkills);
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddJob(AddJobSkillViewModel viewModel)
        {
            if (ModelState.IsValid)
            {

                int jobId = viewModel.JobId;
                int skillId = viewModel.SkillId;

                List<JobSkill> existingItems = context.JobSkills
                    //queries for existing JobSkills to not duplicate entried
                    .Where(js => js.JobId == jobId)
                    .Where(js => js.SkillId == skillId)
                    .ToList();

                if (existingItems.Count == 0)
                {
                    JobSkill jobSkill = new JobSkill
                    {
                        JobId = jobId,
                        SkillId = skillId
                    };
                    context.JobSkills.Add(jobSkill);
                    context.SaveChanges();
                }

                return Redirect("/Home/Detail/" + jobId);
            }

            return View(viewModel);
        }
        public IActionResult Detail(int id) //<==== ?????
        {//query join class jobs with given skill
            List<JobSkill> jobSkills = context.JobSkills
                  .Where(et => et.SkillId == id)//filters JobSkills to search id
                  .Include(et => et.Job)//"eager load"
                  .Include(et => et.Skill)
                  .ToList();//turns search results into list

            return View(jobSkills);
        }
    }
}
