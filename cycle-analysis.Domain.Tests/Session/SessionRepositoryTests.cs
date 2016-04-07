/****************************** Cycle Analysis ******************************\
Description: Cycle Analysis Software
Author: Joe Houghton - C3375905
Assignment: Advanced Software Engineering B

All other rights reserved.

THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND,
EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
\***************************************************************************/
namespace cycle_analysis.Domain.Tests.Session
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using cycle_analysis.Domain.Athlete.Models;
    using cycle_analysis.Domain.Context;
    using cycle_analysis.Domain.Helper;
    using cycle_analysis.Domain.Session;
    using cycle_analysis.Domain.Session.Dto;
    using cycle_analysis.Domain.Session.Models;
    using cycle_analysis.Domain.SessionData.Dtos;
    using cycle_analysis.Domain.SessionData.Models;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class SessionRepositoryTests
    {
        private CycleAnalysisContext _context;
        private readonly SessionRepository _sessionRepository;
        private readonly List<SessionData> _sessionDataList;
        private readonly List<Session> _sessionList;
        private readonly List<Athlete> _athleteList;

        /// <summary>
        /// Constructor creates a mock context class with a mocked DbSet, which is passed into the SessionRepository.
        /// </summary>
        public SessionRepositoryTests()
        {
            // create connection string so we're not reliant on Web.config connection strings
             var connectionString = new SqlConnectionStringBuilder() { DataSource = ".", IntegratedSecurity = true, InitialCatalog = "RandomDbName" }.ConnectionString;

            // pass connection string to context class
             _context = new CycleAnalysisContext(connectionString);

            _sessionDataList = new List<SessionData>
            {
                new SessionData(){ Id = 0, HeartRate = 125, Speed = 214, Cadence = 55, Altitude = 33, Power = 19, Row = 1, SessionId = 1 },
                new SessionData(){ Id = 1, HeartRate = 129, Speed = 259, Cadence = 60, Altitude = 32, Power = 477, Row = 2, SessionId = 1 },
                new SessionData(){ Id = 2, HeartRate = 132, Speed = 308, Cadence = 70, Altitude = 32, Power = 837, Row = 3, SessionId = 1 },
                new SessionData(){ Id = 3, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 799, Row = 4, SessionId = 1 },
                new SessionData(){ Id = 4, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 735, Row = 5, SessionId = 1 },
                new SessionData(){ Id = 5, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 740, Row = 6, SessionId = 1 },

                // session used for interval detection
                // first interval to detect
                new SessionData(){ Id = 1, HeartRate = 125, Speed = 214, Cadence = 55, Altitude = 33, Power = 19, Row = 1, SessionId = 2},
                new SessionData(){ Id = 2, HeartRate = 129, Speed = 259, Cadence = 60, Altitude = 32, Power = 20, Row = 2, SessionId = 2 },
                new SessionData(){ Id = 3, HeartRate = 132, Speed = 308, Cadence = 70, Altitude = 32, Power = 18, Row = 3, SessionId = 2 },
                new SessionData(){ Id = 4, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 20, Row = 4, SessionId = 2 }, // potential interval
                new SessionData(){ Id = 5, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 23, Row = 5, SessionId = 2 },
                new SessionData(){ Id = 6, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 27, Row = 6, SessionId = 2 },
                new SessionData(){ Id = 7, HeartRate = 125, Speed = 214, Cadence = 55, Altitude = 33, Power = 30, Row = 7, SessionId = 2 },
                new SessionData(){ Id = 8, HeartRate = 129, Speed = 259, Cadence = 60, Altitude = 32, Power = 35, Row = 8, SessionId = 2 },
                new SessionData(){ Id = 9, HeartRate = 132, Speed = 308, Cadence = 70, Altitude = 32, Power = 40, Row = 9, SessionId = 2 }, // potential interval start
                new SessionData(){ Id = 10, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 10, SessionId = 2 },
                new SessionData(){ Id = 11, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 11, SessionId = 2 },
                new SessionData(){ Id = 12, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 12, SessionId = 2 },
                new SessionData(){ Id = 13, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 13, SessionId = 2 },
                new SessionData(){ Id = 14, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 14, SessionId = 2 },
                new SessionData(){ Id = 15, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 15, SessionId = 2 },
                new SessionData(){ Id = 16, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 16, SessionId = 2 },
                new SessionData(){ Id = 17, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 17, SessionId = 2 },
                new SessionData(){ Id = 18, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 18, SessionId = 2 },
                new SessionData(){ Id = 19, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 19, SessionId = 2 },
                new SessionData(){ Id = 20, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 20, SessionId = 2 },
                new SessionData(){ Id = 21, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 21, SessionId = 2 },
                new SessionData(){ Id = 22, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 22, SessionId = 2 },
                new SessionData(){ Id = 23, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 23, SessionId = 2 },
                new SessionData(){ Id = 24, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 24, SessionId = 2 },
                new SessionData(){ Id = 25, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 25, SessionId = 2 },
                new SessionData(){ Id = 26, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 26, SessionId = 2 },
                new SessionData(){ Id = 27, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 27, SessionId = 2 },
                new SessionData(){ Id = 28, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 28, SessionId = 2 },
                new SessionData(){ Id = 29, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 29, SessionId = 2 },
                new SessionData(){ Id = 30, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 30, SessionId = 2 },
                new SessionData(){ Id = 31, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 31, SessionId = 2 },
                new SessionData(){ Id = 32, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 32, SessionId = 2 },
                new SessionData(){ Id = 33, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 33, SessionId = 2 },
                new SessionData(){ Id = 34, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 34, SessionId = 2 },
                new SessionData(){ Id = 35, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 35, SessionId = 2 },
                new SessionData(){ Id = 36, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 36, SessionId = 2 },
                new SessionData(){ Id = 37, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 37, SessionId = 2 },
                new SessionData(){ Id = 38, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 38, SessionId = 2 },
                new SessionData(){ Id = 39, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 39, SessionId = 2 },
                new SessionData(){ Id = 40, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 6, Row = 40, SessionId = 2 }, // interval end
                new SessionData(){ Id = 41, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 3, Row = 41, SessionId = 2 },
                new SessionData(){ Id = 42, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 0, Row = 42, SessionId = 2 },

                // second interval to detect
                new SessionData(){ Id = 43, HeartRate = 125, Speed = 214, Cadence = 55, Altitude = 33, Power = 19, Row = 43, SessionId = 2}, // potential interval
                new SessionData(){ Id = 44, HeartRate = 129, Speed = 259, Cadence = 60, Altitude = 32, Power = 20, Row = 44, SessionId = 2 },
                new SessionData(){ Id = 45, HeartRate = 132, Speed = 308, Cadence = 70, Altitude = 32, Power = 18, Row = 45, SessionId = 2 },
                new SessionData(){ Id = 46, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 20, Row = 46, SessionId = 2 },
                new SessionData(){ Id = 47, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 23, Row = 47, SessionId = 2 },
                new SessionData(){ Id = 48, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 27, Row = 48, SessionId = 2 },
                new SessionData(){ Id = 49, HeartRate = 125, Speed = 214, Cadence = 55, Altitude = 33, Power = 30, Row = 49, SessionId = 2 },
                new SessionData(){ Id = 50, HeartRate = 129, Speed = 259, Cadence = 60, Altitude = 32, Power = 35, Row = 50, SessionId = 2 },
                new SessionData(){ Id = 51, HeartRate = 132, Speed = 308, Cadence = 70, Altitude = 32, Power = 40, Row = 51, SessionId = 2 }, // potential interval start
                new SessionData(){ Id = 52, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 52, SessionId = 2 },
                new SessionData(){ Id = 53, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 53, SessionId = 2 },
                new SessionData(){ Id = 54, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 54, SessionId = 2 },
                new SessionData(){ Id = 55, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 55, SessionId = 2 },
                new SessionData(){ Id = 56, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 56, SessionId = 2 },
                new SessionData(){ Id = 57, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 57, SessionId = 2 },
                new SessionData(){ Id = 58, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 58, SessionId = 2 },
                new SessionData(){ Id = 59, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 59, SessionId = 2 },
                new SessionData(){ Id = 60, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 60, SessionId = 2 },
                new SessionData(){ Id = 61, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 61, SessionId = 2 },
                new SessionData(){ Id = 62, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 62, SessionId = 2 },
                new SessionData(){ Id = 63, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 39, Row = 63, SessionId = 2 },
                new SessionData(){ Id = 64, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 64, SessionId = 2 },
                new SessionData(){ Id = 65, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 65, SessionId = 2 },
                new SessionData(){ Id = 66, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 66, SessionId = 2 },
                new SessionData(){ Id = 67, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 67, SessionId = 2 },
                new SessionData(){ Id = 68, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 68, SessionId = 2 },
                new SessionData(){ Id = 69, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 69, SessionId = 2 },
                new SessionData(){ Id = 70, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 70, SessionId = 2 },
                new SessionData(){ Id = 71, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 71, SessionId = 2 },
                new SessionData(){ Id = 72, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 72, SessionId = 2 },
                new SessionData(){ Id = 73, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 73, SessionId = 2 },
                new SessionData(){ Id = 74, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 74, SessionId = 2 },
                new SessionData(){ Id = 75, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 75, SessionId = 2 },
                new SessionData(){ Id = 76, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 76, SessionId = 2 },
                new SessionData(){ Id = 77, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 39, Row = 77, SessionId = 2 },
                new SessionData(){ Id = 78, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 40, Row = 78, SessionId = 2 },
                new SessionData(){ Id = 79, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 79, SessionId = 2 },
                new SessionData(){ Id = 80, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 80, SessionId = 2 },
                new SessionData(){ Id = 81, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 39, Row = 81, SessionId = 2 },
                new SessionData(){ Id = 82, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 6, Row = 82, SessionId = 2 }, // interval end
                new SessionData(){ Id = 83, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 3, Row = 83, SessionId = 2 },
                new SessionData(){ Id = 84, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 0, Row = 84, SessionId = 2 }
            };

            _sessionList = new List<Session>()
            {
                new Session(){ Id = 1, Date = new DateTime(2009, 07, 25, 14 ,26, 18, 0), Length = new DateTime(2009, 07, 25, 1, 14, 21, 100), Interval = 1, SMode = 000000000, SessionData = _sessionDataList, AthleteId = 1},
                new Session(){ Id = 2, Date = new DateTime(2010, 07, 25, 14 ,26, 18, 0), Length = new DateTime(2009, 07, 25, 1, 14, 21, 100), Interval = 1, SMode = 000000010, SessionData = _sessionDataList, AthleteId = 1 }
            };

            _athleteList = new List<Athlete>()
            {
                new Athlete(){ Id = 1, FunctionalThresholdPower = 320}
            };

            // convert the IEnumerable _sessionList to an IQueryable list
            IQueryable<Session> queryableListSession = _sessionList.AsQueryable();

            // convert the IEnumerable _sessionDataList to an IQueryable list
            IQueryable<SessionData> queryableListSessionData = _sessionDataList.AsQueryable();

            // convert the IEnumerable _athleteList to an IQueryable list
            IQueryable<Athlete> queryableListAthlete = _athleteList.AsQueryable();

            // force DbSet to return the IQueryable members of converted list object as its data source
            var mockSetSession = new Mock<DbSet<Session>>();
            mockSetSession.As<IQueryable<Session>>().Setup(m => m.Provider).Returns(queryableListSession.Provider);
            mockSetSession.As<IQueryable<Session>>().Setup(m => m.Expression).Returns(queryableListSession.Expression);
            mockSetSession.As<IQueryable<Session>>().Setup(m => m.ElementType).Returns(queryableListSession.ElementType);
            mockSetSession.As<IQueryable<Session>>().Setup(m => m.GetEnumerator()).Returns(queryableListSession.GetEnumerator());

            // force DbSet to return the IQueryable members of converted list object as its data source
            var mockSetSessionData = new Mock<DbSet<SessionData>>();
            mockSetSessionData.As<IQueryable<SessionData>>().Setup(m => m.Provider).Returns(queryableListSessionData.Provider);
            mockSetSessionData.As<IQueryable<SessionData>>().Setup(m => m.Expression).Returns(queryableListSessionData.Expression);
            mockSetSessionData.As<IQueryable<SessionData>>().Setup(m => m.ElementType).Returns(queryableListSessionData.ElementType);
            mockSetSessionData.As<IQueryable<SessionData>>().Setup(m => m.GetEnumerator()).Returns(queryableListSessionData.GetEnumerator());

            // force DbSet to return the IQueryable members of converted list object as its data source
            var mockSetAthlete = new Mock<DbSet<Athlete>>();
            mockSetAthlete.As<IQueryable<Athlete>>().Setup(m => m.Provider).Returns(queryableListAthlete.Provider);
            mockSetAthlete.As<IQueryable<Athlete>>().Setup(m => m.Expression).Returns(queryableListAthlete.Expression);
            mockSetAthlete.As<IQueryable<Athlete>>().Setup(m => m.ElementType).Returns(queryableListAthlete.ElementType);
            mockSetAthlete.As<IQueryable<Athlete>>().Setup(m => m.GetEnumerator()).Returns(queryableListAthlete.GetEnumerator());

            // context class will return mocked DbSets 
            _context.Sessions = mockSetSession.Object;
            _context.SessionData = mockSetSessionData.Object;
            _context.Athletes = mockSetAthlete.Object;

            // pass context to repository
            _sessionRepository = new SessionRepository(_context);
        }
        /// <summary>
        /// DateTime seconds for each session header row should be calculated by interval.
        /// </summary>
        [Test]
        public void DateTimeSecondsForSessionHeaderRowsShouldBeCalculatedByInterval()
        {
            var interval = 1;
            var startDate = new DateTime(2016, 10, 05, 09, 30, 5, 1); // hh:mm:s:ms - 09:30:5:100

            var sessionRow1 = new SessionDataDto() { Row = 0 };
            var sessionRow2 = new SessionDataDto() { Row = 1 };
            var sessionRow3 = new SessionDataDto() { Row = 2 };

            // calculate seconds for each session header row
            sessionRow1.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow1.Row);
            sessionRow2.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow2.Row);
            sessionRow3.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow3.Row);

            Assert.AreEqual(5, sessionRow1.Date.Second);
            Assert.AreEqual(6, sessionRow2.Date.Second);
            Assert.AreEqual(7, sessionRow3.Date.Second);
        }

        /// <summary>
        /// DateTime minutes for each session header row should be calculated by interval.
        /// </summary>
        [Test]
        public void DateTimeMinutesForSessionHeaderRowsShouldBeCalculatedByInterval()
        {
            var interval = 1;
            var startDate = new DateTime(2016, 10, 05, 09, 59, 59, 1); // hh:mm:s:ms - 09:30:5:100

            var sessionRow1 = new SessionDataDto() { Row = 0 };
            var sessionRow2 = new SessionDataDto() { Row = 1 };
            var sessionRow3 = new SessionDataDto() { Row = 2 };

            // calculate seconds for each session header row
            sessionRow1.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow1.Row);
            sessionRow2.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow2.Row);
            sessionRow3.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow3.Row);

            // assert seconds
            Assert.AreEqual(59, sessionRow1.Date.Second);
            Assert.AreEqual(0, sessionRow2.Date.Second);
            Assert.AreEqual(1, sessionRow3.Date.Second);

            // assert minutes
            Assert.AreEqual(59, sessionRow1.Date.Minute);
            Assert.AreEqual(0, sessionRow2.Date.Minute);
            Assert.AreEqual(0, sessionRow3.Date.Minute);
        }

        /// <summary>
        /// DateTime hours for each session header row should be calculated by interval.
        /// </summary>
        [Test]
        public void DateTimeHoursForSessionHeaderRowsShouldBeCalculatedByInterval()
        {
            var interval = 1;
            var startDate = new DateTime(2016, 10, 05, 09, 59, 59, 1); // hh:mm:s:ms - 09:30:5:100

            var sessionRow1 = new SessionDataDto() { Row = 0 };
            var sessionRow2 = new SessionDataDto() { Row = 1 };
            var sessionRow3 = new SessionDataDto() { Row = 2 };

            // calculate seconds for each session header row
            sessionRow1.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow1.Row);
            sessionRow2.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow2.Row);
            sessionRow3.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow3.Row);

            // assert seconds
            Assert.AreEqual(59, sessionRow1.Date.Second);
            Assert.AreEqual(0, sessionRow2.Date.Second);
            Assert.AreEqual(1, sessionRow3.Date.Second);

            // assert minutes
            Assert.AreEqual(09, sessionRow1.Date.Hour);
            Assert.AreEqual(10, sessionRow2.Date.Hour);
            Assert.AreEqual(10, sessionRow3.Date.Hour);
        }

        /// <summary>
        /// DateTime days for each session header row should be calculated by interval.
        /// </summary>
        [Test]
        public void DateTimeDaysForSessionHeaderRowsShouldBeCalculatedByInterval()
        {
            var interval = 1;
            var startDate = new DateTime(2016, 05, 10, 23, 59, 59, 1); // hh:mm:s:ms - 09:30:5:100

            var sessionRow1 = new SessionDataDto() { Row = 0 };
            var sessionRow2 = new SessionDataDto() { Row = 1 };
            var sessionRow3 = new SessionDataDto() { Row = 2 };

            // calculate seconds for each session header row
            sessionRow1.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow1.Row);
            sessionRow2.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow2.Row);
            sessionRow3.Date = DateFormat.CalculateSessionDataRowDate(startDate, interval, sessionRow3.Row);

            // assert seconds
            Assert.AreEqual(59, sessionRow1.Date.Second);
            Assert.AreEqual(0, sessionRow2.Date.Second);
            Assert.AreEqual(1, sessionRow3.Date.Second);

            // assert minutes
            Assert.AreEqual(10, sessionRow1.Date.Day);
            Assert.AreEqual(11, sessionRow2.Date.Day);
            Assert.AreEqual(11, sessionRow3.Date.Day);
        }

        /// <summary>
        /// Correct averages should be calculated.
        /// </summary>
        [Test]
        public void CorrectAveragesAreCalculated()
        {
            var sessionSummaryRequestDto = new SessionSummaryRequestDto() { SessionId = 1, Unit = 0 };
            var sessionSummary = _sessionRepository.GetSummary(sessionSummaryRequestDto);

            var averageSpeed = sessionSummary.AverageSpeed;
            var averageHeatRate = sessionSummary.AverageHeartRate;
            var averageAltitude = sessionSummary.AverageAltitude;
            var averagePower = sessionSummary.AveragePower;

            Assert.AreEqual(29.58m, averageSpeed);
            Assert.AreEqual(134.17, averageHeatRate);
            Assert.AreEqual(3.22d, averageAltitude);
            Assert.AreEqual(601.17m, averagePower);
        }

        /// <summary>
        /// Correct maximum values should be found.
        /// </summary>
        [Test]
        public void CorrectMaximumValuesAreFound()
        {
            var sessionSummaryRequestDto = new SessionSummaryRequestDto() { SessionId = 1, Unit = 0 };
            var sessionSummary = _sessionRepository.GetSummary(sessionSummaryRequestDto);

            var maximumSpeed = sessionSummary.MaximumSpeed;
            var maximumHeatRate = sessionSummary.MaximumHeartRate;
            var maximumAltitude = sessionSummary.MaximumAltitude;
            var maximumPower = sessionSummary.MaximumPower;

            Assert.AreEqual(33.8m, maximumSpeed);
            Assert.AreEqual(145, maximumHeatRate);
            Assert.AreEqual(3.3, maximumAltitude);
            Assert.AreEqual(837, maximumPower);
        }

        /// <summary>
        /// Correct minimum values should be found.
        /// </summary>
        [Test]
        public void CorrectMinimumValuesAreFound()
        {
            var sessionSummaryRequestDto = new SessionSummaryRequestDto() { SessionId = 1, Unit = 0 };
            var sessionSummary = _sessionRepository.GetSummary(sessionSummaryRequestDto);

            var minimumHeartRate = sessionSummary.MinimumHeartRate;

            Assert.AreEqual(125, minimumHeartRate);
        }

        /// <summary>
        /// Correct distance should be calculated.
        /// </summary>
        [Test]
        public void CorrectDistanceIsCalculated()
        {
            var sessionSummaryRequestDtoMetric = new SessionSummaryRequestDto() { SessionId = 1, Unit = 0 };
            var sessionSummaryRequestDtoImperial = new SessionSummaryRequestDto() { SessionId = 2, Unit = 0 };

            var sessionSummaryMetric = _sessionRepository.GetSummary(sessionSummaryRequestDtoMetric);
            var sessionSummaryImperial = _sessionRepository.GetSummary(sessionSummaryRequestDtoImperial);

            var totalDistanceKilometres =  sessionSummaryMetric.TotalDistance;
            var totalDistanceMiles = sessionSummaryImperial.TotalDistance;

            Assert.AreEqual(36.66, totalDistanceKilometres);
            Assert.AreEqual(64.43m, totalDistanceMiles);
        }

        /// <summary>
        /// The expected results should be returned when comparing dates.
        /// </summary>
        [Test]
        public void ExpectedResultsShouldBeReturnedWhenComparingDates()
        {
            DateTime date1 = new DateTime(1993, 05, 10, 08, 30, 20, 2); // earlier date
            DateTime date2 = new DateTime(1993, 05, 10, 09, 30, 20, 2);

            DateTime date3 = new DateTime(1993, 05, 10, 09, 30, 20, 2); // same date
            DateTime date4 = new DateTime(1993, 05, 10, 09, 30, 20, 2);

            DateTime date5 = new DateTime(1993, 05, 10, 10, 30, 20, 2); // later date
            DateTime date6 = new DateTime(1993, 05, 10, 09, 23, 20, 2);

            int earlierThan = DateTime.Compare(date1, date2);
            int sameDate = DateTime.Compare(date3, date4);
            int laterThan = DateTime.Compare(date5, date6);

            Assert.AreEqual(-1, earlierThan); // compare returns -1 for if first date is earlier than second
            Assert.AreEqual(0, sameDate); // compare returns 0 for if dates are the same
            Assert.AreEqual(1, laterThan); // compare returns 1 for if first date is later than the second
        }

        /// <summary>
        /// Seconds should be added to date.
        /// </summary>
        [Test]
        public void SecondsShouldBeAddedToDate()
        {
            DateTime date1 = new DateTime(1993, 05, 10, 09, 59, 20, 2); // earlier date
            DateTime date2 = new DateTime(1993, 05, 10, 10, 01, 00, 2);
            date1 = date1.AddSeconds(100);

            var checkAreEqual = DateTime.Compare(date1, date2);
            Assert.AreEqual(0, checkAreEqual); // compare returns 0 for if dates are the same
        }

        /// <summary>
        /// Dates within range should be returned.
        /// </summary>
        [Test]
        public void DatesWithinRangeShouldBeReturned()
        {
            var startDate = new DateTime(2013, 03, 05, 15, 46, 00, 0);
            var minimumSeconds = 0;
            var maximumSeconds = 60;

            var dates = new List<DateTime>()
            {
                new DateTime(2013, 03, 05, 14, 46, 20, 0), // too low
                new DateTime(2013, 03, 05, 15, 45, 59, 0), // too low
                new DateTime(2013, 03, 05, 15, 47, 01, 0), // too high
                new DateTime(2013, 03, 05, 15, 46, 00, 0), // minimum
                new DateTime(2013, 03, 05, 15, 46, 01, 0),
                new DateTime(2013, 03, 05, 15, 46, 50, 0),
                new DateTime(2013, 03, 05, 15, 47, 00, 0), // maximum
                new DateTime(2013, 03, 05, 16, 46, 01, 0)  // too high
            };

            var filteredDates = dates.FilterDates(startDate, minimumSeconds, maximumSeconds);

            Assert.AreEqual(4, filteredDates.Count);
        }

        /// <summary>
        /// Subset of SessionData should be returned for specified seconds.
        /// </summary>
        [Test]
        public void SubsetOfSessionDataShouldBeReturnedForSpecifiedSeconds()
        {
            var sessionDataSubsetdto = new SessionDataSubsetDto()
            {
                MinimumSecond = 2,
                MaximumSecond = 4,
                SessionId = 1
            };

            var filteredDates = _sessionRepository.GetSessionDataSubset(sessionDataSubsetdto);

            var averageSpeed = filteredDates.AverageSpeed;
            var averageHeartRate = filteredDates.AverageHeartRate;
            var averageAltitude = filteredDates.AverageAltitude;
            var averagePower = filteredDates.AveragePower;

            Assert.AreEqual(29.63, averageSpeed);
            Assert.AreEqual(132.0, averageHeartRate);
            Assert.AreEqual(3.2, averageAltitude);
            Assert.AreEqual(704.33, averagePower);
        }

        /// <summary>
        /// Units of measurement should be calculated depending on bit in SMode
        /// </summary>
        [Test]
        public void UnitsOfMeasurementShouldBeCalculatedDependingOnSMode()
        {
            var imperialSMode = "000000010";
            var metricSMode = "000000000";

            var isImperial = imperialSMode.IsMetric();
            var isMetric = metricSMode.IsMetric();

            Assert.AreEqual(isImperial, false);
            Assert.AreEqual(isMetric, true);
        }

        /// <summary>
        /// Normalized power is calculated correctly
        /// </summary>
        [Test]
        public void NormalizedPowerIsCalculatedCorrectly()
        {
            var interval = 1;

            var powers = new List<double>() { 10, 8, 7, 6, 4, 2, 7, 5, 6, 3 };

            // calculate a rolling 30 second average (of the preceding time points
            var movingAverages = powers.CalculateMovingAverages(3 / interval);

            // raise all the moving averages to the fourth power
            var averagesToFourthPower = movingAverages.ToPower(4);

            // find the average of values raised to fourth power
            var averageOfFourthPower = averagesToFourthPower.Average();

            // take the fourth root of the average values raised to the fourth power
            var normalizedPower =  Math.Round(averageOfFourthPower.NthRoot(4), 2, MidpointRounding.AwayFromZero);
            Assert.AreEqual(averagesToFourthPower[0], 4822.5308641975325d);
            Assert.AreEqual(averagesToFourthPower[7], 474.27160493827171d);
            Assert.AreEqual(normalizedPower, 6.1);
        }

        /// <summary>
        /// First 30 seconds are ignored
        /// </summary>
        [Test]
        public void First30SecondsAreIgnored()
        {
            var sessionData = new List<double>() {
            3, 6, 7, 4, 2, 333, 5, 7, 9, 2,
            99, 55, 33, 11, 44, 3, 6, 7, 4, 2,
            4, 5, 7, 9, 2, 99, 55, 33, 11, 663,
            54,74,88,66,55,44,777 };

            var interval = 1;
            var powers = new List<double>();

            for (var x = 0; x < sessionData.Count; x++)
            {
                if (((x + 1) * interval) >= 30) // start rolling average at 30 seconds
                {
                    powers.Add(sessionData[x]);
                }
            }

            var firstPower = powers[0];

            Assert.AreEqual(663, firstPower);
        }

        /// <summary>
        /// First 3 seconds of dates are ignored
        /// </summary>
        [Test]
        public void First3SecondsOfDatesAreIgnored()
        {
            var sessionData = _sessionRepository.GetSingle(1);
            var interval = sessionData.Interval;
            var powers = new List<double>{};

            for (var x = 0; x < sessionData.SessionData.Count; x++)
            {
                if (((x + 1) * interval) >= 3) // start rolling average at 3 seconds
                {
                    powers.Add(sessionData.SessionData[x].Power);
                }
            }

            var firstPower = powers[0];
            var lastpower =  powers[3];

            Assert.AreEqual(837m, firstPower);
            Assert.AreEqual(740m, lastpower);
        }


        /// <summary>
        /// Training Stress Score Is Calculated
        /// </summary>
        [Test]
        public void TrainingStressScoreIsCalculated()
        {
            var seconds = 3978.8999999999996;
            var intensityFactor = 0.72;
            var normalizedPower = 231.95;
            var functionalThresholdPower = 320;

            var trainingStressScore = Metrics.CalculateTrainingStressScore(seconds, normalizedPower, intensityFactor, functionalThresholdPower);

            Assert.AreEqual(58.0d, trainingStressScore);
        }

        /// <summary>
        /// The start and end of the first two intervals are detected
        /// </summary>
        [Test]
        public void DetectFirstAndSecondIntervals()
        {
            var session = _sessionRepository.GetSingle(2);
            var detectedIntervals = _sessionRepository.DetectIntervals(session);

            var detectedInterval1 = detectedIntervals[0];
            var detectedInterval2 = detectedIntervals[1];

            Assert.AreEqual(1, detectedInterval1.StartTime);
            Assert.AreEqual(40, detectedInterval1.FinishTime);

            Assert.AreEqual(43, detectedInterval2.StartTime);
            Assert.AreEqual(82, detectedInterval2.FinishTime);
        }
    }

}
