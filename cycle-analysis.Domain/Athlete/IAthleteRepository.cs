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
    using System.Collections.Generic;
    using cycle_analysis.Domain.Athlete.Dto;

    public interface IAthleteRepository
    {
        AthleteDto Add(AthleteDto athleteDto);
        AthleteDto GetSingle(int athleteId);
        AthleteDto Edit(AthleteDto athleteDto);
        List<AthleteDto> GetAll(int currentPage, int currentPageSize);
        List<AthleteDto> GetAllByFilter(string filter, int currentPage, int currentPageSize);
        int Count();
        int Count(string filter);
    }
}