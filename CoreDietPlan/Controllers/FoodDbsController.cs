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
    public class FoodDbsController : Controller
    {
        private readonly DietPlanDBContext _context;

        public FoodDbsController(DietPlanDBContext context)
        {
            _context = context;
        }

        // GET: FoodDbs
        public async Task<IActionResult> Index()
        {
            try
            {
                if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                    return RedirectToAction("Login", "Admin");
                else
                    return View(await _context.FoodDb.ToListAsync());
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // GET: FoodDbs/Details/5
        public async Task<IActionResult> Details(int? id)
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

                    var foodDb = await _context.FoodDb
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (foodDb == null)
                    {
                        return NotFound();
                    }

                    return View(foodDb);
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // GET: FoodDbs/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                return RedirectToAction("Login", "Admin");
            else

                return View();
        }

        // POST: FoodDbs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,servqty,serv,group,food")] FoodDb foodDb)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Add(foodDb);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(foodDb);
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // GET: FoodDbs/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

                    var foodDb = await _context.FoodDb.FindAsync(id);
                    if (foodDb == null)
                    {
                        return NotFound();
                    }
                    return View(foodDb);
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // POST: FoodDbs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,servqty,serv,group,food")] FoodDb foodDb)
        {
            try
            {
                if (id != foodDb.Id)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(foodDb);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!FoodDbExists(foodDb.Id))
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
                return View(foodDb);
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // GET: FoodDbs/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

                    var foodDb = await _context.FoodDb
                        .FirstOrDefaultAsync(m => m.Id == id);
                    if (foodDb == null)
                    {
                        return NotFound();
                    }

                    return View(foodDb);
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", "Admin", new { controllerName = controller, actionName = action });

            }
        }

        // POST: FoodDbs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var foodDb = await _context.FoodDb.FindAsync(id);
                _context.FoodDb.Remove(foodDb);
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

        private bool FoodDbExists(int id)
        {
            return _context.FoodDb.Any(e => e.Id == id);
        }
    }
}
