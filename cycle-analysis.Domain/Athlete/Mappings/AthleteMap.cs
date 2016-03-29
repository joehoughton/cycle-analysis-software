/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Athlete.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using cycle_analysis.Domain.Athlete.Models;

    public class AthleteMap : EntityTypeConfiguration<Athlete>
    {
        public AthleteMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(100);

            Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            ToTable("Athlete");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Username).HasColumnName("Username");
            Property(t => t.FirstName).HasColumnName("FirstName");
            Property(t => t.LastName).HasColumnName("LastName");
            Property(t => t.Email).HasColumnName("Email");
            Property(t => t.RegistrationDate).HasColumnName("RegistrationDate");
            Property(t => t.Image).HasColumnName("Image");
            Property(t => t.LactateThreshold).HasColumnName("LactateThreshold");
            Property(t => t.FunctionalThresholdPower).HasColumnName("FunctionalThresholdPower");
            Property(t => t.Weight).HasColumnName("Weight");
            Property(t => t.UniqueKey).HasColumnName("UniqueKey");
        }
    }
}
