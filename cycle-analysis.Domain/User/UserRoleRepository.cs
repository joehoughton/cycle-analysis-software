/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.User
{
    using cycle_analysis.Domain.Context;
    using cycle_analysis.Domain.User.Models;

    public class UserRoleRepository : IUserRoleRepository
    {
         private readonly CycleAnalysisContext _context;

         public UserRoleRepository(CycleAnalysisContext context)
        {
            _context = context;
        }

        public void Add(UserRole userRole)
        {
            _context.UserRoles.Add(userRole);
        }
    }
}
