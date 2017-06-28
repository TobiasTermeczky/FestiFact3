using FestiFact3.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Model.Abstract;
using Model.Models;
using QRCoder;
using Rotativa;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FestiFact3.Controllers
{
    public class TicketController : Controller
    {
        private IFestivalRepository repo;
        private ApplicationUserManager userManager;


        public TicketController(IFestivalRepository repository)
        {
            repo = repository;
        }

        // GET: Ticket
        public ActionResult Index()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            this.userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = userManager.FindById(User.Identity.GetUserId());

            IEnumerable<Ticket> tickets = repo.UserTickets(user.Id);

            return View(tickets);
        }

        public ActionResult BuyTicket(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            this.userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));

            var user = userManager.FindById(User.Identity.GetUserId());

            bool result = repo.AddTicket(user.Id, user.Email, id);

            if (result == false)
            {
                return RedirectToAction("Detail", "Festival", new { id });
            }
            else { return RedirectToAction("Index"); }    
        }

        public ActionResult Details(int id)
        {
            this.userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(new ApplicationDbContext()));
            var user = userManager.FindById(User.Identity.GetUserId());

            Ticket ticket = repo.UserTicketById(id);

            if (!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Account");
            }
            else if (user.Id != ticket.UserId)
            {
                return RedirectToAction("List");
            }

            string code = ticket.UserEmail + "|" + ticket.Festival.Name + "|" + ticket.TicketId + "|" + ticket.BuyDateTime;
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(code, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            var bitMapBytes = BitmapToBytes(qrCodeImage);
            ViewBag.qr = bitMapBytes;

            return new ViewAsPdf(ticket)
            {
                FileName = "Ticket.pdf"
            };
        }

        // This method is for converting bitmap into a byte array
        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}