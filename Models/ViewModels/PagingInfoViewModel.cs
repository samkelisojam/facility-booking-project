namespace University_of_free_state_booking_Facilities.Models.ViewModels
{
    public class PagingInfoViewModel
    {
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages =>
           (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage);
    }
}
