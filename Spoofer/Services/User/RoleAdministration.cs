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
        /// <summary>
        /// Check if the Connected User has the permission to make the action
        /// </summary>
        /// <param name="role"></param>
        /// <returns>True if user has the permission and false if he has not.</returns>
        public static bool IsInRole(params string[] role)
        {
            using (var _context = new CoordinatesContext())
            {
                var userToCheck = _context.User.FirstOrDefault(p => p.IsAuthenticated == true);
                return role.Any(r => r == userToCheck.Permission);
            }

        }
    }
}
