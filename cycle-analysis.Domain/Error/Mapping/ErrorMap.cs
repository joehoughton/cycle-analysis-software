/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Error.Mapping
{
    using System.Data.Entity.ModelConfiguration;
    using cycle_analysis.Domain.Error.Models;

    public class ErrorMap : EntityTypeConfiguration<Error>
    {
        public ErrorMap()
        {
            // Primary Key
            HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            ToTable("Error");
            Property(t => t.Id).HasColumnName("Id");
            Property(t => t.Message).HasColumnName("Message");
            Property(t => t.StackTrace).HasColumnName("StackTrace");
            Property(t => t.DateCreated).HasColumnName("DateCreated");
        }
    }
}
