using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoreDietPlan.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Html;
using System.Data.SqlClient;

namespace CoreDietPlan.Controllers
{
    public class AdminController : Controller
    {
        private readonly DietPlanDBContext _context;


        public AdminController(DietPlanDBContext context)
        {
            _context = context;
        }

        public  IActionResult Dashboard()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                return RedirectToAction("Login");
            else
            return View();
        }

        public IActionResult Login()
        {


            if (HttpContext.Session.GetString("FailedReason") == "Incorrect")
            {
                string errString = "<div class='alert alert-danger'><strong> Error </strong> Incorrect username or password entered  </div>";
                HtmlString errHtml = new HtmlString(errString);
                ViewBag.failedLogin = errHtml;
            }
            else if (HttpContext.Session.GetString("FailedReason") == "Suspended")
            {
                string errString = "<div class='alert alert-danger'><strong> Error </strong> Your account has been suspended, Try again in a while </div>";
                HtmlString errHtml = new HtmlString(errString);
                ViewBag.failedLogin = errHtml;
            }

            else
                ViewBag.failedLogin = null;

            return View();
        }

        [HttpPost]
        public IActionResult Login(string userID, string password)
        {

            try
            {
                var dbList = _context.AdminsTable.ToList();

                //getting the password of this user from database
                var pwd = dbList.Where(z => z.AdminUsername.ToLower() == userID.ToLower()).Select(x => x.AdminPassword).FirstOrDefault();

                if (pwd == password)
                {
                    if (dbList.Where(z => z.AdminUsername.ToLower() == userID.ToLower()).Select(x => x.Status).FirstOrDefault() == "Active")
                    {
                        HttpContext.Session.SetString("AdminLoggedIn", "True");
                        HttpContext.Session.SetString("Username", userID);
                        HttpContext.Session.SetString("FailedReason", "None");

                        return RedirectToAction("Dashboard");
                    }

                    else
                    {
                        HttpContext.Session.SetString("AdminLoggedIn", "False");
                        HttpContext.Session.SetString("FailedReason", "Suspended");

                        return RedirectToAction("Login");
                    }

                }
                else
                {
                    HttpContext.Session.SetString("LoggedIn", "False");
                    HttpContext.Session.SetString("FailedReason", "Incorrect");

                    return RedirectToAction("Login");

                }

            }

            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction( "Error", new { controllerName = controller, actionName = action });

            }

        }

        public IActionResult Signout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");

        }



        // GET: DietUsers
        public async Task<IActionResult> Index()
        {
            try
            {
                if (HttpContext.Session.GetString("AdminLoggedIn") != null)
                    return View(await _context.DietUsers.AsNoTracking().ToListAsync());

                else
                {
                    return RedirectToAction("Login");

                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        // GET: DietUsers/Details/5
        public async Task<IActionResult> Details(string id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var dietUsers = await _context.DietUsers
                    .FirstOrDefaultAsync(m => m.UserName == id);
                if (dietUsers == null)
                {
                    return NotFound();
                }

                return View(dietUsers);
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        // GET: DietUsers/Create
        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                return RedirectToAction("Login");
            else
            
                return View();
        }

        // POST: DietUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("UserName,UserFirstName,UserLastName,UserAge,UserEmail,UserGender,UserPassword")] DietUsers dietUsers, double UserWc, double UserWeight, double HeightMeter)
        {
            dietUsers.NewUser = true;
            try
            {

                if (ModelState.IsValid)
                {
                    double WeightPound = 0;
                    double HeightInch = 0;
                    UserClass newUser = new UserClass(dietUsers.UserName);
                    double height, weight;
               
                        height = HeightMeter;
                        HeightInch = height * 39.3701;
                    
                 
                        //if weight was already set in kg
                        WeightPound = UserWeight * 2.20462;
                        weight = UserWeight;
                    
                  
                    newUser.NewUserConfig(dietUsers, UserWc, WeightPound, HeightInch, weight, height);

                    return RedirectToAction(nameof(Index));
                }
                return View(dietUsers);
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        // GET: DietUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

           try{
                if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                    return RedirectToAction("Login");
                else
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var dietUsers = _context.DietUsers.Where(x=> x.UserName.ToLower() == id.ToLower()).AsNoTracking().FirstOrDefault();
                    if (dietUsers == null)
                    {
                        return NotFound();
                    }
                    string vegOptString = "";
                    if (dietUsers.IsVeg == true)

                    {
                        vegOptString = " <option Selected >True</option> <option> False </option> ";
                    }
                    else
                    {
                        vegOptString = " <option>True</option> <option Selected> False </option> ";

                    }
                    HtmlString optHtml = new HtmlString(vegOptString);
                    ViewBag.optVeg = optHtml;

                    string genderString = "";
                    if (dietUsers.UserGender == "Male")

                    {
                        genderString = " <option Selected>Male</option> <option>Female</option> ";
                    }
                    else
                    {
                        genderString = " <option>Male</option><option Selected>Female</option> ";

                    }
                    HtmlString genderHtml = new HtmlString(genderString);
                    ViewBag.Gender = genderHtml;

                    return View(dietUsers);
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        // POST: DietUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,UserFirstName,UserLastName,UserAge,UserEmail,UserGender,UserPassword,UserWeightCategory,ResetPasswordCode,NewUser,IsVeg,SetPicture")] DietUsers UPdietUsers)
        {
            try
            {
                if (id != UPdietUsers.UserName)
                {
                    return NotFound();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        var dietUser = _context.DietUsers.Where(x => x.UserName.ToLower() == id.ToLower()).AsNoTracking().FirstOrDefault();
                        UPdietUsers.Status = dietUser.Status;

                        _context.Update(UPdietUsers);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DietUsersExists(UPdietUsers.UserName))
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
                return View(UPdietUsers);
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        public async Task<IActionResult> Suspend(string id)
        {
            try
            {
                if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                    return RedirectToAction("Login");
                else
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var dietUsers = await _context.DietUsers.FindAsync(id);
                    if (dietUsers == null)
                    {
                        return NotFound();
                    }
                    string optString = "";
                    if (dietUsers.Status == "Active")
                    {
                        optString = " <option Selected >Active</option> <option> Suspend </option> ";
                    }
                    else
                    {
                        optString = " <option>Active</option> <option Selected> Suspend </option> ";

                    }
                        HtmlString optHtml = new HtmlString(optString);
                    ViewBag.opt = optHtml;
                    return View(dietUsers);
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }
    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Suspend(string id,string Status)
        {
            try
            {
                if (Status == "Suspend")
                    Status = "Suspended";

                if (id == null)
                {
                    return NotFound();
                }

                var dietUsers = await _context.DietUsers.FindAsync(id);
                if (dietUsers == null)
                {
                    return NotFound();
                }

                else
                    dietUsers.Status = Status;
                _context.Update(dietUsers);
                await _context.SaveChangesAsync();


                return RedirectToAction(nameof(Index));
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        // GET: DietUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                    return RedirectToAction("Login");
                else
                {
                    if (id == null)
                    {
                        return NotFound();
                    }

                    var dietUsers = await _context.DietUsers
                        .FirstOrDefaultAsync(m => m.UserName == id);
                    if (dietUsers == null)
                    {
                        return NotFound();
                    }

                    return View(dietUsers);
                }
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        // POST: DietUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            try
            {
                var dietUsers = await _context.DietUsers.FindAsync(id);
                _context.DietUsers.Remove(dietUsers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (SqlException)
            {
                string action = this.ControllerContext.RouteData.Values["action"].ToString();
                string controller = this.ControllerContext.RouteData.Values["controller"].ToString();
                return RedirectToAction("Error", new { controllerName = controller, actionName = action });

            }
        }

        private bool DietUsersExists(string id)
        {
            
                return _context.DietUsers.Any(e => e.UserName == id);
          
        }

        public IActionResult DashboardRedirect (string page)
        {
            
            if (HttpContext.Session.GetString("AdminLoggedIn") == null)

                return RedirectToAction("Login");
            else
            {
                var username = HttpContext.Session.GetString("Username");

                return RedirectToAction("Index", page);

            }
        }

        public IActionResult Error(string controllerName = "", string actionName = "")
        {

            if (controllerName != "" && actionName != "")
            {
                ViewBag.actionName = actionName;
                ViewBag.controllerName = controllerName;
                ViewBag.set = "True";
            }
            else
            {
                ViewBag.actionName = "Login";
                ViewBag.controllerName = "Admin";
                ViewBag.set = "false";

            }
            return View();

        }
    }
}
