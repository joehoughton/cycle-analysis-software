/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Athlete.Dto
{
    using System;

    public class AthleteDto
    {
        public AthleteDto() { }
        public AthleteDto(int id, string username, string firstName, string lastName, string email, DateTime registrationDate, string image, double lactateThreshold, double weight, Guid uniqueKey)
        {
            Id = id;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            RegistrationDate = registrationDate;
            Image = image;
            LactateThreshold = lactateThreshold;
            Weight = weight;
            UniqueKey = uniqueKey;
        }

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
