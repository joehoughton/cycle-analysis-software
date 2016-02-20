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
                    if (line.Contains("Date"))
                    {
                        sections.Params.Date = line.Split('=')[1].FormatDate();
                      
                    }
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
                    
                    string[] splitData = line.Split(null); // split line by whitespace

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
    }
}