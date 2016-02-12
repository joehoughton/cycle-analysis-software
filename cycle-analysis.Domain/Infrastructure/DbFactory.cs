/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Infrastructure
{
    using cycle_analysis.Domain.Context;

    public class DbFactory : Disposable, IDbFactory
    {
        private readonly CycleAnalysisContext _context;
        public DbFactory(CycleAnalysisContext context)
        {
            _context = context;
        }

        public CycleAnalysisContext Init()
        {
        return _context;
        }

        protected override void DisposeCore()
        {
            if (_context != null){
                _context.Dispose();
            }
        }
    }
}
