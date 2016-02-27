/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Session.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using cycle_analysis.Domain.Session.Models;

    public class SessionMap : EntityTypeConfiguration<Session>
    {
        public SessionMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("Session");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Title).HasColumnName("Title");
            this.Property(t => t.SoftwareVersion).HasColumnName("SoftwareVersion");
            this.Property(t => t.MonitorVersion).HasColumnName("MonitorVersion");
            this.Property(t => t.SMode).HasColumnName("SMode");
            this.Property(t => t.StartTime).HasColumnName("StartTime");
            this.Property(t => t.Length).HasColumnName("Length");
            this.Property(t => t.Date).HasColumnName("Date");
            this.Property(t => t.Interval).HasColumnName("Interval");
            this.Property(t => t.Upper1).HasColumnName("Upper1");
            this.Property(t => t.Lower1).HasColumnName("Lower1");
            this.Property(t => t.AthleteId).HasColumnName("AthleteId");

            // Relationships
            this.HasRequired(t => t.Athlete)
                .WithMany(t => t.Sessions)
                .HasForeignKey(d => d.AthleteId);
        }
    }
}


