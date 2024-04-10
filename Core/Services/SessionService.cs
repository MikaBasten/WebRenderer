using Core.IRepository;
using Core.IServices;
using Core.Models;
using System;

namespace Core.Services
{
    public class SessionService : ISessionService
    {
        private readonly ISessionRepository _sessionRepository;
        private readonly IUserRepository _userRepository;

        public SessionService(ISessionRepository sessionRepository, IUserRepository userRepository)
        {
            _sessionRepository = sessionRepository;
            _userRepository = userRepository;
        }

        public Session CreateSession(string fileName, string loggedInUsername)
        {
            var loggedInUser = _userRepository.GetUserByUsername(loggedInUsername);

            if (loggedInUser == null)
            {
                throw new ArgumentException("Logged-in user not found.");
            }

            var session = new Session
            {
                FileName = fileName,
                Users = new List<User> { loggedInUser },
            };

            _sessionRepository.Add(session);

            return session;
        }

        public void AddUserToSession(int sessionId, string username)
        {
            var session = _sessionRepository.GetById(sessionId);

            if (session == null)
            {
                throw new ArgumentException("Session not found.");
            }

            var user = _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            // Add the user to the session's list of users
            session.Users.Add(user);

            // Update the session in the repository
            _sessionRepository.Update(session);
        }

        public void RemoveUserFromSession(int sessionId, string username)
        {
            var session = _sessionRepository.GetById(sessionId);

            if (session == null)
            {
                throw new ArgumentException("Session not found.");
            }

            var user = _userRepository.GetUserByUsername(username);

            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            // Remove the user from the session's list of users
            session.Users.Remove(user);

            // Update the session in the repository
            _sessionRepository.Update(session);
        }

        public IEnumerable<Session> GetAllSessions()
        {
            return _sessionRepository.GetAll();
        }
    }

}
