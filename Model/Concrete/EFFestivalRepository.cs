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

        public bool AddFestival(Festival festival)
        {
            _dbContext.Festivals.Add(festival);
            _dbContext.SaveChanges();
            return true;
        }

        public bool AddPerformance(Performance performance)
        {
            List<Performance> performances = PerformancesByStage(performance.Stage);
            Festival festival = performance.Stage.Festival;
            if(performance.StartTime < festival.StartTime)
            {
                return false;
            }
            if (performance.EndTime > festival.EndTime)
            {
                return false;
            }
            if (performance.EndTime < performance.StartTime)
            {
                return false;
            }
            foreach (Performance p in performances)
            {
                if (performance.StartTime < p.EndTime && p.StartTime < performance.EndTime)
                {
                    return false;
                }
            }
            _dbContext.Performances.Add(performance);
            _dbContext.SaveChanges();
            return true;
        }

        public bool AddRating(int festivalId, string vote, string userId)
        {
            int intVote = 0;
            if (vote == "up")
            {
                intVote = 1;
            } 
            Rating oldRating = _dbContext.Ratings.FirstOrDefault(r => r.Festival.Id == festivalId && r.UserId == userId);
            if(oldRating == null)
            {
                Rating newRating = new Rating();
                newRating.Festival = FestivalById(festivalId);
                newRating.Rate = intVote;
                newRating.UserId = userId;

                _dbContext.Ratings.Add(newRating);
                _dbContext.SaveChanges();
            }
            else
            {
                oldRating.Rate = intVote;

                _dbContext.Ratings.Attach(oldRating);
                _dbContext.Entry(oldRating).State = EntityState.Modified;
                _dbContext.SaveChanges();
                return true;
            }
            return true;
        }

        public bool AddStage(Stage stage)
        {
            _dbContext.Stages.Add(stage);
            _dbContext.SaveChanges();
            return true;
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

        public bool DeleteFestival(Festival festival)
        {
            _dbContext.Festivals.Remove(festival);
            _dbContext.SaveChanges();
            return true;
        }

        public bool DeletePerformance(Performance performance)
        {
            _dbContext.Performances.Remove(performance);
            _dbContext.SaveChanges();
            return true;
        }

        public bool DeleteStage(Stage stage)
        {
            _dbContext.Stages.Remove(stage);
            _dbContext.SaveChanges();
            return true;
        }

        public int Downvotes(int festivalId)
        {
            return _dbContext.Ratings.Count(r => r.Festival.Id == festivalId && r.Rate == 0);
        }

        public bool EditFestival(Festival festival)
        {
            _dbContext.Festivals.Attach(festival);
            _dbContext.Entry(festival).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return true;
        }

        public bool EditStage(Stage stage)
        {
            _dbContext.Stages.Attach(stage);
            _dbContext.Entry(stage).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return true;
        }

        public Festival FestivalById(int festivalId)
        {
            return _dbContext.Festivals.FirstOrDefault(f => f.Id == festivalId);
        }

        public IEnumerable<Festival> FestivalByOrganiser(string userId)
        {
            return _dbContext.Festivals.Where(x => x.OrganizerID == userId);
        }

        public Performance PerformanceById(int PerformanceId)
        {
            return _dbContext.Performances.Include(s => s.Stage).Include(f => f.Stage.Festival).FirstOrDefault(p => p.Id == PerformanceId);
        }

        public List<Performance> PerformancesByFestival(Festival festival)
        {
            return _dbContext.Performances.Where(p => p.Stage.Festival.Id == festival.Id).Include(p => p.Stage).ToList();
        }

        public List<Performance> PerformancesByStage(Stage stage)
        {
            return _dbContext.Performances.Where(p => p.Stage.Id == stage.Id).ToList();
        }

        public List<Festival> SearchFestival(string search)
        {
            IEnumerable<Festival> festivals = _dbContext.Festivals.ToList();
            List<Festival> returnFestivals = new List<Festival>();
            foreach (Festival festival in Festivals) { 
                if (festival.Name.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 
                    || festival.Genre.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0 
                    || festival.Description.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0
                    || festival.Location.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    returnFestivals.Add(festival);
                }
            }
            return returnFestivals;
        }

        public Stage StageById(int StageId)
        {
            return _dbContext.Stages.Include(f => f.Festival).FirstOrDefault(f => f.Id == StageId);
        }

        public List<Stage> StagesByFestival(Festival festival)
        {
            return _dbContext.Stages.Where(p => p.Festival.Id == festival.Id).ToList();
        }

        public int TicketsLeft(int festivalId)
        {
            Festival festival = _dbContext.Festivals.FirstOrDefault(f => f.Id == festivalId);
            int totalTickets = festival.MaxTicket;
            int soldTickets = _dbContext.Tickets.Count(t => t.Festival.Id == festival.Id);
            int ticketsLeft = totalTickets - soldTickets;

            return ticketsLeft;
        }

        public int TicketsSold(int festivalId)
        {
            return _dbContext.Tickets.Count(t => t.Festival.Id == festivalId);
        }

        public int TotalPerformances(int festivalId)
        {
            return _dbContext.Performances.Count(p => p.Stage.Festival.Id == festivalId);
        }

        public int TotalStages(int festivalId)
        {
            return _dbContext.Stages.Count(p => p.Festival.Id == festivalId);
        }

        public int Upvotes(int festivalId)
        {
            return _dbContext.Ratings.Count(r => r.Festival.Id == festivalId && r.Rate == 1);
        }

        public Ticket UserTicketById(int ticketId)
        {
            return _dbContext.Tickets.Include(t => t.Festival).FirstOrDefault(t => t.TicketId == ticketId);
        }

        public IEnumerable<Ticket> UserTickets(string userId)
        {
            return _dbContext.Tickets.Where(t => t.UserId == userId).Include(t => t.Festival).OrderByDescending(t => t.BuyDateTime);
        }

        public void DropDatabase()
        {
            _dbContext.Database.Delete();
        }

        public int StagePerformanceCount(int stageId)
        {
            return _dbContext.Performances.Count(p => p.Stage.Id == stageId);
        }
    }
}