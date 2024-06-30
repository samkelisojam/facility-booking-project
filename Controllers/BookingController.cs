using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using University_of_free_state_booking_Facilities.Data;
using University_of_free_state_booking_Facilities.Models;
using System.Drawing;
using System.Net;
using System.Net.Mail;


using University_of_free_state_booking_Facilities.Models.ViewModels;

using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace University_of_free_state_booking_Facilities.Controllers
{
    [Authorize(Roles = "Staff,Student,Visitor")]

    public class BookingController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IRepositoryWrapper _repo;
        private readonly IOptions<StripeSettings> _stripeSettings;
        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;
        int icurrentBooking;
        public string SessionId;
        string BookingName;
        string BookingID;
        public BookingController(IRepositoryWrapper Repo, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment, IOptions<StripeSettings> stripeSettings, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _repo = Repo;
            _hostingEnvironment = hostingEnvironment;
            _stripeSettings = stripeSettings;
            _userManager = userManager;
            _signInManager = signInManager;

        }
      
        public IActionResult Index()
        {
            var currentDate = DateTime.Now.Date;
            var upcomingBookings = _repo.Booking.FindAll()
                .Where(p => p.BookingDate >= currentDate)
                .OrderBy(p => p.StartTime)
                .ToList();

            var model = new DisplayViewModel
            {
                Booking = upcomingBookings,
            };

            return View(model);
        }
		

		[HttpGet]
        public IActionResult AddRating()
        {
			
			return View();
        }
        [HttpPost]
        public IActionResult AddRating(FeedBack feedback)
        {
            
            if (ModelState.IsValid)
            {
                
                _repo.Review.Create(feedback);
                _repo.Save(); 
                return RedirectToAction("Mybooking","Users"); 
            }
           
            return View(feedback);
        }
		

		[HttpGet]
        public IActionResult EditBooking(int id)
        {
          
            var booking = _repo.Booking.GetById(id);

            if (booking == null)
            {
              
                return NotFound();
            }

          
            PopulateDLL(booking.BookingId);

            return View("EditBooking", booking); 
        }

  
        [HttpPost]
        public IActionResult EditBooking(Booking booking)
        {
            if (ModelState.IsValid)
            {
              
                _repo.Booking.Update(booking);
                _repo.Save();

               
                return RedirectToAction("Mybooking", "Users"); 
            }

     
            PopulateDLL(booking.BookingId);
            return View("EditBooking", booking);
        }

   

        [HttpGet]
        public IActionResult AddBooking()
        {
            if(User.Identity.IsAuthenticated){
                ViewBag.username = User.Identity.Name;
                PopulateDLL();
                return View();
            }
            else
            {
                return RedirectToAction("SignIn","Account");
            }

        
        }


        [HttpPost]

        public IActionResult AddBooking(Booking booking)
        {
            if (ModelState.IsValid)
            {
                
                TimeSpan startTime = TimeSpan.Parse(booking.StartTime);
                TimeSpan endTime = TimeSpan.Parse(booking.EndTime);
                double durationInHours = (endTime - startTime).TotalHours;

             
                decimal ratePerHour = 10; 
                booking.TotalAmount =Math.Abs((decimal)((decimal)durationInHours * ratePerHour));

              BookingName =booking.FacilityId.ToString();
                booking.PaymentStatus = "";
                booking.UserName = User.Identity.Name;
                icurrentBooking = booking.BookingId;


                _repo.Booking.Create(booking);
                _repo.Save();

              ViewBag.AmountBooking = booking.TotalAmount; 
                ViewBag.bookingdetail = +booking.FacilityId +"\t"+booking.StartTime+"\t"+booking.EndTime+"\t"+booking.TotalAmount+"\t"+booking.BookingDate;

                return RedirectToAction("Payment", new { AmountBooking = ViewBag.AmountBooking, BookingDetail = ViewBag.bookingdetail });
            }

            PopulateDLL(booking.BookingId);
            return View(booking);
        }




        [HttpGet]

        private void PopulateDLL(object selectedDegree = null)
        {
            @ViewBag.Cat = new SelectList(_repo.Facility.FindAll()
                , "FacilityId", "Name", selectedDegree
                ); ;
        }




        public ActionResult Payment(decimal AmountBooking, string BookingDetail)
        {
            string[] st = BookingDetail.Split('\t');
            int i = int.Parse(st[0]);
            var facility = _repo.Facility.GetById(i);
            ViewBag.Amount = AmountBooking;
            ViewBag.BookingDetail = facility.Name.ToString()+"\t"+BookingDetail;

            return View();
        }
        [HttpGet]
        public IActionResult PaymentConfirmation()
        {


            return View("PaymentConfirmation");
        }

        [HttpGet]
        public IActionResult CashPayment()
        {
       
           
            return View("PaymentConfirmation");
        }

        [HttpPost]
        public ActionResult CardPayment(CardViewModel model, string booking)
        {
           
            string[] bbokingArr = booking.Split('\t');
            Transaction trans = new Transaction();
            trans.TransactionDate = DateTime.Now;
            trans.PaymentAmount = decimal.Parse(bbokingArr[4]);
            trans.TransactionDetail = " Paid Online for Booking:" + bbokingArr[1]+"Total amount of:" + booking[3]+"by:"+User.Identity.Name;
           

		
     
			_repo.Transaction.Create(trans);

			
			_repo.Save();
            var facility = _repo.Facility.GetById(int.Parse(bbokingArr[1]));

            Notification notif = new Notification();
            notif.Title = "Approved Booking Facility";
            notif.Username = User.Identity.Name;

          
         
            notif.Description = "your booking has been approved to Take place as follow :\n" + bbokingArr[5] + "\n" +
                "facility Name is: " + bbokingArr[0] + "\t will start at: " + bbokingArr[1] + " to " + bbokingArr[2] + "\n Thank you for booking with us enjoy our facility" +
                "\n\nufs facility online+\n" ;
            _repo.Notification.Create(notif);
            ViewBag.BookingRecord = bbokingArr[0]+"\t\t"+trans.PaymentAmount.ToString("C");
            
          //  return View(Session.Url);
             return RedirectToAction("Confirmation", new { BookingRecord = ViewBag.BookingRecord });
      
        }


        [HttpGet]

        public IActionResult CancelBooking()
        {
            
            return View("Home", "Index");
        }

        public IActionResult Confirmation(IFormCollection formCollection,string bookingRecord)
        {

            var qrCodeString = bookingRecord;

            var writer = new ZXing.QrCode.QRCodeWriter();
            var resultBit = writer.encode(qrCodeString, ZXing.BarcodeFormat.QR_CODE, 200, 200);



            var matrix = resultBit;


            int scale = 2;

            Bitmap result = new Bitmap(matrix.Width * scale, matrix.Height * scale);
            for (int x = 0; x < matrix.Height; x++)
                for (int y = 0; y < matrix.Width; y++)
                {
                    Color pixel = matrix[x, y] ? Color.Black : Color.White;
                    for (int i = 0; i < scale; i++)
                        for (int j = 0; j < scale; j++)
                            result.SetPixel(x * scale + i, y * scale + j, pixel);
                }


            string webRootPath = _hostingEnvironment.WebRootPath;

            var random = new Random();
            var randomNumber = random.Next(1000, 9999); 

      
            var imageName = $"QrcodeNew_{randomNumber}.png";


            result.Save(Path.Combine(webRootPath, "Images", imageName));

            ViewBag.URL = $"\\Images\\{imageName}"; 

            string imagePath = Path.Combine(webRootPath, "Images", imageName);


           



            ViewBag.Message = "You can save your QRCode u will use it in entrence";

            
        

            return View();

        }
      



    }
}
