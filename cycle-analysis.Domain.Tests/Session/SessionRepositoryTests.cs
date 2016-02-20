namespace cycle_analysis.Domain.Tests.Session
{
    using System;
    using cycle_analysis.Domain.Helper;
    using cycle_analysis.Domain.SessionData.Dtos;
    using NUnit.Framework;

    [TestFixture]
    public class SessionRepositoryTests
    {
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
            sessionRow1.Date = DateFormat.CalculateSessionDataRowDate(startDate ,interval, sessionRow1.Row);
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
        public void DateTimDaysForSessionHeaderRowsShouldBeCalculatedByInterval()
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

    }
}
