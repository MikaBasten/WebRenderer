using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IServices
{
    public interface ISessionService
    {
        Session CreateSession(string fileName, string loggedInUsername);
        void AddUserToSession(int sessionId, string username);
        void RemoveUserFromSession(int sessionId, string username);
        IEnumerable<Session> GetAllSessions();
    }
}
