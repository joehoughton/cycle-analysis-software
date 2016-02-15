namespace cycle_analysis.Domain.Athlete.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using cycle_analysis.Domain.Athlete.Models;

    public class AthleteMap : EntityTypeConfiguration<Athlete>
    {
        public AthleteMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Username)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.LastName)
                .IsRequired()
                .HasMaxLength(100);

            this.Property(t => t.Email)
                .IsRequired()
                .HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("Athlete");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Username).HasColumnName("Username");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.RegistrationDate).HasColumnName("RegistrationDate");
            this.Property(t => t.Image).HasColumnName("Image");
            this.Property(t => t.LactateThreshold).HasColumnName("LactateThreshold");
            this.Property(t => t.Weight).HasColumnName("Weight");
            this.Property(t => t.UniqueKey).HasColumnName("UniqueKey");
        }
    }
}
