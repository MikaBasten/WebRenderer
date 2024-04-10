using Core.IRepository;
using Core.Models;
using Dall.DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dall.Repository
{
    public class SessionRepository : ISessionRepository
    {
        private readonly YourDbContext _dbContext;

        public SessionRepository(YourDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(Session session)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            _dbContext.Sessions.Add(session);
            _dbContext.SaveChanges();
        }
        public void Update(Session session)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            _dbContext.Sessions.Update(session);
            _dbContext.SaveChanges(); // Save changes to the database
        }
        public void Delete(Session session)
        {
            if (session == null)
                throw new ArgumentNullException(nameof(session));

            _dbContext.Sessions.Remove(session);
            _dbContext.SaveChanges();
        }

        public Session GetById(int id)
        {
            return _dbContext.Sessions
                .Include(s => s.Users) // Include users associated with the session
                .FirstOrDefault(s => s.Id == id);
        }

        public IEnumerable<Session> GetAll()
        {
            return _dbContext.Sessions
                .Include(s => s.Users) // Include users associated with each session
                .ToList();
        }
    }
}
