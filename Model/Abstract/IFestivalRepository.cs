using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Model.Abstract
{
    public interface IFestivalRepository
    {
        IEnumerable<Festival> Festivals { get; }
        IEnumerable<Performance> Performances { get; }
        IEnumerable<Stage> Stages { get; }
        IEnumerable<Ticket> Tickets { get; }
        List<Performance> PerformancesByFestival(Festival festival);
        List<Stage> StagesByFestival(Festival festival);
        List<Performance> PerformancesByStage(Stage stage);
        IEnumerable<Ticket> UserTickets(string userId);
        bool AddTicket(string userId, string UserEmail, int festivalId);
        Ticket UserTicketById(int ticketId);
        IEnumerable<Festival> FestivalByOrganiser(string userId);
        List<Festival> SearchFestival(string search);
        int TicketsLeft(int festivalId);
        int TicketsSold(int festivalId);
        int TotalPerformances(int festivalId);
        int TotalStages(int festivalId);
        Festival FestivalById(int festivalId);
        Stage StageById(int StageId);
        Performance PerformanceById(int PerformanceId);
        bool AddFestival(Festival festival);
        bool EditFestival(Festival festival);
        bool DeleteFestival(Festival festival);
        bool AddStage(Stage stage);
        bool EditStage(Stage stage);
        bool DeleteStage(Stage stage);
        string AddPerformance(Performance performance);
        string EditPerformance(Performance performance);
        bool DeletePerformance(Performance performance);
        bool AddRating(int festivalId, string vote, string userId);
        int Upvotes(int festivalId);
        int Downvotes(int festivalId);
    }
}