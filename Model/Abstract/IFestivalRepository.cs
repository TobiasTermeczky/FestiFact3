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
        IEnumerable<Ticket> UserTickets(string userId);
        bool AddTicket(string userId, string UserEmail, int festivalId);
        Ticket UserTicketById(int ticketId);

    }
}