using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreDietPlan.Models;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

namespace CoreDietPlan.Controllers
{
    public class ManageAdminsController : Controller
    {
        private readonly DietPlanDBContext _context;
       

    
        
        public ManageAdminsController(DietPlanDBContext context)
        {
            try
            {

                _context = context;
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // GET: ManageAdmins
        public async Task<IActionResult> Index()
        {
            try
            {
                if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                    return RedirectToAction("Login", "Admin");
                else
                    return View(await _context.AdminsTable.ToListAsync());
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }

        }

        // GET: ManageAdmins/Details/5
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                    return RedirectToAction("Login", "Admin");
                else
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var adminsTable = await _context.AdminsTable
                        .FirstOrDefaultAsync(m => m.AdminUsername == id);
                    if (adminsTable == null)
                    {
                        return NotFound();
                    }

                    return View(adminsTable);
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // GET: ManageAdmins/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                return RedirectToAction("Login", "Admin");
            else
            
                return View();
        }

        // POST: ManageAdmins/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("AdminUsername,AdminPassword,AdminName")] AdminsTable adminsTable)
        {try
            {
                if (ModelState.IsValid)
                {
                    adminsTable.Status = "Active";
                    _context.Add(adminsTable);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(adminsTable);
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }
        public async Task<IActionResult> Suspend(string id)
        {
            try
            {
                if (HttpContext.Session.GetString("LoggedIn") == null)

                    return RedirectToAction("Login", "Admin");
                else
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var Admins = await _context.AdminsTable.FindAsync(id);
                    if (Admins == null)
                    {
                        return NotFound();
                    }
                    return View(Admins);
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Suspend(string id, string Status)
        {
            try
            {
                if (Status == "Suspend")
                    Status = "Suspended";

                if (id == null)
                {
                    return NotFound();
                }

                var Admins = await _context.AdminsTable.FindAsync(id);
                if (Admins == null)
                {
                    return NotFound();
                }

                else
                    Admins.Status = Status;
                _context.Update(Admins);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // GET: ManageAdmins/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            try
            {
                if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                    return RedirectToAction("Login", "Admin");
                else
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var adminsTable = await _context.AdminsTable.FindAsync(id);
                    if (adminsTable == null)
                    {
                        return NotFound();
                    }
                    return View(adminsTable);
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // POST: ManageAdmins/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("AdminUsername,AdminPassword,AdminName")] AdminsTable adminsTable)
        {
            try
            {
                if (id != adminsTable.AdminUsername)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var admin = await _context.AdminsTable.FindAsync(id);
                        adminsTable.Status = admin.Status;
                        _context.Update(adminsTable);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!AdminsTableExists(adminsTable.AdminUsername))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                return View(adminsTable);
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // GET: ManageAdmins/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                    return RedirectToAction("Login", "Admin");
                else
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var adminsTable = await _context.AdminsTable
                        .FirstOrDefaultAsync(m => m.AdminUsername == id);
                    if (adminsTable == null)
                    {
                        return NotFound();
                    }

                    return View(adminsTable);
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // POST: ManageAdmins/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try { 
            var adminsTable = await _context.AdminsTable.FindAsync(id);
            _context.AdminsTable.Remove(adminsTable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }

        }

        private bool AdminsTableExists(string id)
        {
            return _context.AdminsTable.Any(e => e.AdminUsername == id);
        }


        public IActionResult Signout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Admin");

        }
    }
}
