﻿using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Spoofer.Models
{
    public partial class User
    {
        public User()
        {
            Coordinates = new HashSet<Coordinates>();
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool? IsAuthenticated { get; set; }

        public virtual ICollection<Coordinates> Coordinates { get; set; }
    }
}
