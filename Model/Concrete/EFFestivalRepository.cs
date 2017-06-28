using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Model.Abstract;
using Model.Models;
using System.Data.Entity;

namespace Model.Concrete
{
    public class EFFestivalRepository : IFestivalRepository
    {
        private EFDbContext _dbContext = new EFDbContext();

        public IEnumerable<Festival> Festivals
        {
            get { return _dbContext.Festivals.ToList(); }
        }

        public IEnumerable<Performance> Performances
        {
            get { return _dbContext.Performances.ToList(); }
        }

        public IEnumerable<Stage> Stages
        {
            get { return _dbContext.Stages.ToList(); }
        }
        
        public IEnumerable<Ticket> Tickets
        {
            get { return _dbContext.Tickets.ToList(); }
        }

        public bool AddTicket(string userId, string UserEmail, int festivalId)
        {
            Festival festival = _dbContext.Festivals.FirstOrDefault(f => f.Id == festivalId);
            int totalTickets = festival.MaxTicket;
            int soldTickets = _dbContext.Tickets.Count(t => t.Festival.Id == festival.Id);
            int ticketsLeft = totalTickets - soldTickets;

            if (ticketsLeft < 1)
            {
                return false;
            }

            Ticket ticket = new Ticket();
            ticket.UserId = userId;
            ticket.UserEmail = UserEmail;
            ticket.Festival = festival;
            ticket.BuyDateTime = DateTime.Now;

            _dbContext.Tickets.Add(ticket);
            _dbContext.SaveChanges();

            return true;
        }

        public List<Performance> PerformancesByFestival(Festival festival)
        {
            return _dbContext.Performances.Where(p => p.Stage.Festival.Id == festival.Id).Include(p => p.Stage).ToList();
        }

        public Ticket UserTicketById(int ticketId)
        {
            return _dbContext.Tickets.Include(t => t.Festival).FirstOrDefault(t => t.TicketId == ticketId);
        }

        public IEnumerable<Ticket> UserTickets(string userId)
        {
            return _dbContext.Tickets.Where(t => t.UserId == userId).Include(t => t.Festival).OrderByDescending(t => t.BuyDateTime);
        }
    }
}