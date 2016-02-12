/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.User.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using cycle_analysis.Domain.User.Models;

    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.HashedPassword)
                .IsRequired()
                .HasMaxLength(200);

            this.Property(t => t.Salt)
                .IsRequired()
                .HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("User");
            this.Property(t => t.Id).HasColumnName("ID");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.HashedPassword).HasColumnName("HashedPassword");
            this.Property(t => t.Salt).HasColumnName("Salt");
            this.Property(t => t.IsLocked).HasColumnName("IsLocked");
            this.Property(t => t.DateCreated).HasColumnName("DateCreated");
        }
    }
}
