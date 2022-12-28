using UnitLibrary.ClassesWithDependencies.Entities;

namespace UnitLibrary.ClassesWithDependencies.Repository
{
    public interface IBookingRepository
    {
        IQueryable<Booking> GetActiveBookings(int? excludedBookingId = null);
    }
}