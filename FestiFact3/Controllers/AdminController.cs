using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FestiFact3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace FestiFact3.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult PendingOrganisers()
        {
            // Check if user is authorized as a manager
            if (!User.IsInRole("Admin")) { return View("NotAuthorized"); }

            var context = new ApplicationDbContext();

            IEnumerable<ApplicationUser> allUsers = context.Users.Where(x => x.OrganiserRequested).ToList();

            return View(allUsers);
        }

        public ActionResult ListOrganisers()
        {
            // Check if user is authorized as a manager
            if (!User.IsInRole("Admin")) { return View("NotAuthorized"); }

            var context = new ApplicationDbContext();
            IEnumerable<ApplicationUser> list = context.Users.ToList();
            List<ApplicationUser> organisers = new List<ApplicationUser>();
            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            foreach (ApplicationUser appuser in list)
            {
                if (userManager.IsInRole(appuser.Id, "organiser"))
                {
                    organisers.Add(appuser);
                }
            }

            return View(organisers);
        }

        [HttpGet]
        public ActionResult EditOrganiser(string id)
        {
            // Check if user is authorized as a manager
            if (!User.IsInRole("Admin")) { return View("NotAuthorized"); }

            var userManager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            ApplicationUser user = userManager.Users.Where(x => x.Id == id).ToList().FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public ActionResult EditOrganiser(ApplicationUser user)
        {
            // Check if user is authorized as a manager
            if (!User.IsInRole("Admin")) { return View("NotAuthorized"); }

            if (ModelState.IsValid)
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var u = userManager.Users.FirstOrDefault(x => x.Id == user.Id);

                u.Email = user.Email;
                u.PhoneNumber = user.PhoneNumber;


                userManager.Update(u);

                return RedirectToAction("ListOrganisers");
            }
            else
            {
                // There is a validation error
                return View();
            }
        }
    }
}