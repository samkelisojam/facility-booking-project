using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using University_of_free_state_booking_Facilities.Models;
using University_of_free_state_booking_Facilities.Models.ViewModels;

namespace University_of_free_state_booking_Facilities.Data
{
    public class FacilityRepository : RepositoryBase<Facility>, IFacilityRepository
    {
        public FacilityRepository(AppDbContext appDbContext) : base(appDbContext)
        {
            
        }

        public List<FacilityInfoViewModel> GetFacilityInfo()
        {


			var facilityInfoList = _appDbContext.Facilities
				 .Select(f => new FacilityInfoViewModel
				 {
					 FacilityName = f.Name,
					 TotalBookings = f.Bookings.Count,
					 TotalAmount = f.Bookings.Sum(b => b.TotalAmount)
				 })
				 .ToList();

			return facilityInfoList;
		}
	}
    
}
