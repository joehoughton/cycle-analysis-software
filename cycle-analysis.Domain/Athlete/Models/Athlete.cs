namespace cycle_analysis.Domain.Athlete.Models
{
    using System;

    public class Athlete
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string Image { get; set; }
        public double? LactateThreshold { get; set; }
        public double? Weight { get; set; }
        public Guid UniqueKey { get; set; }
    }
}
