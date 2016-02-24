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
                new SessionData(){ Id = 0, HeartRate = 125, Speed = 214, Cadence = 55, Altitude = 33, Power = 19, Row = 1, SessionId = 1 },
                new SessionData(){ Id = 1, HeartRate = 129, Speed = 259, Cadence = 60, Altitude = 32, Power = 477, Row = 2, SessionId = 1 },
                new SessionData(){ Id = 2, HeartRate = 132, Speed = 308, Cadence = 70, Altitude = 32, Power = 837, Row = 3, SessionId = 1 },
                new SessionData(){ Id = 3, HeartRate = 135, Speed = 322, Cadence = 79, Altitude = 32, Power = 799, Row = 4, SessionId = 1 },
                new SessionData(){ Id = 4, HeartRate = 139, Speed = 334, Cadence = 90, Altitude = 32, Power = 735, Row = 5, SessionId = 1 },
                new SessionData(){ Id = 5, HeartRate = 145, Speed = 338, Cadence = 92, Altitude = 32, Power = 740, Row = 6, SessionId = 1 }
            };

            _sessionList = new List<Session>()
            {
                new Session(){ Id = 1, Date = new DateTime(2009, 07, 25, 14 ,26, 18, 0), Length = new DateTime(2009, 07, 25, 1, 14, 21, 100), Interval = 1, SessionData = _sessionDataList }
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
            var sessionSummary = _sessionRepository.GetSummary(1);

            var averageSpeed = sessionSummary.AverageSpeed;
            var averageHeatRate = sessionSummary.AverageHeartRate;
            var averageAltitude = sessionSummary.AverageAltitude;
            var averagePower = sessionSummary.AveragePower;

            Assert.AreEqual(29.58m, averageSpeed);
            Assert.AreEqual(134.17, averageHeatRate);
            Assert.AreEqual(32.17m, averageAltitude);
            Assert.AreEqual(601.17m, averagePower);
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

            Assert.AreEqual(33.8m, maximumSpeed);
            Assert.AreEqual(145, maximumHeatRate);
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

            Assert.AreEqual(36.66, totalDistanceKilometres);
            Assert.AreEqual(22.78, totalDistanceMiles);
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
            Assert.AreEqual(32, averageAltitude);
            Assert.AreEqual(704.33, averagePower);
        }

    }
}
