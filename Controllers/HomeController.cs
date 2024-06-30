using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using University_of_free_state_booking_Facilities.Data;
using University_of_free_state_booking_Facilities.Models;

namespace University_of_free_state_booking_Facilities.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRepositoryWrapper _repo;
        public HomeController(IRepositoryWrapper Repo)
        {
            _repo = Repo;
        }
        public IActionResult Index()
        {
            return View(_repo.Facility.FindAll().Take(3));
        }

        public IActionResult AboutUs()
        {
            return View();
        }


    }
}