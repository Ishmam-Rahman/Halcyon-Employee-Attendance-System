using HalcyonAttendance.Data;
using HalcyonAttendance.Models;
using HalcyonAttendance.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HalcyonAttendance.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;
        public EmployeeController(ApplicationDbContext db, ILogger<HomeController> logger)
        {
            _db = db;
            _logger = logger;
        }


        public IActionResult LoadAttendanceHistory()
        {

            return View(_db.AttendanceModels.ToList());
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginmodel)
        {
            if (ModelState.IsValid)
            {
                //Checking Duplicate Email
                var findemployee = _db.EmployeeDetails.FirstOrDefault(c => c.EmpEmail == loginmodel.LoginEmail && c.EmpPassword==loginmodel.Password);
                if (findemployee == null)
                {
                    ViewBag.message = "Employee with this Email and Password Doestn't exist!!!";
                    return View(loginmodel);
                }

                if (findemployee.LoginStatus == false)
                {
                    findemployee.LoginStatus = true;
                    _db.EmployeeDetails.Update(findemployee);
                    await _db.SaveChangesAsync();

                    var AttModel = new AttendanceModel
                    {
                        Email = loginmodel.LoginEmail,
                        Date = DateTime.Now.Date,
                        ArrivalTime = DateTime.Now.ToLocalTime(),
                        //LeavingTime = null,
                    };
                    _db.AttendanceModels.Add(AttModel);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    findemployee.LoginStatus = false;
                    _db.EmployeeDetails.Update(findemployee);
                    await _db.SaveChangesAsync();

                    var FindLast = _db.AttendanceModels.AsEnumerable().LastOrDefault(c => c.Email == loginmodel.LoginEmail); // .AsEnumerable() is for working with "LastOrDefult"
                    FindLast.LeavingTime = DateTime.Now;
                    _db.AttendanceModels.Update(FindLast);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Login));
                }
            }
            return View(loginmodel);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changeingmodel)
        {
            var FindEmployee = _db.EmployeeDetails.FirstOrDefault(c => c.EmpEmail == changeingmodel.email && c.EmpPassword==changeingmodel.OldPassword);
            if (FindEmployee == null)
            {
                return View(changeingmodel);
            }

            if(changeingmodel.NewPassword!=changeingmodel.ConfirmPassword)
            {
                return View(changeingmodel);
            }
            FindEmployee.EmpPassword = changeingmodel.NewPassword;

            _db.EmployeeDetails.Update(FindEmployee);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }
    }
}
