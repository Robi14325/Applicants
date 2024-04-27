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
            return View(_context.Applicants.Include(f => f.Experience).Where(f => f.Id.Equals(id)).FirstOrDefault());
        }
        [HttpPost]
        public IActionResult Edit(Applicant applicant, string btn)
        {
            if (btn == "ADD")
            {
                applicant.Experience.Add(new Experience());
            }
            if (btn == "Edit")
            {
                //if (ModelState.IsValid)
                //{
                var oldapplicant = _context.Applicants.Find(applicant.Id);
                if (applicant.Image != null)
                {
                    // var ext = Path.GetExtension(faculty.Picture.FileName);
                    var rootPath = this._webHostEnvironment.ContentRootPath;
                    var fileToSave = Path.Combine(rootPath, "wwwroot/Pictures", applicant.Image.FileName);
                    using (var fileStream = new FileStream(fileToSave, FileMode.Create))
                    {
                        applicant.Image.CopyToAsync(fileStream);
                    }
                    applicant.ImageUrl = "~/Pictures/" + applicant.Image.FileName;
    
                }
                else
                {
                    oldapplicant.ImageUrl = oldapplicant.ImageUrl;
                }
                oldapplicant.Name = applicant.Name;
                oldapplicant.Age = applicant.Age;
            
                _context.Experiences.RemoveRange(_context.Experiences.Where(s => s.ApplicantID == applicant.Id));
                _context.SaveChanges();
                oldapplicant.Experience = applicant.Experience;
                _context.Entry(oldapplicant).State = EntityState.Modified;
                if (_context.SaveChanges() > 0)
                {
                    return RedirectToAction("Index");
                }
                //}
                else
                {
                    var message = string.Join(" | ", ModelState.Values
                                                .SelectMany(v => v.Errors)
                                                .Select(e => e.ErrorMessage));
                    ModelState.AddModelError("", message);
                }
            }
            return View(applicant);
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
