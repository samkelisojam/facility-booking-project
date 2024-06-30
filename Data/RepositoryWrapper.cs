using Stripe;
using System.Collections.Generic;

namespace University_of_free_state_booking_Facilities.Data
{
    public class RepositoryWrapper:IRepositoryWrapper
    {
        private readonly AppDbContext _appDbContext;
        private IBookingRepository _Booking;
        private IFacilityRepository _Facility;
        private ITransactionRepository _Transaction;
        private IReviewRepository _Review;
        private INotificationRepository _Notification;
        public RepositoryWrapper(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public IBookingRepository Booking
        {
            get
            {
                if (_Booking == null)
                {
                    _Booking = new BookingRepository(_appDbContext);
                }

                return _Booking;
            }
        }
        public ITransactionRepository Transaction
        {
            get
            {
                if (_Transaction == null)
                {
                    _Transaction = new TransactionRepository(_appDbContext);
                }

                return _Transaction;
            }
        }
    
        public INotificationRepository Notification
        {
            get
            {
                if (_Notification == null)
                {
                    _Notification = new NotificationRepository(_appDbContext);
                }

                return _Notification;
            }
        }
        public IFacilityRepository Facility
        {
            get
            {
                if (_Facility == null)
                {
                    _Facility = new FacilityRepository(_appDbContext);
                }

                return _Facility;
            }
        }

        public IReviewRepository Review
        {
            get
            {
                if (_Review == null)
                {
                    _Review = new ReviewRepository(_appDbContext);
                }

                return _Review;
            }
        }

        public void Save()
        {
            _appDbContext.SaveChanges();
        }
    }
}
