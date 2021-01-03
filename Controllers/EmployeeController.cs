using HalcyonAttendance.Data;
using HalcyonAttendance.Models;
using HalcyonAttendance.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HalcyonAttendance.Controllers
{
    [Authorize(Roles = "Admin")]
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

        [HttpPost]
        public IActionResult LoadAttendanceHistory(string SearchEmail, DateTime FromDate, DateTime ToDate)
        {
            var findemployee = _db.EmployeeDetails.FirstOrDefault(c => c.EmpEmail == SearchEmail);
            
            if (FromDate==default(DateTime) || ToDate == default(DateTime))
            {
                if(SearchEmail == null)
                    return View(_db.AttendanceModels.ToList());
                else
                {
                    if (findemployee != null) ViewBag.message = findemployee.EmpName;
                    return View(_db.AttendanceModels.Where(c => c.Email == SearchEmail).ToList());
                }
                   
            }
            if (SearchEmail == null)
            {
               // Console.WriteLine("came here here!!!!");
                //Console.WriteLine(SearchEmail);
                var searchbyTime = _db.AttendanceModels.Where(c => (c.Date.Date >= FromDate) && (c.Date.Date <= ToDate)).ToList();
                return View(searchbyTime);
            }
            else
            {
                if (findemployee != null) ViewBag.message = findemployee.EmpName;
                var searchbyTime = _db.AttendanceModels.Where(c => (c.Date.Date >= FromDate) && (c.Date.Date <= ToDate)&&(c.Email==SearchEmail)).ToList();
                return View(searchbyTime);
            }
            
        }
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }
        [AllowAnonymous]
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
                    TempData["error"] = "Email and Password Does not exist!!!";
                    return View(loginmodel);
                }
            come_here:
                if (findemployee.LoginStatus == false)
                {
                   
                    findemployee.LoginStatus = true;
                    _db.EmployeeDetails.Update(findemployee);
                    await _db.SaveChangesAsync();

                    bool checkle = false;
                    DateTime CurrrentTime = Convert.ToDateTime(DateTime.Now.ToString("h:mm tt"));
                    DateTime TimeWithCompare = Convert.ToDateTime("9:15 AM"); //Get given time with curretn date

                    if (CurrrentTime > TimeWithCompare)
                    {
                        checkle = true;
                    }

                    var AttModel = new AttendanceModel
                    {
                        Name = findemployee.EmpName,
                        Email = loginmodel.LoginEmail,
                        Date = DateTime.Now.Date,
                        ArrivalTime = DateTime.Now.ToLocalTime(),
                        LateEarly = checkle,
                        //LeavingTime = null,
                    };
                    _db.AttendanceModels.Add(AttModel);
                    await _db.SaveChangesAsync();
                    TempData["login"] = "Welcome "+findemployee.EmpName;
                    return RedirectToAction(nameof(Login));
                }
                else
                {
                    findemployee.LoginStatus = false;
                    _db.EmployeeDetails.Update(findemployee);
                    await _db.SaveChangesAsync();

                    var FindLast = _db.AttendanceModels.AsEnumerable().LastOrDefault(c => c.Email == loginmodel.LoginEmail);
                    // .AsEnumerable() is for working with "LastOrDefult"
                    DateTime ArrivlTimePlus10Hr = FindLast.ArrivalTime.AddHours(1);
                    if (ArrivlTimePlus10Hr < DateTime.Now)
                    {
                        FindLast.LeavingTime = ArrivlTimePlus10Hr;
                        _db.AttendanceModels.Update(FindLast);
                        await _db.SaveChangesAsync();
                        goto come_here;
                    }
                    FindLast.LeavingTime = DateTime.Now;
                    bool checkle = FindLast.LateEarly;
                    DateTime CurrrentTime = Convert.ToDateTime(DateTime.Now.ToString("h:mm tt"));
                    DateTime TimeWithCompare = Convert.ToDateTime("5:00 AM"); //Get given time with curretn date

                    if (CurrrentTime < TimeWithCompare)
                    {
                        checkle = true;
                    }
                    FindLast.LateEarly = checkle;
                    _db.AttendanceModels.Update(FindLast);
                    await _db.SaveChangesAsync();
                    @TempData["logout"] = "Thank you " + findemployee.EmpName;
                    return RedirectToAction(nameof(Login));
                }
            }
            return View(loginmodel);
        }
        [AllowAnonymous]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel changeingmodel)
        {
            var FindEmployee = _db.EmployeeDetails.FirstOrDefault(c => c.EmpEmail == changeingmodel.email && c.EmpPassword==changeingmodel.OldPassword);
            if (FindEmployee == null)
            {
                
                TempData["errorcp"] = "Email and Password Does not exist!!!";
                return View(changeingmodel);
            }

            if(changeingmodel.NewPassword!=changeingmodel.ConfirmPassword)
            {
                TempData["errordup"] = "New and Confirm Password are not same";
                return View(changeingmodel);
            }
            FindEmployee.EmpPassword = changeingmodel.NewPassword;

            _db.EmployeeDetails.Update(FindEmployee);
            await _db.SaveChangesAsync();
            TempData["cps"] = "Password Successfully Changed...";
            return RedirectToAction(nameof(Login));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReportGenerate(string SearchEmail, DateTime FromDate, DateTime ToDate)
        {
            Console.WriteLine("came here here!!");
            if (SearchEmail != null)
            {
                var checkval = _db.EmployeeDetails.FirstOrDefault(c => c.EmpEmail == SearchEmail);
                if (checkval == null)
                {
                    SearchEmail = null;
                }
            }
            var allemployee = _db.EmployeeDetails.ToList();
            var report = new List<AllEmployeeReportViewModel>();

            if (FromDate == default(DateTime) || ToDate == default(DateTime))
            {
                //Console.WriteLine("came to default");
                if (SearchEmail == null)
                {
                    foreach (var item in allemployee)
                    {
                        double workinghours = 0.0;
                        //Retriving Attendance details for perticular employee by EmpEmail
                        var tergetEmployee = _db.AttendanceModels.Where(c => (c.LeavingTime != default(DateTime)) && (c.Email == item.EmpEmail)).ToList();
                        foreach (var item2 in tergetEmployee)
                        {
                            workinghours += (item2.LeavingTime - item2.ArrivalTime).TotalHours;
                        }
                        var indreport = new AllEmployeeReportViewModel
                        {
                            image = item.EmpImage,
                            name = item.EmpName,
                            email = item.EmpEmail,
                            WorkingHour = workinghours,
                            position = item.EmpPosition,
                        };
                        report.Add(indreport);
                    }
                    return View(report);
                }
                    
                else
                {
                    double workinghours = 0.0;
                    var findEmp = _db.EmployeeDetails.FirstOrDefault(c => c.EmpEmail == SearchEmail);
                    var tergetEmployee = _db.AttendanceModels.Where(c => (c.LeavingTime != default(DateTime)) && (c.Email == SearchEmail)).ToList();
                    foreach (var item2 in tergetEmployee)
                    {
                        workinghours += (item2.LeavingTime - item2.ArrivalTime).TotalHours;
                    }
                    var indreport = new AllEmployeeReportViewModel
                    {
                        image = findEmp.EmpImage,
                        name = findEmp.EmpName,
                        email = SearchEmail,
                        WorkingHour = workinghours,
                        position = findEmp.EmpPosition,
                    };
                    report.Add(indreport);

                    return View(report);
                }
            }

            if (SearchEmail == null)
            {
                foreach (var item in allemployee)
                {
                    double workinghours = 0.0;
                    string EmailForSearch = item.EmpEmail;
                    //Retriving Attendance details for perticular employee
                    var tergetEmployee = _db.AttendanceModels.Where(c => (c.LeavingTime != default(DateTime)) && (c.Date.Date >= FromDate) && (c.Date.Date <= ToDate) && (c.Email == EmailForSearch)).ToList();
                    foreach (var item2 in tergetEmployee)
                    {
                        workinghours += (item2.LeavingTime - item2.ArrivalTime).TotalHours;
                    }
                    var indreport = new AllEmployeeReportViewModel
                    {
                        image = item.EmpImage,
                        name = item.EmpName,
                        email = EmailForSearch,
                        WorkingHour = workinghours,
                        position = item.EmpPosition,
                    };
                    report.Add(indreport);
                }
                return View(report);
            }
            else
            {
                    double workinghours = 0.0;
                //int Arrlev = 0;
                    var findEmp = _db.EmployeeDetails.FirstOrDefault(c => c.EmpEmail == SearchEmail);
                    var tergetEmployee = _db.AttendanceModels.Where(c => (c.LeavingTime != default(DateTime)) && (c.Date.Date >= FromDate) && (c.Date.Date <= ToDate) && (c.Email == SearchEmail)).ToList();
                    foreach (var item2 in tergetEmployee)
                    {
                        workinghours += (item2.LeavingTime - item2.ArrivalTime).TotalHours;
                    }
                    var indreport = new AllEmployeeReportViewModel
                    {
                        image = findEmp.EmpImage,
                        name = findEmp.EmpName,
                        email = SearchEmail,
                        WorkingHour = workinghours,
                        position = findEmp.EmpPosition,
                    };
                    report.Add(indreport);

                return View(report);
            }
            
        }
    }
}
