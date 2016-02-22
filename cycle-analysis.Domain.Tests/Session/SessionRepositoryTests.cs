namespace cycle_analysis.Domain.Tests.Session
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.SqlClient;
    using System.Linq;
    using cycle_analysis.Domain.Context;
    using cycle_analysis.Domain.Helper;
    using cycle_analysis.Domain.Session;
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
                new SessionData(){ Id = 1, HeartRate = 125, Speed = 214, Cadence = 55, Altitude = 33, Power = 19, SessionId = 1 },
                new SessionData(){ Id = 2, HeartRate = 129, Speed = 259, Cadence = 60, Altitude = 32, Power = 477, SessionId = 1 },
                new SessionData(){ Id = 3, HeartRate = 132, Speed = 308, Cadence = 70, Altitude = 32, Power = 837, SessionId = 1 },
                new SessionData(){ Id = 4, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 799, SessionId = 1 },
                new SessionData(){ Id = 5, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 735, SessionId = 1 }
            };

            _sessionList = new List<Session>()
            {
                new Session(){ Id = 1, Date = new DateTime(2009, 07, 25, 14 ,26, 18, 0), Length = new DateTime(1754, 01, 01, 1, 14, 21, 100), SessionData = _sessionDataList }
            };

            // convert the IEnumerable _sessionList to an IQueryable list
            IQueryable<Session> queryableListSession = _sessionList.AsQueryable();

            // convert the IEnumerable _sessionDataList to an IQueryable list
            IQueryable<SessionData> queryableListSessionData = _sessionDataList.AsQueryable();

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

            // context class will return mocked DbSets 
            _context.Sessions = mockSetSession.Object;
            _context.SessionData = mockSetSessionData.Object;
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

            var sessionRow1 = new SessionDataDto() { Row = 1 };
            var sessionRow2 = new SessionDataDto() { Row = 2 };
            var sessionRow3 = new SessionDataDto() { Row = 3 };

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

            var sessionRow1 = new SessionDataDto() { Row = 1 };
            var sessionRow2 = new SessionDataDto() { Row = 2 };
            var sessionRow3 = new SessionDataDto() { Row = 3 };

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

            var sessionRow1 = new SessionDataDto() { Row = 1 };
            var sessionRow2 = new SessionDataDto() { Row = 2 };
            var sessionRow3 = new SessionDataDto() { Row = 3 };

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

            var sessionRow1 = new SessionDataDto() { Row = 1 };
            var sessionRow2 = new SessionDataDto() { Row = 2 };
            var sessionRow3 = new SessionDataDto() { Row = 3 };

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
            var sessionSumamry = _sessionRepository.GetSummary(1);

            var averageSpeed = sessionSumamry.AverageSpeed;
            var averageHeatRate = sessionSumamry.AverageHeartRate;
            var averageAltitude = sessionSumamry.AverageAltitude;
            var averagePower = sessionSumamry.AveragePower;

            Assert.AreEqual(28.74m, averageSpeed);
            Assert.AreEqual(132, averageHeatRate);
            Assert.AreEqual(32.2m, averageAltitude);
            Assert.AreEqual(573.4m, averagePower);
        }

        /// <summary>
        /// Correct maximum values should be found.
        /// </summary>
        [Test]
        public void CorrectMaximumValuesAreFound()
        {
            var sessionSumamry = _sessionRepository.GetSummary(1);

            var maximumSpeed = sessionSumamry.MaximumSpeed;
            var maximumHeatRate = sessionSumamry.MaximumHeartRate;
            var maximumAltitude = sessionSumamry.MaximumAltitude;
            var maximumPower = sessionSumamry.MaximumPower;

            Assert.AreEqual(33.4m, maximumSpeed);
            Assert.AreEqual(139, maximumHeatRate);
            Assert.AreEqual(33, maximumAltitude);
            Assert.AreEqual(837, maximumPower);
        }

        /// <summary>
        /// Correct minimum values should be found.
        /// </summary>
        [Test]
        public void CorrectMinimumValuesAreFound()
        {
            var sessionSumamry = _sessionRepository.GetSummary(1);

            var minimumHeartRate = sessionSumamry.MinimumHeartRate;

            Assert.AreEqual(125, minimumHeartRate);
        }

        /// <summary>
        /// Correct distance should be calculated.
        /// </summary>
        [Test]
        public void CorrectDistanceIsCalculated()
        {
            var sessionSumamry = _sessionRepository.GetSummary(1);

            var totalDistanceKilometres =  sessionSumamry.TotalDistanceKilometres;
            var totalDistanceMiles = sessionSumamry.TotalDistanceMiles;

            Assert.AreEqual(35.61, totalDistanceKilometres);
            Assert.AreEqual(22.13, totalDistanceMiles);
        }
    }
}
