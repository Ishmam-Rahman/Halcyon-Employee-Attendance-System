using HalcyonAttendance.Data;
using HalcyonAttendance.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HalcyonAttendance.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<HomeController> _logger;
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(ApplicationDbContext db, ILogger<HomeController> logger, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _logger = logger;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel rolee)
        {
            var result = await _roleManager.CreateAsync(new IdentityRole(rolee.RoleName));
            return RedirectToAction(nameof(Create));
        }
    }
}