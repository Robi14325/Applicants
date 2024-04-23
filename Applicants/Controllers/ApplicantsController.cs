using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Applicants.Models;
using NuGet.Protocol.Plugins;

namespace Applicants.Controllers
{
    public class ApplicantsController : Controller
    {
        private readonly ResumeDbContext _context;
        IHostEnvironment environment;


        public ApplicantsController(ResumeDbContext context, IHostEnvironment environment)
        {
            _context = context;
            this.environment = environment;
        }

       
        public  ActionResult Index()
        {
            return View(_context.Applicants.Include(a => a.Experience).ToList());

        }

        public IActionResult Create()
        {
            Applicant applicant = new Applicant();
            applicant.Experience.Add(new Experience()
            {
                CompanyName = "",
                YearOfExperience = 0
            });
            return View(applicant);
        }
        [HttpPost]
        public IActionResult Create(string btn, Applicant applicants) 
        {
            if (btn == "ADD")
            {
                applicants.Experience.Add(new Experience());
            }
            if (btn == "Create")
            {
                //if (ModelState.IsValid)
                //{
                if (applicants.Image != null)
                {
                    // var ext = Path.GetExtension(faculty.Picture.FileName);
                    var rootPath = this.environment.ContentRootPath;
                    var fileToSave = Path.Combine(rootPath, "wwwroot/Pictures", applicants.Image.FileName);
                    using (var fileStream = new FileStream(fileToSave, FileMode.Create))
                    {
                        applicants.Image.CopyToAsync(fileStream);
                    }
                    applicants.ImageUrl = "~/Pictures/" + applicants.Image.FileName;

                    _context.Applicants.Add(applicants);
                    if (_context.SaveChanges() > 0)
                    {
                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Please Provide Profile Picture");
                    return View(applicants);
                }
            }
            else
            {
                var message = string.Join(" | ", ModelState.Values
                                            .SelectMany(v => v.Errors)
                                            .Select(e => e.ErrorMessage));
                ModelState.AddModelError("", message);
            }
            //}
            return View(applicants);
        }
        public IActionResult Edit(int id)
        {
            return View(_context.Applicants.Find(id));
        }
        public IActionResult Details(int id)
        {
            Applicant applicant = _context.Applicants.Include(a => a.Experience).FirstOrDefault(a => a.Id == id);

            return View(applicant);
        }
        public ActionResult Delete(int id)
        {
            _context.Applicants.Remove(_context.Applicants.Find(id));
            _context.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
