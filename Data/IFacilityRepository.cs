using University_of_free_state_booking_Facilities.Models;
using University_of_free_state_booking_Facilities.Models.ViewModels;

namespace University_of_free_state_booking_Facilities.Data
{
    public interface IFacilityRepository:IRepositoryBase<Facility>
    {
		List<FacilityInfoViewModel> GetFacilityInfo();
    }
}
