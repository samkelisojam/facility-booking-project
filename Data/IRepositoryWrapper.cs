namespace University_of_free_state_booking_Facilities.Data
{
    public interface IRepositoryWrapper
    {
        IBookingRepository Booking { get; }
        IFacilityRepository Facility { get; }
        ITransactionRepository Transaction { get; }
        IReviewRepository Review { get; }
        INotificationRepository Notification { get; }
        void Save();
    }
}
