using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface ISessionRepository
    {
        void Add(Session session);
        void Update(Session session);
        void Delete(Session session);
        Session GetById(int id);
        IEnumerable<Session> GetAll();
    }
}
