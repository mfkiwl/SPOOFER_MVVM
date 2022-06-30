using Spoofer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User = Spoofer.Models.User;

namespace Spoofer.Services.User
{
    public static class RoleAdministration
    {
        public static bool IsInRole(params string[] role)
        {
            using (var _context = new CoordinatesContext())
            {
                var userToCheck = _context.User.SingleOrDefault(p => p.IsAuthenticated == true);
                foreach (var item in role)
                {
                    if (userToCheck.Permission == item)
                        return true;
                    else
                        return false;
                }
                return false;
            }

        }
    }
}
