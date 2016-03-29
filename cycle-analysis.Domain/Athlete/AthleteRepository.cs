/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Athlete
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using cycle_analysis.Domain.Athlete.Dto;
    using cycle_analysis.Domain.Athlete.Models;
    using cycle_analysis.Domain.Context;

    public class AthleteRepository : IAthleteRepository
    {
        private readonly CycleAnalysisContext _context;

        public AthleteRepository(CycleAnalysisContext context)
        {
            _context = context;
        }

        public AthleteDto Add(AthleteDto athleteDto)
        {
            var athlete = new Athlete()
            {
                Username = athleteDto.Username,
                FirstName = athleteDto.FirstName,
                LastName = athleteDto.LastName,
                Email = athleteDto.Email,
                RegistrationDate = DateTime.Now,
                Image = athleteDto.Image ?? "unknown.jpg",
                LactateThreshold = Math.Round(athleteDto.LactateThreshold, 2, MidpointRounding.AwayFromZero),
                FunctionalThresholdPower = Math.Round(athleteDto.FunctionalThresholdPower, 2, MidpointRounding.AwayFromZero),
                Weight = Math.Round(athleteDto.Weight, 2, MidpointRounding.AwayFromZero),
                UniqueKey = Guid.NewGuid()
            };

            _context.Athletes.Add(athlete);

            _context.SaveChanges();
    
            var newAthleteDto = new AthleteDto()
            {
                Id = athlete.Id,
                FirstName = athleteDto.FirstName,
                LastName = athleteDto.LastName,
                Email = athleteDto.Email,
                RegistrationDate = DateTime.Now,
                Image = athleteDto.Image ?? "unknown.jpg",
                LactateThreshold = athleteDto.LactateThreshold,
                FunctionalThresholdPower = athleteDto.FunctionalThresholdPower,
                Weight = athleteDto.Weight,
                UniqueKey = Guid.NewGuid()
            };

            return newAthleteDto;
        }

        public AthleteDto GetSingle(int athleteId)
        {
            var athlete = _context.Athletes.Single(x => x.Id == athleteId);

            var athleteDto = new AthleteDto()
            {
                Id = athlete.Id,
                FirstName = athlete.FirstName,
                LastName = athlete.LastName,
                Email = athlete.Email,
                RegistrationDate = DateTime.Now,
                Image = athlete.Image ?? "unknown.jpg",
                LactateThreshold = athlete.LactateThreshold,
                FunctionalThresholdPower = athlete.FunctionalThresholdPower,
                Weight = athlete.Weight,
                UniqueKey = athlete.UniqueKey
            };

            return athleteDto;
        }

        public AthleteDto Edit(AthleteDto athleteDto)
        {
            var athlete = _context.Athletes.Single(x => x.Id == athleteDto.Id);

            athlete.FirstName = athleteDto.FirstName;
            athlete.LastName = athleteDto.LastName;
            athlete.Image = athleteDto.Image ?? "unknown.jpg";
            athlete.LactateThreshold = Math.Round(athleteDto.LactateThreshold, 2, MidpointRounding.AwayFromZero);
            athlete.FunctionalThresholdPower = Math.Round(athleteDto.FunctionalThresholdPower, 2, MidpointRounding.AwayFromZero);
            athlete.Weight = Math.Round(athleteDto.Weight, 2, MidpointRounding.AwayFromZero);
            athlete.Email = athleteDto.Email;

            _context.SaveChanges();

            var newAthleteDto = new AthleteDto()
            {
                Id = athlete.Id,
                FirstName = athlete.FirstName,
                LastName = athlete.LastName,
                Email = athlete.Email,
                RegistrationDate = DateTime.Now,
                Image = athlete.Image ?? "unknown.jpg",
                LactateThreshold = athlete.LactateThreshold,
                FunctionalThresholdPower = athlete.LactateThreshold,
                Weight = athlete.Weight,
                UniqueKey = athlete.UniqueKey
            };

            return newAthleteDto; 
        }

        public List<AthleteDto> GetAll(int currentPage, int currentPageSize)
        {
            var athletes = _context.Athletes
                        .OrderBy(a => a.FirstName)
                        .ThenBy(a => a.LastName)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .Select(a => new AthleteDto()
                        {
                            Id = a.Id,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            Email = a.Email,
                            RegistrationDate = DateTime.Now,
                            Image = a.Image ?? "unknown.jpg",
                            LactateThreshold = a.LactateThreshold,
                            FunctionalThresholdPower = a.FunctionalThresholdPower,
                            Weight = a.Weight,
                            UniqueKey = a.UniqueKey
                        })
                        .ToList();

            return athletes;
        }

        public List<AthleteDto> GetAllByFilter(string filter, int currentPage, int currentPageSize)
        {
            var athletes = _context.Athletes.Where(a => a.Email.ToLower().Contains(filter.ToLower().Trim()) 
                        || a.FirstName.ToLower().Contains(filter.ToLower().Trim()) 
                        || a.LastName.ToLower().Contains(filter.ToLower().Trim())
                        || a.Username.ToLower().Contains(filter.ToLower().Trim()))
                        .OrderBy(b => b.FirstName)
                        .ThenBy(b => b.LastName)
                        .Skip(currentPage * currentPageSize)
                        .Take(currentPageSize)
                        .Select(a => new AthleteDto()
                        {
                            Id = a.Id,
                            FirstName = a.FirstName,
                            LastName = a.LastName,
                            Email = a.Email,
                            RegistrationDate = DateTime.Now,
                            Image = a.Image ?? "unknown.jpg",
                            LactateThreshold = a.LactateThreshold,
                            FunctionalThresholdPower = a.FunctionalThresholdPower,
                            Weight = a.Weight,
                            UniqueKey = a.UniqueKey
                        })
                        .ToList();
            return athletes;
        }

        public int Count()
        {
            var athleteCount = _context.Athletes.Count();
            return athleteCount;
        }

        public int Count(string filter)
        {
            var athleteCount = _context.Athletes
                           .Count(a => a.Email.ToLower().Contains(filter.ToLower().Trim()) 
                            || a.FirstName.ToLower().Contains(filter.ToLower().Trim()) 
                            || a.LastName.ToLower().Contains(filter.ToLower().Trim())
                            || a.Username.ToLower().Contains(filter.ToLower().Trim()));
            return athleteCount;
        }

    }
}
