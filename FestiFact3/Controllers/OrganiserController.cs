using FestiFact3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Model.Abstract;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FestiFact3.Controllers
{
    public class OrganiserController : Controller
    {

        private IFestivalRepository repo;

        public OrganiserController(IFestivalRepository repository)
        {
            repo = repository;
        }

        // GET: Organiser
        [Authorize(Roles = "organiser")]
        public ActionResult Index()
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = userManager.FindById(User.Identity.GetUserId());

            IEnumerable<Festival> festivals = repo.FestivalByOrganiser(user.Id);

            return View(festivals);
        }

        [Authorize(Roles = "organiser")]
        public ActionResult DetailFestival(int id)
        {
            Festival festival = repo.FestivalById(id);
            IEnumerable<Performance> performances = repo.PerformancesByFestival(festival);
            IEnumerable<Stage> stages = repo.StagesByFestival(festival);

            ViewBag.stages = stages;
            ViewBag.performances = performances;
            ViewBag.soldTickets = repo.TicketsSold(id);
            ViewBag.ticketsLeft = repo.TicketsLeft(id);
            ViewBag.totalPerformances = repo.TotalPerformances(id);
            ViewBag.totalStages = repo.TotalStages(id);
            return View(festival);
        }

        [Authorize(Roles = "organiser")]
        public ActionResult CreateFestival()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "organiser")]
        public ActionResult CreateFestival(Festival festival)
        {
            if (ModelState.IsValid)
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userManager.FindById(User.Identity.GetUserId());

                festival.OrganizerID = user.Id;

                repo.AddFestival(festival);

                return RedirectToAction("Index");
            }
            else
            {
                // There is a validation error
                return View();
            }
        }

        [Authorize(Roles = "organiser")]
        public ActionResult EditFestival(int id)
        {
            return View(repo.FestivalById(id));
        }

        [HttpPost]
        [Authorize(Roles = "organiser")]
        public ActionResult EditFestival(Festival festival)
        {
            if (ModelState.IsValid)
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userManager.FindById(User.Identity.GetUserId());

                if (user.Id != festival.OrganizerID)
                {
                    return View("NotAuthorized");
                }

                repo.EditFestival(festival);
                return RedirectToAction("Index");
            }
            else
            {
                // There is a validation error
                return View();
            }
        }

        [Authorize(Roles = "organiser")]
        public ActionResult DeleteFestival(int id)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(User.Identity.GetUserId());
            Festival festival = repo.FestivalById(id);

            if (user.Id != festival.OrganizerID)
            {
                return View("NotAuthorized");
            }

            repo.DeleteFestival(festival);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "organiser")]
        public ActionResult CreateStage(int id)
        {
            ViewBag.festival = repo.FestivalById(id);
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "organiser")]
        public ActionResult CreateStage(Stage stage, int festivalId)
        {
            if (ModelState.IsValid)
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userManager.FindById(User.Identity.GetUserId());
                Festival festival = repo.FestivalById(festivalId);

                if (user.Id != festival.OrganizerID)
                {
                    return View("NotAuthorized");
                }

                stage.Festival = festival;

                repo.AddStage(stage);

                return RedirectToAction("DetailFestival", new { stage.Festival.Id });
            }
            else
            {
                // There is a validation error
                return View();
            }
        }

        [Authorize(Roles = "organiser")]
        public ActionResult EditStage(int id)
        {
            Stage stage = repo.StageById(id);
            ViewBag.festival = stage.Festival;
            return View(repo.StageById(id));
        }

        [HttpPost]
        [Authorize(Roles = "organiser")]
        public ActionResult EditStage(Stage stage, int festivalId)
        {
            if (ModelState.IsValid)
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userManager.FindById(User.Identity.GetUserId());
                Festival festival = repo.FestivalById(festivalId);

                if (user.Id != festival.OrganizerID)
                {
                    return View("NotAuthorized");
                }

                stage.Festival = festival;

                repo.EditStage(stage);
                return RedirectToAction("DetailFestival", new { stage.Festival.Id });
            }
            else
            {
                // There is a validation error
                return View();
            }
        }

        [Authorize(Roles = "organiser")]
        public ActionResult DeleteStage(int id)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(User.Identity.GetUserId());
            Stage stage = repo.StageById(id);

            if (user.Id != stage.Festival.OrganizerID)
            {
                return View("NotAuthorized");
            }

            int FestivalReturnId = stage.Festival.Id;
            repo.DeleteStage(stage);

            return RedirectToAction("DetailFestival", new { id = FestivalReturnId });
        }

        [Authorize(Roles = "organiser")]
        public ActionResult CreatePerformance(int id)
        {
            List<Stage> stages = repo.StagesByFestival(repo.FestivalById(id));
            List<SelectListItem> stagelist = new List<SelectListItem>();
            foreach(Stage stage in stages)
            {
                var stageListItem = new SelectListItem
                {
                    Text = stage.Name,
                    Value = stage.Id.ToString()
                };
                stagelist.Add(stageListItem);
            }
            SelectList stageSelectList = new SelectList(stagelist, "Value", "Text", 1);
            ViewBag.stageSelectList = stageSelectList;
            ViewBag.festival = repo.FestivalById(id);
            ViewBag.overlap = "";
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "organiser")]
        public ActionResult CreatePerformance(Performance performance, int festivalId, string stageId)
        {
            if (ModelState.IsValid)
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userManager.FindById(User.Identity.GetUserId());
                Festival festival = repo.FestivalById(festivalId);

                if (user.Id != festival.OrganizerID)
                {
                    return View("NotAuthorized");
                }

                Stage stage = repo.StageById(Int32.Parse(stageId));
                performance.Stage = stage;

                string overlapResult = repo.AddPerformance(performance);
                if(overlapResult != "Success")
                {
                    //Overlap
                    List<Stage> stages = repo.StagesByFestival(repo.FestivalById(festivalId));
                    List<SelectListItem> stagelist = new List<SelectListItem>();
                    foreach (Stage stageIndu in stages)
                    {
                        var stageListItem = new SelectListItem
                        {
                            Text = stageIndu.Name,
                            Value = stageIndu.Id.ToString()
                        };
                        stagelist.Add(stageListItem);
                    }
                    SelectList stageSelectList = new SelectList(stagelist, "Value", "Text", 1);
                    ViewBag.stageSelectList = stageSelectList;
                    ViewBag.festival = repo.FestivalById(festivalId);
                    ViewBag.overlap = overlapResult;
                    return View();
                }
                return RedirectToAction("DetailFestival", new { id = festivalId });
            }
            else
            {
                //There is a validation error
                List<Stage> stages = repo.StagesByFestival(repo.FestivalById(festivalId));
                List<SelectListItem> stagelist = new List<SelectListItem>();
                foreach (Stage stageIndu in stages)
                {
                    var stageListItem = new SelectListItem
                    {
                        Text = stageIndu.Name,
                        Value = stageIndu.Id.ToString()
                    };
                    stagelist.Add(stageListItem);
                }
                SelectList stageSelectList = new SelectList(stagelist, "Value", "Text", 1);
                ViewBag.stageSelectList = stageSelectList;
                ViewBag.festival = repo.FestivalById(festivalId);
                ViewBag.overlap = "";
                return View();
            }
            
        }

        [Authorize(Roles = "organiser")]
        public ActionResult EditPerformance(int id)
        {
            Performance performance = repo.PerformanceById(id);
            List<Stage> stages = repo.StagesByFestival(performance.Stage.Festival);
            List<SelectListItem> stagelist = new List<SelectListItem>();
            foreach (Stage stage in stages)
            {
                var stageListItem = new SelectListItem
                {
                    Text = stage.Name,
                    Value = stage.Id.ToString()
                };
                stagelist.Add(stageListItem);
            }
            SelectList stageSelectList = new SelectList(stagelist, "Value", "Text", 1);
            ViewBag.stageSelectList = stageSelectList;
            ViewBag.festival = performance.Stage.Festival;
            ViewBag.overlap = "";
            return View(repo.PerformanceById(id));
        }

        [HttpPost]
        [Authorize(Roles = "organiser")]
        public ActionResult EditPerformance(Performance performance, int festivalId, string stageId)
        {
            if (ModelState.IsValid)
            {
                var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var user = userManager.FindById(User.Identity.GetUserId());
                Festival festival = repo.FestivalById(festivalId);

                if (user.Id != festival.OrganizerID)
                {
                    return View("NotAuthorized");
                }

                Stage stage = repo.StageById(Int32.Parse(stageId));
                performance.Stage = stage;

                string overlapResult = repo.EditPerformance(performance);
                if (overlapResult != "Success")
                {
                    //Overlap
                    List<Stage> stages = repo.StagesByFestival(repo.FestivalById(festivalId));
                    List<SelectListItem> stagelist = new List<SelectListItem>();
                    foreach (Stage stageIndu in stages)
                    {
                        var stageListItem = new SelectListItem
                        {
                            Text = stageIndu.Name,
                            Value = stageIndu.Id.ToString()
                        };
                        stagelist.Add(stageListItem);
                    }
                    SelectList stageSelectList = new SelectList(stagelist, "Value", "Text", 1);
                    ViewBag.stageSelectList = stageSelectList;
                    ViewBag.festival = repo.FestivalById(festivalId);
                    ViewBag.overlap = overlapResult;
                    return View(performance);
                }
                return RedirectToAction("DetailFestival", new { id = festivalId });
            }
            else
            {
                //There is a validation error
                List<Stage> stages = repo.StagesByFestival(repo.FestivalById(festivalId));
                List<SelectListItem> stagelist = new List<SelectListItem>();
                foreach (Stage stageIndu in stages)
                {
                    var stageListItem = new SelectListItem
                    {
                        Text = stageIndu.Name,
                        Value = stageIndu.Id.ToString()
                    };
                    stagelist.Add(stageListItem);
                }
                SelectList stageSelectList = new SelectList(stagelist, "Value", "Text", 1);
                ViewBag.stageSelectList = stageSelectList;
                ViewBag.festival = repo.FestivalById(festivalId);
                ViewBag.overlap = "";
                return View(performance);
            }

        }



        [Authorize(Roles = "organiser")]
        public ActionResult DeletePerformance(int id)
        {
            var userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(User.Identity.GetUserId());
            Performance performance = repo.PerformanceById(id);

            if (user.Id != performance.Stage.Festival.OrganizerID)
            {
                return View("NotAuthorized");
            }

            int FestivalReturnId = performance.Stage.Festival.Id;
            repo.DeletePerformance(performance);

            return RedirectToAction("DetailFestival", new { id = FestivalReturnId });
        }
    }
}