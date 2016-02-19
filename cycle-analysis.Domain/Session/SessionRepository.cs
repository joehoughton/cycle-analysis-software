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
    using cycle_analysis.Domain.Context;
    using cycle_analysis.Domain.Helper;
    using cycle_analysis.Domain.Session.Dto;
    using cycle_analysis.Domain.Session.Models;

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
                    if (line.Contains("Date")){ sections.Params.Date = line.Split('=')[1].FormatDate(); }
                    if (line.Contains("Interval")) { sections.Params.Interval = int.Parse(line.Split('=')[1]); }
                    if (line.Contains("Upper1")) { sections.Params.Upper1 = int.Parse(line.Split('=')[1]); }
                    if (line.Contains("Lower1")) { sections.Params.Lower1 = int.Parse(line.Split('=')[1]); }
                }
            }

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
            _context.SaveChanges();
        }

    }
}