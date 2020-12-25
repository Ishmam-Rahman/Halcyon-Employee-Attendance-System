using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using HalcyonAttendance.Data;
using HalcyonAttendance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HalcyonAttendance.Controllers;

namespace Practicum_Project_HAS.Controllers
{
    public class Visitor : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;
        public Visitor(ApplicationDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }
        public IActionResult LoadVisitor()
        {
            return View(_db.VisitorDetails.ToList());
        }
        [HttpPost]
        public IActionResult LoadVisitor(DateTime FromDate, DateTime ToDate )
        {
            if(FromDate == default(DateTime) || ToDate == default(DateTime))
            {
                var searchVisitor = _db.VisitorDetails.ToList();
                return View(searchVisitor);
            }
            else
            {
                var searchbyTime = _db.VisitorDetails.Where(c => (c.dateTime.Date > FromDate) && (c.dateTime.Date < ToDate));
                return View(searchbyTime);
            }
           
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VisitorDetails visitorDetails)
        {
            visitorDetails.dateTime = DateTime.Now;
            if (ModelState.IsValid)
            {
                _db.VisitorDetails.Update(visitorDetails);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Create));
            }
            return View();
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var visitor = _db.VisitorDetails.Find(id);
            return View(visitor);
        }

        //Post Edit method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(VisitorDetails visitorDetails)
        {
            if (ModelState.IsValid)
            {
                _db.VisitorDetails.Update(visitorDetails);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(LoadVisitor));
            }
            return View(visitorDetails);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var visitordetails = _db.VisitorDetails.Find(id);
            if (visitordetails == null)
            {
                return NotFound();
            }
            return View(visitordetails);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id, VisitorDetails visitorDetails)
        {
            var visitor = _db.VisitorDetails.Find(id);
            _db.VisitorDetails.Remove(visitor);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(LoadVisitor));
        }


    }
}
