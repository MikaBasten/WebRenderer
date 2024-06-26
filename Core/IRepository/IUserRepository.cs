﻿using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.IRepository
{
    public interface IUserRepository
    {
        bool IsValidUser(string username, string password);
        User GetUserByUsername(string username);
        void Add(User user);
    }
}
