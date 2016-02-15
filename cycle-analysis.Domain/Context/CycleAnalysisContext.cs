/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Context
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using cycle_analysis.Domain.Athlete.Mappings;
    using cycle_analysis.Domain.Athlete.Models;
    using cycle_analysis.Domain.Error.Mapping;
    using cycle_analysis.Domain.Error.Models;
    using cycle_analysis.Domain.Role.Mapping;
    using cycle_analysis.Domain.Role.Models;
    using cycle_analysis.Domain.User.Mapping;
    using cycle_analysis.Domain.User.Models;

    public class CycleAnalysisContext : DbContext
    {
        static CycleAnalysisContext()
        {
            Database.SetInitializer<CycleAnalysisContext>(null);
        }

        public CycleAnalysisContext(string connectionString)
            : base(connectionString)
        {
            Configuration.LazyLoadingEnabled = true;
        }

        public virtual void Commit()
        {
            this.SaveChanges();
        }

        public DbSet<Error> Errors { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Athlete> Athletes { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            modelBuilder.Configurations.Add(new ErrorMap());
            modelBuilder.Configurations.Add(new RoleMap());
            modelBuilder.Configurations.Add(new UserMap());
            modelBuilder.Configurations.Add(new UserRoleMap());
            modelBuilder.Configurations.Add(new AthleteMap());
        }
    }
}
