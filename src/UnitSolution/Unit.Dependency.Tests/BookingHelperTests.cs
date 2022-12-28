using Moq;
using NUnit.Framework;
using UnitLibrary.ClassesWithDependencies.Repository;
using UnitLibrary.ClassesWithDependencies.Entities;
using System.Collections.Generic;
using System.Linq;
using System;
using UnitLibrary.ClassesWithDependencies;

namespace Unit.Dependency.Tests
{
    public class BookingHelperTests
    {
        private Mock<IBookingRepository> _bookingRepository;
        private Booking _presentBooking;

        [SetUp]
        public void SetUp()
        {
            _bookingRepository = new Mock<IBookingRepository>();
            _presentBooking = new Booking()
            {
                Id = 1,
                ArrivalDate = new DateTime(2022, 12, 21),
                DepartureDate = new DateTime(2022, 12, 22),
                Reference = "Sundarban",
                Status = "Not Cancelled"
            };
        }

        private List<Booking> GetAllBookingsNotCancelled()
        {
            return new List<Booking>()
            {
                new Booking()
                {
                    Id = 2,
                    ArrivalDate = new DateTime(2022, 12, 20),
                    DepartureDate = new DateTime(2022, 12, 22),
                    Reference = "Bandarban",
                    Status = "Not Cancelled"
                },
                new Booking()
                {
                    Id = 3,
                    ArrivalDate = new DateTime(2023, 12, 20),
                    DepartureDate = new DateTime(2023, 12, 22),
                    Reference = "Japan",
                    Status = "Not Cancelled"
                }
            };
        }

        [Test]
        public void OverlappingBookingsExist_WhereNoOverLappingBookingOccurs_ReturnEmptyString()
        {
            List<Booking> bookings = new List<Booking>()
            { 
                new Booking()
                {
                    Id = 4,
                    ArrivalDate = new DateTime(2023, 12, 21),
                    DepartureDate = new DateTime(2024, 12, 22),
                    Reference = "UK",
                    Status = "Not Cancelled"
                }
            };

            _bookingRepository.Setup(repository => repository.GetActiveBookings(It.IsAny<int>())).Returns(bookings.AsQueryable());

            var result = BookingHelper.OverlappingBookingsExist(_presentBooking, _bookingRepository.Object);

            Assert.AreEqual(string.Empty, result);
        }

        [Test]
        public void OverlappingBookingsExist_WhereExistingBookingIsFound_ReturnsNonEmptyString()
        {
            List<Booking> bookings = GetAllBookingsNotCancelled();
            _bookingRepository.Setup(repository => repository.GetActiveBookings(It.IsAny<int>())).Returns(bookings.AsQueryable());

            var result = BookingHelper.OverlappingBookingsExist(_presentBooking, _bookingRepository.Object);

            Assert.AreNotEqual(string.Empty, result);
        }

        [Test]
        public void OverlappingBookingsExist_WhereSameBookinExists_ReturnsReference()
        {
            List<Booking> bookings = GetAllBookingsNotCancelled();
            _bookingRepository.Setup(repository => repository.GetActiveBookings(It.IsAny<int>())).Returns(bookings.AsQueryable());

            var result = BookingHelper.OverlappingBookingsExist(new Booking()
                        {
                            Id = 2,
                            ArrivalDate = new DateTime(2022, 12, 20),
                            DepartureDate = new DateTime(2022, 12, 22),
                            Reference = "Bandarban",
                            Status = "Not Cancelled"
                        }, _bookingRepository.Object
            );

            Assert.AreEqual("Bandarban", result);
        }

        [Test]
        public void OverlappingBookingsExist_WherBookingIsCalcelled_ReturnEmptyString()
        {
            List<Booking> bookings = GetAllBookingsNotCancelled();
            IQueryable<Booking> queryableBookings = bookings.AsQueryable();
            _bookingRepository.Setup(repository => repository.GetActiveBookings(It.IsAny<int>())).Returns(bookings.AsQueryable());

            _presentBooking.Status = "Cancelled";
            var result = BookingHelper.OverlappingBookingsExist(_presentBooking, _bookingRepository.Object);

            Assert.AreEqual(string.Empty, result);
        }
    }
}