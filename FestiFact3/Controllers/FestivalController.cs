using Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Models;
using Model.Abstract;
using FestiFact3.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;

namespace FestiFact3.Controllers
{
    public class FestivalController : Controller
    {
        private IFestivalRepository repo;

        public FestivalController(IFestivalRepository repository)
        {
            repo = repository;
        }

        // GET: Festival
        public ViewResult Index(string search)
        {
            List<Festival> festivals;

            if (search != null)
            {

                DateTime searchDate;
                if (DateTime.TryParse(search, out searchDate))
                {
                    festivals = repo.Festivals
                        .Where(f => f.StartTime == searchDate).ToList();
                }
                else
                {
                    festivals = repo.SearchFestival(search);
                }
            }
            else
            {
                festivals = repo.Festivals.ToList();
            }

            return View(festivals);
        }

        public ActionResult Detail(int id)
        {
            ViewBag.ticketsLeft = repo.TicketsLeft(id);
            ViewBag.Upvotes = repo.Upvotes(id);
            ViewBag.Downvotes = repo.Downvotes(id);
            return View(repo.FestivalById(id));
        }

        public ActionResult Performances(int id)
        {
            Festival festival = repo.FestivalById(id);
            List<Performance> performances = repo.PerformancesByFestival(festival);
            ViewBag.festival = festival.Name;
            return View(performances);
        }

        public ActionResult RateFestival(int id, string vote)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(User.Identity.GetUserId());

            repo.AddRating(id, vote, user.Id);

            return RedirectToAction("Detail", new { id = id });
        }
    }
}