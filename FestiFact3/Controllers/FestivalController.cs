using Model.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Model.Models;
using Model.Abstract;

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
            IEnumerable<Festival> festivals;

            if (search != null)
            {

                DateTime searchDate;
                if (DateTime.TryParse(search, out searchDate))
                {
                    festivals = repo.Festivals
                        .Where(f => f.StartTime == searchDate);
                }
                else
                {
                    festivals = repo.Festivals
                        .Where(f => f.Name.Contains(search) ||
                        f.Location.Contains(search) ||
                        f.Genre.Contains(search) || 
                        f.Description.Contains(search));
                }
            }
            else
            {
                festivals = repo.Festivals;
            }

            return View(festivals);
        }

        public ActionResult Detail(int id)
        {
            Festival festival = repo.Festivals.FirstOrDefault(f => f.Id == id);
            int totalTickets = festival.MaxTicket;
            int soldTickets = repo.Tickets.Count(t => t.Festival.Id == festival.Id);
            int ticketsLeft = totalTickets - soldTickets;

            ViewBag.ticketsLeft = ticketsLeft;
            return View(festival);
        }

        public ActionResult Performances(int id)
        {
            Festival festival = repo.Festivals.FirstOrDefault(f => f.Id == id);
            List<Performance> performances = repo.PerformancesByFestival(festival);
            ViewBag.festival = festival.Name;
            return View(performances);
        }
    }
}