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

            // get unit from SMode - 8th digit
            var unit = int.Parse(sections.Params.SMode.ToString().ToCharArray()[7].ToString());

            var session = new Session()
            {
                Title = hrmFileDto.Title,
                Unit = unit,
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
                var rowCount = 0;

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
                Title = s.Title,
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
                    Row = sd.Row,
                    Date = DateFormat.CalculateSessionDataRowDate(session.Date, session.Interval, sd.Row)
                }).ToList();

            var sessionDto = new SessionDto
            {
                Id = session.Id,
                Title = session.Title,
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

        public SessionSummaryDto GetSummary(SessionSummaryRequestDto sessionSummaryRequestDto)
        {
            var requestedUnitIsMetric = sessionSummaryRequestDto.Unit == 0;

            var sessionData = _context.SessionData.Where(s => s.SessionId == sessionSummaryRequestDto.SessionId).ToList();
            var session = _context.Sessions.Single(s => s.Id == sessionSummaryRequestDto.SessionId);
            var sModeIsMetric = session.SMode.ToString("D9").IsMetric(); // pad int to 9 decimals if zero
            var totalCount = sessionData.Count();

            double maximumSpeed;
            double averageSpeed;
            double totalDistance;
            double averageAltitude;
            double maximumAltitude;

            if (requestedUnitIsMetric) // return metric values
            {
                if (sModeIsMetric)
                {
                    // calculate speed - divided speed by 10 as speed is *10 in file
                    var totalSpeed = sessionData.Sum(s => s.Speed);
                    averageSpeed = Math.Round((totalSpeed / 10) / totalCount, 2, MidpointRounding.AwayFromZero);
                    maximumSpeed = Math.Round(sessionData.MaxBy(s => s.Speed).Speed / 10, 2, MidpointRounding.AwayFromZero);

                    // calculate distance
                    var totalTimeInHours = session.Length.TimeOfDay.TotalSeconds / 3600;
                    totalDistance = Math.Round(averageSpeed * totalTimeInHours, 2, MidpointRounding.AwayFromZero);

                    // calculate altitiude
                    var totalAltitude = sessionData.Sum(s => s.Altitude / 10);
                    averageAltitude = Math.Round(totalAltitude / totalCount, 2, MidpointRounding.AwayFromZero);
                    maximumAltitude = Math.Round(sessionData.MaxBy(s => s.Altitude).Altitude / 10, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    // calculate speed - divided speed by 10 as speed is *10 in file - converted kilometres to miles
                    var totalSpeed = sessionData.Sum(s => s.Speed);
                    averageSpeed = ((totalSpeed / 10) / totalCount).ConvertToKilometres();
                    maximumSpeed = (sessionData.MaxBy(s => s.Speed).Speed / 10).ConvertToKilometres();

                    // calculate distance
                    var totalTimeInHours = session.Length.TimeOfDay.TotalSeconds / 3600;
                    totalDistance = ((totalSpeed / 10) / totalCount * totalTimeInHours).ConvertToKilometres();

                    // calculate altitiude - convert metres to feet
                    var totalAltitude = sessionData.Sum(s => s.Altitude / 10);
                    averageAltitude = (totalAltitude / totalCount).ConvertToMetres();
                    maximumAltitude = (sessionData.MaxBy(s => s.Altitude).Altitude / 10).ConvertToMetres();
                }
                
            }
            else // return imperial values
            {
                if (sModeIsMetric)
                {
                    // calculate speed - divided speed by 10 as speed is *10 in file - converted kilometres to miles
                    var totalSpeed = sessionData.Sum(s => s.Speed);
                    averageSpeed = ((totalSpeed / 10) / totalCount).ConvertToMiles();
                    maximumSpeed = (sessionData.MaxBy(s => s.Speed).Speed / 10).ConvertToMiles();

                    // calculate distance
                    var totalTimeInHours = session.Length.TimeOfDay.TotalSeconds / 3600;
                    totalDistance = ((totalSpeed / 10) / totalCount * totalTimeInHours).ConvertToMiles();

                    // calculate altitiude - convert metres to feet
                    var totalAltitude = sessionData.Sum(s => s.Altitude / 10);
                    averageAltitude = (totalAltitude / totalCount).ConvertToFeet();
                    maximumAltitude = (sessionData.MaxBy(s => s.Altitude).Altitude / 10).ConvertToFeet();
                }
                else
                {
                    // calculate speed - divided speed by 10 as speed is *10 in file
                    var totalSpeed = sessionData.Sum(s => s.Speed);
                    averageSpeed = Math.Round((totalSpeed / 10) / totalCount, 2, MidpointRounding.AwayFromZero);
                    maximumSpeed = Math.Round(
                    sessionData.MaxBy(s => s.Speed).Speed / 10, 2, MidpointRounding.AwayFromZero);

                    // calculate distance
                    var totalTimeInHours = session.Length.TimeOfDay.TotalSeconds / 3600;
                    totalDistance = Math.Round(averageSpeed * totalTimeInHours, 2, MidpointRounding.AwayFromZero);

                    // calculate altitiude
                    var totalAltitude = sessionData.Sum(s => s.Altitude / 10);
                    averageAltitude = Math.Round(totalAltitude / totalCount, 2, MidpointRounding.AwayFromZero);
                    maximumAltitude = Math.Round(sessionData.MaxBy(s => s.Altitude).Altitude / 10, 2, MidpointRounding.AwayFromZero);
                }
            }

            // calculate heart rate
            var totalHeartRate = sessionData.Sum(s => s.HeartRate);
            var averageHeartRate =  Math.Round(totalHeartRate / totalCount, 2, MidpointRounding.AwayFromZero);
            var minimumHeartRate = sessionData.MinBy(s => s.HeartRate).HeartRate;
            var maximumHeartRate = sessionData.MaxBy(s => s.HeartRate).HeartRate;

            // calculate power
            var totalPower = sessionData.Sum(s => s.Power);
            var averagePower =  Math.Round(totalPower / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumPower =  Math.Round(sessionData.MaxBy(s => s.Power).Power, 2, MidpointRounding.AwayFromZero);

            // calculate cadence
            var totalCadence = sessionData.Sum(s => s.Cadence);
            var averageCadence = Math.Round(totalCadence / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumCadence = Math.Round(sessionData.MaxBy(s => s.Cadence).Cadence, 2, MidpointRounding.AwayFromZero);

            // calculate normalized power
            var sessionDto = GetSingle(sessionSummaryRequestDto.SessionId);
            var normalizedPower = sessionDto.CalculateNormalizedPower();

            // calculate intensity factor
            var functionalThresholdPower = _context.Athletes.Single(a => a.Id == session.AthleteId).FunctionalThresholdPower;
            var intensityFactor = normalizedPower.CalculateIntensityFactor(functionalThresholdPower);

            // calculate training stress score
            var sessionTimeInSeconds = session.Length.TimeOfDay.TotalSeconds;
            double trainingStressScore = 0;
            string trainingStressScoreStatus = "";

            if (normalizedPower != 0)
            {
                trainingStressScore = Metrics.CalculateTrainingStressScore(sessionTimeInSeconds, normalizedPower, intensityFactor, functionalThresholdPower);
                trainingStressScoreStatus = Metrics.TrainingStressScoreStatus(trainingStressScore);
            }
           
            var sessionSummaryDto = new SessionSummaryDto()
            {
                Title = session.Title,
                AverageAltitude = averageAltitude,
                MaximumAltitude = maximumAltitude,
                AverageCadence = averageCadence,
                MaximumCadence = maximumCadence,
                AverageHeartRate = averageHeartRate,
                MinimumHeartRate = minimumHeartRate,
                MaximumHeartRate = maximumHeartRate,
                AverageSpeed = averageSpeed,
                MaximumSpeed = maximumSpeed,
                AveragePower = averagePower,
                MaximumPower = maximumPower,
                TotalDistance = totalDistance,
                Date = session.Date,
                NormalizedPower = normalizedPower,
                IntensityFactor = intensityFactor,
                TrainingStressScore = trainingStressScore,
                TrainingStressScoreStatus = trainingStressScoreStatus,
                SessionId = sessionSummaryRequestDto.SessionId
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
            var maxiumList = new [] {maximumHeartRate, maximumPower, maximumSpeed, maximumCadence, maximumAltitude};
            var highestMaximumValue =  maxiumList.OrderByDescending(x => x).ToArray()[0];

            var yAxisScale = highestMaximumValue; // amount of y axis scales - maximum value recorded
            var xAxisScale = session.SessionData.Count; // amount of x axis scales - total number of rows to plot
            var interval = session.Interval; // amount of time record row represents

            var sessionData = _context.SessionData.Where(sd => sd.SessionId == sessionId).OrderBy(sd => sd.Row).ToList();
            var heartRates = sessionData.Select(sd => new HeartRates(){HeartRate = sd.HeartRate}).ToList(); 
            var speeds = sessionData.Select(sd => new Speeds(){Speed = sd.Speed}).ToList();
            var altitudes = sessionData.Select(sd => new Altitudes(){Altitude = sd.Altitude}).ToList();
            var cadences = sessionData.Select(sd => new Cadences(){Cadence = sd.Cadence}).ToList();
            var power = sessionData.Select(sd => new Powers(){Power = sd.Power}).ToList();

            var sessionToDetectIntervals = GetSingle(sessionId);
            var detectedIntervals = DetectIntervals(sessionToDetectIntervals);

            var sessionDataGraphDto = new SessionDataGraphDto()
            {
                Interval = interval,
                HeartRates = heartRates,
                Speeds = speeds,
                Altitudes = altitudes,
                Cadences = cadences,
                Powers = power,
                XAxisScale = xAxisScale, 
                YAxisScale = yAxisScale,
                DetectedIntervals = detectedIntervals
            };

            return sessionDataGraphDto;
        }

        public SessionSummaryDto GetSessionDataSubset(SessionDataSubsetDto sessionDataSubsetDto)
        {
            var minimumSeconds = sessionDataSubsetDto.MinimumSecond;
            var maximumSeconds = sessionDataSubsetDto.MaximumSecond;
            var totalTimeInHours = (maximumSeconds - minimumSeconds) / 3600;
            var requestedUnitIsMetric = sessionDataSubsetDto.Unit == 0;
            
            var session = _context.Sessions.Single(x => x.Id == sessionDataSubsetDto.SessionId);
            var sModeIsMetric = session.SMode.ToString("D9").IsMetric(); // pad int to 9 decimals if zero

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
                    Date = DateFormat.CalculateSessionDataRowDate(session.Date, session.Interval, sd.Row)
                }).ToList();

            var filteredSessionData = sessionData.FilterListDates(session.Date, minimumSeconds, maximumSeconds); // filter sessions by min and max seconds
            var totalCount = filteredSessionData.Count();

            double maximumSpeed;
            double averageSpeed;
            double totalSpeed;
            double totalDistance;
            double averageAltitude;
            double maximumAltitude;
            double totalAltitude;

            if (requestedUnitIsMetric) // return metric values
            {
                if (sModeIsMetric)
                {
                    // calculate speed - divided speed by 10 as speed is *10 in file
                    totalSpeed = filteredSessionData.Sum(s => s.Speed);
                    averageSpeed = Math.Round((totalSpeed / 10) / totalCount, 2, MidpointRounding.AwayFromZero);
                    maximumSpeed = Math.Round(filteredSessionData.MaxBy(s => s.Speed).Speed / 10, 2, MidpointRounding.AwayFromZero);

                    // calculate distance
                    totalDistance = Math.Round(averageSpeed * totalTimeInHours, 2, MidpointRounding.AwayFromZero);

                    // calculate altitiude
                    totalAltitude = filteredSessionData.Sum(s => s.Altitude / 10);
                    averageAltitude = Math.Round(totalAltitude / totalCount, 2, MidpointRounding.AwayFromZero);
                    maximumAltitude = Math.Round(filteredSessionData.MaxBy(s => s.Altitude).Altitude / 10, 2, MidpointRounding.AwayFromZero);
                }
                else
                {
                    // calculate speed - divided speed by 10 as speed is *10 in file - converted kilometres to miles
                    totalSpeed = filteredSessionData.Sum(s => s.Speed);
                    averageSpeed = ((totalSpeed / 10) / totalCount).ConvertToKilometres();
                    maximumSpeed = (filteredSessionData.MaxBy(s => s.Speed).Speed / 10).ConvertToKilometres();

                    // calculate distance
                    totalDistance = ((totalSpeed / 10) / totalCount * totalTimeInHours).ConvertToKilometres();

                    // calculate altitiude - convert metres to feet
                    totalAltitude = filteredSessionData.Sum(s => s.Altitude / 10);
                    averageAltitude = (totalAltitude / totalCount).ConvertToMetres();
                    maximumAltitude = (filteredSessionData.MaxBy(s => s.Altitude).Altitude / 10).ConvertToMetres();
                }
            }
            else // return imperial values
            {
                if (sModeIsMetric)
                {
                    // calculate speed - divided speed by 10 as speed is *10 in file - converted kilometres to miles
                    totalSpeed = filteredSessionData.Sum(s => s.Speed);
                    averageSpeed = ((totalSpeed / 10) / totalCount).ConvertToMiles();
                    maximumSpeed = (filteredSessionData.MaxBy(s => s.Speed).Speed / 10).ConvertToMiles();

                    // calculate distance
                    totalDistance = ((totalSpeed / 10) / totalCount * totalTimeInHours).ConvertToMiles();

                    // calculate altitiude - convert metres to feet
                    totalAltitude = filteredSessionData.Sum(s => s.Altitude / 10);
                    averageAltitude = (totalAltitude / totalCount).ConvertToFeet();
                    maximumAltitude = (filteredSessionData.MaxBy(s => s.Altitude).Altitude / 10).ConvertToFeet();
                }
                else
                {
                    // calculate speed - divided speed by 10 as speed is *10 in file
                    totalSpeed = filteredSessionData.Sum(s => s.Speed);
                    averageSpeed = Math.Round((totalSpeed / 10) / totalCount, 2, MidpointRounding.AwayFromZero);
                    maximumSpeed = Math.Round(
                    filteredSessionData.MaxBy(s => s.Speed).Speed / 10, 2, MidpointRounding.AwayFromZero);

                    // calculate distance
                    totalDistance = Math.Round(averageSpeed * totalTimeInHours, 2, MidpointRounding.AwayFromZero);

                    // calculate altitiude
                    totalAltitude = filteredSessionData.Sum(s => s.Altitude / 10);
                    averageAltitude = Math.Round(totalAltitude / totalCount, 2, MidpointRounding.AwayFromZero);
                    maximumAltitude = Math.Round(filteredSessionData.MaxBy(s => s.Altitude).Altitude / 10, 2, MidpointRounding.AwayFromZero);
                }
            }

            // calculate heart rate
            var totalHeartRate = filteredSessionData.Sum(s => s.HeartRate);
            var averageHeartRate =  Math.Round(totalHeartRate / totalCount, 2, MidpointRounding.AwayFromZero);
            var minimumHeartRate = filteredSessionData.MinBy(s => s.HeartRate).HeartRate;
            var maximumHeartRate = filteredSessionData.MaxBy(s => s.HeartRate).HeartRate;

            // calculate power
            var totalPower = filteredSessionData.Sum(s => s.Power);
            var averagePower =  Math.Round(totalPower / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumPower =  Math.Round(filteredSessionData.MaxBy(s => s.Power).Power, 2, MidpointRounding.AwayFromZero);

            // calculate cadence
            var totalCadence = filteredSessionData.Sum(s => s.Cadence);
            var averageCadence = Math.Round(totalCadence / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumCadence = Math.Round(filteredSessionData.MaxBy(s => s.Cadence).Cadence, 2, MidpointRounding.AwayFromZero);

            // calculate normalized power
            var filteredSessionDto = new SessionDto()
            {
                Interval = session.Interval,
                SessionData = filteredSessionData
            };
            var normalizedPower = filteredSessionDto.CalculateNormalizedPower();

            // calculate intensity factor
            var functionalThresholdPower = _context.Athletes.Single(a => a.Id == session.AthleteId).FunctionalThresholdPower;
            var intensityFactor = normalizedPower.CalculateIntensityFactor(functionalThresholdPower);

            // calculate training stress score
            var sessionTimeInSeconds = filteredSessionData.Count * filteredSessionDto.Interval; //session.Length.TimeOfDay.TotalSeconds;
            double trainingStressScore = 0;
            string trainingStressScoreStatus = "";

            if (normalizedPower != 0)
            {
                trainingStressScore = Metrics.CalculateTrainingStressScore(sessionTimeInSeconds, normalizedPower, intensityFactor, functionalThresholdPower);
                trainingStressScoreStatus = Metrics.TrainingStressScoreStatus(trainingStressScore);
            }

            var sessionSummaryDto = new SessionSummaryDto()
            {
                AverageAltitude = averageAltitude,
                MaximumAltitude = maximumAltitude,
                AverageHeartRate = averageHeartRate,
                MinimumHeartRate = minimumHeartRate,
                MaximumHeartRate = maximumHeartRate,
                AveragePower = averagePower,
                MaximumPower = maximumPower,
                AverageCadence = averageCadence,
                MaximumCadence = maximumCadence,
                AverageSpeed = averageSpeed,
                MaximumSpeed = maximumSpeed,
                TotalDistance = totalDistance,
                NormalizedPower = normalizedPower,
                IntensityFactor = intensityFactor,
                TrainingStressScore = trainingStressScore,
                TrainingStressScoreStatus = trainingStressScoreStatus,
                Date = session.Date,
                SessionId = session.Id
            };

            return sessionSummaryDto;
        }

        public List<SessionCalendarDto> GetCalendarData(int athleteId)
        {
            var sessionCalendarDtos = _context.Sessions.Where(x => x.AthleteId == athleteId).ToList().Select(s => new SessionCalendarDto()
            {
                Id = s.Id,
                Title = s.Title,
                StartDate = s.Date,
                EndDate = DateFormat.CalculateSessionDataRowDate(s.Date, s.SessionData.OrderByDescending(x => x.Row).First().Row, s.Interval), // gets the last row from the SessionData of the session
                AthleteId = athleteId
            }).ToList();

            return sessionCalendarDtos;
        }

        public List<DetectedInterval> DetectIntervals(SessionDto session)
        {
            var sessionData = session.SessionData;
            var interval = session.Interval;
            var potentialIntervalStart = new SessionDataDto();
            var detectedIntervalEnd = new SessionDataDto();
            var detectedIntervals = new List<DetectedInterval>();

            for (var x = 0; x < sessionData.Count; x++)
            {
                bool potentialIntervalDetected = false;
                bool intervalDetected = false;

                for (var p = x; p < sessionData.Count; p++)
                {
                    // get average of proceeding 14 seconds of powers - time taken for rider to reach maximum power
                    var currentPowers = new List<double>();
                    var proceedingPowers = new List<double>();

                    for (var i = 0; i < 14; i++)
                    {
                        if (p + (i + 1) < sessionData.Count)
                        {
                            if (sessionData[p + i].Power == 0) // rider must be applying power for next 14 seconds
                            {
                                break;
                            }
                            currentPowers.Add(sessionData[p + i].Power); // get power for the next 14 seconds
                            proceedingPowers.Add(sessionData[(p + 1) + i].Power); // get power for the next 14 seconds starting at current power +1
                        }
                    }

                    if (currentPowers.Count == 0) // no powers added to the last - last detected power was 0
                    {
                        break;
                    }

                    var currentPowersAverage = currentPowers.Average();
                    var proceedingPowersAverage = proceedingPowers.Average();

                    // check for potential interval
                    if (currentPowersAverage < proceedingPowersAverage)
                    {
                        if (!potentialIntervalDetected)
                        {
                            potentialIntervalStart = sessionData[p];
                            potentialIntervalDetected = true;
                        }
                    }
                    else // possible that cyclist built up speed and reached interval speed to maintain
                    {
                        var potentialIntervalPowerToMaintain = sessionData[p].Power;

                        var percentage = ((potentialIntervalPowerToMaintain * 40) / 100);
                        var minimumPowerRange = potentialIntervalPowerToMaintain - percentage;
                        var maximumPowerRange = potentialIntervalPowerToMaintain + percentage;
                        var minimumIntervalDuration = 10; // interval power must be maintained for atleast 10 seconds

                        var timer = 0;
                        var counter = 1;
                        for (var q = p; q < sessionData.Count; q++)
                        {
                            if (sessionData[q].Power > minimumPowerRange && sessionData[q].Power < maximumPowerRange)
                            {
                                timer += interval * counter;
                            }
                            else
                            {
                                if (timer > minimumIntervalDuration)
                                {
                                    intervalDetected = true;
                                    detectedIntervalEnd = sessionData[q];

                                    var startTime = potentialIntervalStart.Row * interval;
                                    var finishTime = detectedIntervalEnd.Row * interval;

                                    var intervalData = GetSessionDataSubset(new SessionDataSubsetDto() // get interval summary
                                    {
                                        MinimumSecond = startTime,
                                        MaximumSecond = finishTime,
                                        Unit = 0,
                                        SessionId = session.Id
                                    });
                                    
                                    detectedIntervals.Add(new DetectedInterval(startTime, finishTime, intervalData.AveragePower, true));
                                }
                                break;
                            }
                        }
                    }

                    if (intervalDetected)
                    {
                        x = detectedIntervalEnd.Row * interval; // start detecting new interval at the end of the detected interval
                        potentialIntervalStart = sessionData[detectedIntervalEnd.Row * interval];
                        break;
                    }
                }
            }

            // detect workout and rest periods
            var totalCount = sessionData.Count();
            var totalPower = sessionData.Sum(s => s.Power);
            var averagePower =  Math.Round(totalPower / totalCount, 2, MidpointRounding.AwayFromZero);
            var minimumIntervalPower = ((averagePower * 80) / 100); // workout periods must be greater than 30% of average session power

            for (var f = 0; f < detectedIntervals.Count; f ++)
            {
                detectedIntervals[f].IsRest = detectedIntervals[f].AveragePower < minimumIntervalPower;
            }

            return detectedIntervals;
        }

        public SessionSummaryDto GetSingleIntervalSummary(SessionDataSubsetDto sessionDataSubsetDto)
        {
            var singleIntervalSummary = GetSessionDataSubset(sessionDataSubsetDto);

            return singleIntervalSummary;
        }

        public List<SessionSummaryDto> GetIntervalSummary(IntervalSummaryRequestDto intervalSummaryRequestDto)
        {
            var session = GetSingle(intervalSummaryRequestDto.SessionId);
            var detectedIntervals = DetectIntervals(session);
            var workoutIntervals = detectedIntervals.Where(x => !x.IsRest).ToList();
            var restIntervals = detectedIntervals.Where(x => x.IsRest).ToList();

            var workoutSummary = CalculateWorkoutSummary(session, workoutIntervals, intervalSummaryRequestDto.Unit);
            var restSummary = CalculateWorkoutSummary(session, restIntervals, intervalSummaryRequestDto.Unit);

            var intervalSummaries = new List<SessionSummaryDto> { workoutSummary, restSummary };
            
            return intervalSummaries;
        }

        public SessionSummaryDto CalculateWorkoutSummary(SessionDto session, List<DetectedInterval> detectedIntervals, int unit)
        {
            var sessionSummaryDtoList = new List<SessionSummaryDto>();

            for (var w = 0; w < detectedIntervals.Count; w++)
            {
                var sessionDataSubsetDto = new SessionDataSubsetDto()
                {
                    MinimumSecond = (double)detectedIntervals[w].StartTime,
                    MaximumSecond = (double)detectedIntervals[w].FinishTime,
                    SessionId = session.Id,
                    Unit = unit
                };

                var sessionSummaryDto = GetSessionDataSubset(sessionDataSubsetDto);
                sessionSummaryDtoList.Add(sessionSummaryDto);
            }

            var totalCount = sessionSummaryDtoList.Count();

            // calculate speed
            var averageSpeed = Math.Round(sessionSummaryDtoList.Sum(x => x.AverageSpeed) / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumSpeed = sessionSummaryDtoList.MaxBy(s => s.MaximumSpeed).MaximumSpeed;

            // calculate distance
            var totalDistance = Math.Round(sessionSummaryDtoList.Sum(x => x.TotalDistance), 2, MidpointRounding.AwayFromZero);

            // calculate altitiude
            var averageAltitude = Math.Round(sessionSummaryDtoList.Sum(x => x.AverageAltitude) / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumAltitude = sessionSummaryDtoList.MaxBy(s => s.MaximumAltitude).MaximumAltitude;

            // calculate heart rate
            var averageHeartRate =  Math.Round(sessionSummaryDtoList.Sum(x => x.AverageHeartRate / totalCount), 2, MidpointRounding.AwayFromZero);
            var minimumHeartRate = sessionSummaryDtoList.MinBy(s => s.MinimumHeartRate).MinimumHeartRate;
            var maximumHeartRate = sessionSummaryDtoList.MinBy(s => s.MaximumHeartRate).MaximumHeartRate;

            // calculate power
            var averagePower =  Math.Round(sessionSummaryDtoList.Sum(x => x.AveragePower) / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumPower = sessionSummaryDtoList.MaxBy(s => s.MaximumPower).MaximumPower;

            // calculate cadence
            var averageCadence =  Math.Round(sessionSummaryDtoList.Sum(x => x.AverageCadence) / totalCount, 2, MidpointRounding.AwayFromZero);
            var maximumCadence = sessionSummaryDtoList.MaxBy(s => s.MaximumCadence).MaximumCadence;

            var workoutSummary = new SessionSummaryDto()
            {
                AverageAltitude = averageAltitude,
                MaximumAltitude = maximumAltitude,
                AverageHeartRate = averageHeartRate,
                MinimumHeartRate = minimumHeartRate,
                MaximumHeartRate = maximumHeartRate,
                AveragePower = averagePower,
                MaximumPower = maximumPower,
                AverageCadence = averageCadence,
                MaximumCadence = maximumCadence,
                AverageSpeed = averageSpeed,
                MaximumSpeed = maximumSpeed,
                TotalDistance = totalDistance,
                Date = session.Date,
                SessionId = session.Id
            };

            return workoutSummary;
        }
    }
}