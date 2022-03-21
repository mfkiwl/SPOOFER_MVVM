﻿using Spoofer.Models;
using Spoofer.ViewModels;
using Spoofer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spoofer.Services.User
{
    public class ServiceRegister : IRegister
    {
        private readonly CoordinatesContext _context;
        public ServiceRegister(CoordinatesContext context)
        {
            _context = context;
        }
        private bool IsRegistered(string userName)
        {
            foreach (var user in _context.User)
            {
                return user.UserName == userName;
            }
            return false;
        }

        public void OnRegister(AccountViewModel model)
        {
            if (!IsRegistered(model.UserName))
            {
                var user = new Spoofer.Models.User()
                {
                    UserId = Guid.NewGuid().ToString(),
                    UserName = model.UserName,
                    Password = model.Password
                };
                _context.User.Add(user);
                _context.SaveChanges();
            }
        }

    }
}
