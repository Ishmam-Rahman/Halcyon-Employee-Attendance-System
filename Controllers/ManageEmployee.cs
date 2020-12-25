using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using HalcyonAttendance.Data;
using HalcyonAttendance.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using HalcyonAttendance.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using HalcyonAttendance.Controllers;
using Microsoft.Data.SqlClient;

namespace Practicum_Project_HAS.Controllers
{
    //[Authorize]
    public class ManageEmployee : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IWebHostEnvironment _he;
        private readonly ILogger<HomeController> _logger;
        public ManageEmployee(ApplicationDbContext db, IWebHostEnvironment he, ILogger<HomeController> logger)
        {
            _db = db;
            _he = he;
            _logger = logger;
        }

        
        public IActionResult ShowAllEmp()
        {
            return View(_db.EmployeeDetails.ToList());
        }


        [HttpPost]
        public IActionResult ShowAllEmp(string email)
        {
            if(email == null)
            {
                var searchbyemail = _db.EmployeeDetails.ToList();
                return View(searchbyemail);
            }
            else
            {
                var searchbyemail = _db.EmployeeDetails.Where(c => c.EmpEmail == email);
                return View(searchbyemail.ToList());
            } 
        }

        //Get create method
        public IActionResult Create()
        {
            return View();
        }

        //Post create method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(EmployeeViewModel employeeDetails)
        {
            if (ModelState.IsValid)
            {
                //Checking Duplicate Email
                var findemployee = _db.EmployeeDetails.FirstOrDefault(c => c.EmpEmail == employeeDetails.EmpEmail);
                if (findemployee != null)
                {
                    ViewBag.message = "Employee with this email already exist!!!";
                    return View(employeeDetails);
                }

                string FileName = UploadedFile(employeeDetails);
                var Employee = new EmployeeDetails
                {
                    EmpName = employeeDetails.EmpName,
                    EmpEmail = employeeDetails.EmpEmail,
                    EmpPassword = employeeDetails.EmpPassword,
                    EmpPhone = employeeDetails.EmpPhone,
                    EmpPosition = employeeDetails.EmpPosition,
                    EmpSalary = employeeDetails.EmpSalary,
                    EmpImage = FileName,
                    LoginStatus = false,
                };
                _db.EmployeeDetails.Add(Employee);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(ShowAllEmp));
            }
            return View(employeeDetails);
        }

        private string UploadedFile(EmployeeViewModel model)
        {
            string uniqueFileName = null;
            if (model.EmpImage != null)
            {
                //Console.WriteLine("UploadedFile");
                //Console.WriteLine("E:\\Practicum Project\\Practicum Project HAS\\wwwroot");
                string webpath = "E:\\Practicum Project\\HalcyonAttendance\\wwwroot";
                string uploadsFolder = Path.Combine(webpath, "Images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.EmpImage.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.EmpImage.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }


        //Get Edit method
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employeedetal = _db.EmployeeDetails.Find(id);
            return View(employeedetal);
        }

        //Post Edit method
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EmployeeDetails employeeDetails)
        {
            if (ModelState.IsValid)
            {
                _db.EmployeeDetails.Update(employeeDetails);
                var tergetemployee = _db.AttendanceModels.Where(c => c.Email == employeeDetails.EmpEmail);

                foreach(AttendanceModel item in tergetemployee)
                {
                    item.Name = employeeDetails.EmpName;
                    _db.AttendanceModels.Update(item);
                }
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(ShowAllEmp));
            }
            return View(employeeDetails);
        }

        //Get details Method
        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var employee = _db.EmployeeDetails.Find(id);
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }


        //Get delete method
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeedetails = _db.EmployeeDetails.Find(id);
            if (employeedetails == null)
            {
                return NotFound();
            }
            return View(employeedetails);

        }

        [HttpPost]
        public async Task<IActionResult> Delete(int? id, EmployeeDetails employeeDetails)
        {
            var employe = _db.EmployeeDetails.Find(id);
            _db.EmployeeDetails.Remove(employe);
            //_db.AttendanceModels.Where(c => c.Email == employe.EmpEmail).DeleteFromQuery();
            _db.AttendanceModels.RemoveRange(_db.AttendanceModels.Where(c => c.Email == employe.EmpEmail));
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(ShowAllEmp));
        }

    }

    internal class IwebHostEnvironment
    {
        public static string WebRootPath { get; internal set; }
    }
}
