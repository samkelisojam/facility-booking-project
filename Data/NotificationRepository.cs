using University_of_free_state_booking_Facilities.Models;

namespace University_of_free_state_booking_Facilities.Data
{
  
    public class NotificationRepository : RepositoryBase<Notification>, INotificationRepository
    {

        public NotificationRepository(AppDbContext appDbContext) : base(appDbContext)
        {
        }
    }
}
