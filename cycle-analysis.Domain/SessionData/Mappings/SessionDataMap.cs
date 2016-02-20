namespace cycle_analysis.Domain.SessionData.Mappings
{
    using System.Data.Entity.ModelConfiguration;
    using cycle_analysis.Domain.SessionData.Models;

    public class SessionDataMap : EntityTypeConfiguration<SessionData>
    {
        public SessionDataMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            // Table & Column Mappings
            this.ToTable("SessionData");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Row).HasColumnName("Row");
            this.Property(t => t.HeartRate).HasColumnName("HeartRate");
            this.Property(t => t.Speed).HasColumnName("Speed");
            this.Property(t => t.Cadence).HasColumnName("Cadence");
            this.Property(t => t.Altitude).HasColumnName("Altitude");
            this.Property(t => t.Power).HasColumnName("Power");
            this.Property(t => t.SessionId).HasColumnName("SessionId");

            // Relationships
            this.HasRequired(t => t.Session)
                .WithMany(t => t.SessionData)
                .HasForeignKey(d => d.SessionId);
        }
    }
}
