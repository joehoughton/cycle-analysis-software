/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Session
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using cycle_analysis.Domain.Context;
    using cycle_analysis.Domain.Helper;
    using cycle_analysis.Domain.Session.Dto;
    using cycle_analysis.Domain.Session.Models;
    using cycle_analysis.Domain.SessionData.Dtos;
    using cycle_analysis.Domain.SessionData.Models;
    using StringReader = System.IO.StringReader;

    public class SessionRepository : ISessionRepository
    {
        private readonly CycleAnalysisContext _context;

        public SessionRepository(CycleAnalysisContext context)
        {
            _context = context;
        }

        public void Add(HRMFileDto hrmFileDto)
        {
            var sections = new Sections();

            var splitSections = hrmFileDto.HRMFile.Split(new[] { "[Note]", "[ExtraData]", "[HRData]" }, StringSplitOptions.None);

            var sectionParams = splitSections[0];
            var sectionNote = splitSections[1];
            var sectionExtraData = splitSections[2];
            var sectionHRData = splitSections[3];

            // add header data
            using (var reader = new StringReader(sectionParams))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    if (line.Contains("Version")) { sections.Params.SoftwareVersion = Math.Round(Double.Parse(line.Split('=')[1]), 2, MidpointRounding.AwayFromZero); }
                    if (line.Contains("Monitor")) { sections.Params.MonitorVersion = int.Parse(line.Split('=')[1]); }
                    if (line.Contains("SMode")) { sections.Params.SMode = int.Parse(line.Split('=')[1]); }
                    if (line.Contains("StartTime")) { sections.Params.StartTime = line.Split('=')[1].FormatTime(); }
                    if (line.Contains("Length")) { sections.Params.Length = line.Split('=')[1].FormatTime(); }
                    if (line.Contains("Date")) { sections.Params.Date = line.Split('=')[1].FormatDate(); }
                    if (line.Contains("Interval")) { sections.Params.Interval = int.Parse(line.Split('=')[1]); }
                    if (line.Contains("Upper1")) { sections.Params.Upper1 = int.Parse(line.Split('=')[1]); }
                    if (line.Contains("Lower1")) { sections.Params.Lower1 = int.Parse(line.Split('=')[1]); }
                }
            }

            // create timespan from StartTime
            var startTime = new TimeSpan(0, sections.Params.StartTime.Hour, sections.Params.StartTime.Minute, sections.Params.StartTime.Second, sections.Params.StartTime.Millisecond);
            // add timespan to Date
            sections.Params.Date = sections.Params.Date.Date + startTime;

            var session = new Session()
            {
                SoftwareVersion = sections.Params.SoftwareVersion,
                MonitorVersion = sections.Params.MonitorVersion,
                SMode = sections.Params.SMode,
                StartTime = sections.Params.StartTime,
                Length = sections.Params.Length,
                Date = sections.Params.Date,
                Interval = sections.Params.Interval,
                Upper1 = sections.Params.Upper1,
                Lower1 = sections.Params.Lower1,
                AthleteId = hrmFileDto.AthleteId
            };

            _context.Sessions.Add(session);

            // add body data
            var sessionDataList = new List<SessionData>();

            using (var reader = new StringReader(sectionHRData))
            {
                string line;
                var rowCount = 1;

                while ((line = reader.ReadLine()) != null)
                {
                    if (line == ""){ continue; } // line is empty on first read - continue and don't attempt to parse

                    string[] splitData = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries); // split line by whitespace and tabs

                    var sessionData = new SessionData
                    {
                        Row = rowCount,
                        HeartRate = int.Parse(splitData[0]),
                        Speed = int.Parse(splitData[1]),
                        Cadence = int.Parse(splitData[2]),
                        Altitude = int.Parse(splitData[3]),
                        Power = int.Parse(splitData[4]),
                        SessionId = session.Id
                    };

                    sessionDataList.Add(sessionData); // add new session to list
                    rowCount++;
                }
            }

            sessionDataList.ForEach(s => _context.SessionData.Add(s)); // loop through each list item and save
            _context.SaveChanges();
        }

        public List<SessionDto> GetSessionHistory(int athleteId)
        {
            var sessions = _context.Sessions.Where(s => s.AthleteId == athleteId)
            .OrderByDescending(s => s.Date)
            .Select(s => new SessionDto
            {
                Id = s.Id,
                SoftwareVersion = s.SoftwareVersion,
                MonitorVersion = s.MonitorVersion,
                SMode = s.SMode,
                StartTime = s.StartTime,
                Length = s.Length,
                Date = s.Date,
                Interval = s.Interval,
                Upper1 = s.Upper1,
                Lower1 = s.Lower1,
                AthleteId = s.AthleteId
            });

            return sessions.ToList();
        }

        public SessionDto GetSingle(int sessionId)
        {
            var session = _context.Sessions.Single(x => x.Id == sessionId);
            var sessionData = _context.SessionData.Where(x => x.SessionId == sessionId).ToList().OrderBy(x =>x.Row)
                .Select(sd => new SessionDataDto()
                {
                    Id = sd.Id,
                    HeartRate = sd.HeartRate,
                    Speed = sd.Speed,
                    Cadence = sd.Cadence,
                    Altitude = sd.Altitude,
                    Power = sd.Power,
                    SessionId = sd.SessionId,
                    Date = DateFormat.CalculateSessionDataRowDate(session.Date, session.Interval, sd.Row),
                    Time = DateFormat.CalculateSessionDataRowDate(session.Date, session.Interval, sd.Row)
                }).ToList();

            var sessionDto = new SessionDto
            {
                Id = session.Id,
                SoftwareVersion = session.SoftwareVersion,
                MonitorVersion = session.MonitorVersion,
                SMode = session.SMode,
                StartTime = session.StartTime,
                Length = session.Length,
                Date = session.Date,
                Interval = session.Interval,
                Upper1 = session.Upper1,
                Lower1 = session.Lower1,
                AthleteId = session.AthleteId,
                SessionData = sessionData
            };

            return sessionDto;
        }

        public SessionSummaryDto GetSummary(int sessionId)
        {
            var sessionData = _context.SessionData.Where(s => s.SessionId == sessionId).ToList();
            var session = _context.Sessions.Single(s => s.Id == sessionId);

            var totalCount = sessionData.Count();

            // calculate speed - divided speed by 10 as speed is *10 in file
            var totalSpeed = sessionData.Sum(s => s.Speed);
            var averageSpeed =  Math.Round((totalSpeed / 10) / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumSpeed =  Math.Round(sessionData.MaxBy(s => s.Speed).Speed / 10, 2, MidpointRounding.AwayFromZero);

            // calculate distance
            var totalTimeInHours = session.Length.TimeOfDay.TotalSeconds / 3600;
            var totalDistanceKilometres =  Math.Round(averageSpeed * totalTimeInHours, 2, MidpointRounding.AwayFromZero);
            var totalDistanceMiles = totalDistanceKilometres.ConvertToMiles();

            // calculate heart rate
            var totalHeartRate = sessionData.Sum(s => s.HeartRate);
            var averageHeartRate =  Math.Round(totalHeartRate / totalCount, 2, MidpointRounding.AwayFromZero);
            var minimumHeartRate = sessionData.MinBy(s => s.HeartRate).HeartRate;
            var maximumHeartRate = sessionData.MaxBy(s => s.HeartRate).HeartRate;

            // calculate power
            var totalPower = sessionData.Sum(s => s.Power);
            var averagePower =  Math.Round(totalPower / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumPower =  Math.Round(sessionData.MaxBy(s => s.Power).Power, 2, MidpointRounding.AwayFromZero);

            // calculate altitiude
            var totalAltitude = sessionData.Sum(s => s.Altitude);
            var averageAltitude = Math.Round(totalAltitude / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumAltitude = Math.Round(sessionData.MaxBy(s => s.Altitude).Altitude, 2, MidpointRounding.AwayFromZero);

            var sessionSummaryDto = new SessionSummaryDto()
            {
                AverageAltitude = averageAltitude,
                AverageHeartRate = averageHeartRate,
                AveragePower = averagePower,
                MaximumPower = maximumPower,
                MaximumAltitude = maximumAltitude,
                MaximumHeartRate = maximumHeartRate,
                AverageSpeed = averageSpeed,
                MaximumSpeed = maximumSpeed,
                MinimumHeartRate = minimumHeartRate,
                TotalDistanceKilometres = totalDistanceKilometres,
                TotalDistanceMiles = totalDistanceMiles,
                Date = session.Date,
                SessionId = sessionId
            };

            return sessionSummaryDto;
        }

        public SessionDataGraphDto GetSessionData(int sessionId)
        {
            var session = _context.Sessions.Single(x => x.Id == sessionId);

            var maximumHeartRate = session.SessionData.MaxBy(s => s.HeartRate).HeartRate;
            var maximumPower = session.SessionData.MaxBy(s => s.Power).Power;
            var maximumSpeed = session.SessionData.MaxBy(s => s.Speed).Speed;
            var maximumCadence = session.SessionData.MaxBy(s => s.Cadence).Cadence;
            var maximumAltitude = session.SessionData.MaxBy(s => s.Altitude).Altitude;
            var maxiumList = new [] { maximumHeartRate, maximumPower, maximumSpeed, maximumCadence, maximumAltitude };
            var highestMaximumValue =  maxiumList.OrderByDescending(x => x).ToArray()[0];

            var yAxisScale = highestMaximumValue; // amount of y axis scales - maximum value recorded
            var xAxisScale = session.SessionData.Count; // amount of x axis scales - total number of rows to plot
            var interval = session.Interval; // amount of time record row represents

            var sessionData = _context.SessionData.Where(sd => sd.SessionId == sessionId).OrderBy(sd => sd.Row).ToList();
            var heartRates = sessionData.Select(sd => new HeartRates(){ HeartRate = sd.HeartRate }).ToList(); 
            var speeds = sessionData.Select(sd => new Speeds(){ Speed = sd.Speed }).ToList();
            var altitudes = sessionData.Select(sd => new Altitudes(){ Altitude = sd.Altitude }).ToList();
            var cadences = sessionData.Select(sd => new Cadences(){ Cadence = sd.Cadence }).ToList();
            var power = sessionData.Select(sd => new Powers(){ Power = sd.Power }).ToList();

            var sessionDataGraphDto = new SessionDataGraphDto()
            {
                Interval = interval,
                HeartRates = heartRates,
                Speeds = speeds,
                Altitudes = altitudes,
                Cadences = cadences,
                Powers = power,
                XAxisScale = xAxisScale, 
                YAxisScale = yAxisScale
            };

            return sessionDataGraphDto;
        }

        public SessionSummaryDto GetSessionDataSubset(SessionDataSubsetDto sessionDataSubsetDto)
        {
            var minimumSeconds = sessionDataSubsetDto.MinimumSecond;
            var maximumSeconds = sessionDataSubsetDto.MaximumSecond;

            var session = _context.Sessions.Single(x => x.Id == sessionDataSubsetDto.SessionId);
            var sessionData = _context.SessionData.Where(x => x.SessionId == sessionDataSubsetDto.SessionId).ToList().OrderBy(x => x.Row)
                .Select(sd => new SessionDataDto()
                {
                    Id = sd.Id,
                    HeartRate = sd.HeartRate,
                    Speed = sd.Speed,
                    Cadence = sd.Cadence,
                    Altitude = sd.Altitude,
                    Power = sd.Power,
                    SessionId = sd.SessionId,
                    Date = DateFormat.CalculateSessionDataRowDate(session.Date, session.Interval, sd.Row),
                    Time = DateFormat.CalculateSessionDataRowDate(session.Date, session.Interval, sd.Row)
                }).ToList();

            var filteredSessionData = sessionData.FilterListDates(session.Date, minimumSeconds, maximumSeconds); // filter sessions by min and max seconds

            var totalCount = filteredSessionData.Count();

            // calculate speed - divided speed by 10 as speed is *10 in file
            var totalSpeed = filteredSessionData.Sum(s => s.Speed);
            var averageSpeed =  Math.Round((totalSpeed / 10) / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumSpeed =  Math.Round(filteredSessionData.MaxBy(s => s.Speed).Speed / 10, 2, MidpointRounding.AwayFromZero);

            // calculate distance
            var totalTimeInHours = session.Length.TimeOfDay.TotalSeconds / 3600;
            var totalDistanceKilometres =  Math.Round(averageSpeed * totalTimeInHours, 2, MidpointRounding.AwayFromZero);
            var totalDistanceMiles = totalDistanceKilometres.ConvertToMiles();

            // calculate heart rate
            var totalHeartRate = filteredSessionData.Sum(s => s.HeartRate);
            var averageHeartRate =  Math.Round(totalHeartRate / totalCount, 2, MidpointRounding.AwayFromZero);
            var minimumHeartRate = filteredSessionData.MinBy(s => s.HeartRate).HeartRate;
            var maximumHeartRate = filteredSessionData.MaxBy(s => s.HeartRate).HeartRate;

            // calculate power
            var totalPower = filteredSessionData.Sum(s => s.Power);
            var averagePower =  Math.Round(totalPower / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumPower =  Math.Round(filteredSessionData.MaxBy(s => s.Power).Power, 2, MidpointRounding.AwayFromZero);

            // calculate altitiude
            var totalAltitude = filteredSessionData.Sum(s => s.Altitude);
            var averageAltitude = Math.Round(totalAltitude / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumAltitude = Math.Round(filteredSessionData.MaxBy(s => s.Altitude).Altitude, 2, MidpointRounding.AwayFromZero);

            var sessionSummaryDto = new SessionSummaryDto()
            {
                AverageAltitude = averageAltitude,
                AverageHeartRate = averageHeartRate,
                AveragePower = averagePower,
                MaximumPower = maximumPower,
                MaximumAltitude = maximumAltitude,
                MaximumHeartRate = maximumHeartRate,
                AverageSpeed = averageSpeed,
                MaximumSpeed = maximumSpeed,
                MinimumHeartRate = minimumHeartRate,
                TotalDistanceKilometres = totalDistanceKilometres,
                TotalDistanceMiles = totalDistanceMiles,
                Date = session.Date,
                SessionId = session.Id
            };

            return sessionSummaryDto;
        }
    }
}