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
    using System.Linq;
    using cycle_analysis.Domain.Context;
    using cycle_analysis.Domain.User.Models;

    public class UserRepository : IUserRepository
    {
        private readonly CycleAnalysisContext _context;

        public UserRepository(CycleAnalysisContext context)
        {
            _context = context;
        }

        public User Get(string username)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username);
        }

        public void Add(User user)
        {
            _context.Users.Add(user);
        }

        public User GetSingle(int userId)
        {
            return _context.Users.FirstOrDefault(x => x.Id == userId);
        }
    }
}
