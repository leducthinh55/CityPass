using Core.Entities;
using Infrastructure.Infrastructure;
using Infrastructure.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;

namespace Service
{
    public interface ITicketService
    {
        void AddTicket(Ticket Ticket);
        void AddRangeTicket(IEnumerable<Ticket> Tickets);
        void UpdateTicket(Ticket Ticket);
        void DeleteTicket(Expression<Func<Ticket, bool>> where);
        void DeleteTicket(Ticket Ticket);
        Ticket GetTicketById(Guid Id);
        Ticket GetTicket(Expression<Func<Ticket, bool>> where);
        IQueryable<Ticket> GetAllTicket(Expression<Func<Ticket, bool>> where, params Expression<Func<Ticket, object>>[] includes);
        IQueryable<Ticket> GetAllTicket();
        Task<bool> SaveTicket();
    }

    public class TicketService : ITicketService
    {
        private readonly ITicketRepository _iRespository;
        public TicketService(ITicketRepository iRespository)
        {
            _iRespository = iRespository;
        }

        public void AddTicket(Ticket Ticket)
        {
            _iRespository.Add(Ticket);
        }

        public void DeleteTicket(Expression<Func<Ticket, bool>> where)
        {
            _iRespository.Delete(where);
        }

        public void UpdateTicket(Ticket Ticket)
        {
            _iRespository.Update(Ticket);
        }

        public Ticket GetTicketById(Guid Id)
        {
            return _iRespository.GetById(Id);
        }

        public Ticket GetTicket(Expression<Func<Ticket, bool>> where)
        {
            return _iRespository.Get(where);
        }

        public IQueryable<Ticket> GetAllTicket(Expression<Func<Ticket, bool>> where, params Expression<Func<Ticket, object>>[] includes)
        {
            return _iRespository.GetAll(where, includes);
        }

        public IQueryable<Ticket> GetAllTicket()
        {
            return _iRespository.GetAll();
        }

        public async Task<bool> SaveTicket()
        {
            return await _iRespository.Commit();
        }

        public void DeleteTicket(Ticket Ticket)
        {
            _iRespository.Delete(Ticket);
        }

        public void AddRangeTicket(IEnumerable<Ticket> Tickets)
        {
            _iRespository.AddRange(Tickets);
        }
    }
}
