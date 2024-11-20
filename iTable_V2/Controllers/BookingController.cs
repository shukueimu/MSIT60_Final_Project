using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using iTable_V2.Models;
using System.Diagnostics;
using System.Globalization;

namespace iTable_V2.Controllers
{
    public class BookingController : Controller
    {
        private readonly ITableDbContext _context;

        public BookingController(ITableDbContext context)
        {
            _context = context;
        }
        //
        // GET 方法: 用於載入訂位頁面並顯示資料+UserID
        public async Task<IActionResult> BookingPage(int? restaurantID)
        {
            if (!restaurantID.HasValue)
            {
                return NotFound(); // 如果 restaurantID 是 null，返回 404
            }

            // 使用 GetInt32 讀取 UserID
            var userID = HttpContext.Session.GetInt32("UserID");

            // 從資料庫讀取相關資料並傳遞給視圖
            var viewModel = await GetBookingPageViewModel(restaurantID.Value);

            // 將 UserID 傳遞給視圖模型
            viewModel.UserID = userID;

            return View(viewModel);
        }


        //
        // GET 方法: 用於載入訂位頁面並顯示資料
        //public async Task<IActionResult> BookingPage(int? restaurantID)
        //{
        //    if (!restaurantID.HasValue)
        //    {
        //        return NotFound(); // 如果 restaurantID 是 null，返回 404
        //    }

        //    // 從資料庫讀取相關資料並傳遞給視圖
        //    var viewModel = await GetBookingPageViewModel(restaurantID.Value);
        //    return View(viewModel);
        //}

        [HttpPost]
        public async Task<IActionResult> BookingPage(Reservation reservation)
        {
            if (ModelState.IsValid)
            {
                // 計算總預訂人數
                var totalGuests = (reservation.NumAdults ?? 0) + (reservation.NumChildren ?? 0);

                // 查詢該餐廳的可用座位數
                var availability = await _context.RestaurantAvailabilities
                    .FirstOrDefaultAsync(ra => ra.RestaurantID == reservation.RestaurantID);

                // 檢查 availability 和 AvailableSeats
                if (availability == null || availability.AvailableSeats == null)
                {
                    ModelState.AddModelError("", "無法找到餐廳的座位資訊");
                    return View(await GetBookingPageViewModel(reservation.RestaurantID));
                }

                // 檢查可用座位數是否足夠
                if (availability.AvailableSeats < totalGuests)
                {
                    ModelState.AddModelError("", "超出目前可用座位數量，請重新選擇人數或時段");
                    return View(await GetBookingPageViewModel(reservation.RestaurantID));
                }

                // 扣除可用座位數
                availability.AvailableSeats -= totalGuests;
                await _context.SaveChangesAsync(); // 儲存座位數更新

                // 儲存訂單資料（只有通過檢查才會執行）
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                // 傳遞必要的訂單資訊到 BookingSuccess 頁面
                TempData["ReservationDetails"] = System.Text.Json.JsonSerializer.Serialize(new Dictionary<string, object>
                {
                    { "RestaurantName", await _context.Restaurants.Where(r => r.RestaurantID == reservation.RestaurantID).Select(r => r.Name).FirstOrDefaultAsync() ?? "未知餐廳" },
                    { "Date", reservation.ReservationDate?.ToString("yyyy/MM/dd") ?? "未指定日期" },
                    { "Time", reservation.ReservationTime?.ToString() ?? "未指定時間" },
                    { "NumAdults", reservation.NumAdults ?? 0 },
                    { "NumChildren", reservation.NumChildren ?? 0 },
                    { "BookerName", reservation.BookerName ?? "未提供姓名" },
                    { "BookerPhone", reservation.BookerPhone ?? "未提供電話" },
                    { "BookerEmail", reservation.BookerEmail ?? "未提供Email" },
                    { "SpecialRequests", reservation.SpecialRequests ?? "無備註" }
                });



                // 跳轉到成功頁面
                return RedirectToAction("BookingSuccess");
            }

            // 如果資料驗證失敗，重新加載頁面
            return View(await GetBookingPageViewModel(reservation.RestaurantID));
        }



        public IActionResult BookingSuccess()
        {
            var reservationDetailsJson = TempData["ReservationDetails"] as string;
            if (reservationDetailsJson == null)
            {
                return RedirectToAction("BookingPage");
            }

            // 反序列化 TempData 中的訂位詳細資訊
            var reservationDetails = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(reservationDetailsJson);

            // 取得日期，並格式化為包含星期幾的字串
            if (reservationDetails!.ContainsKey("Date") && DateTime.TryParse(reservationDetails["Date"].ToString(), out var reservationDate))
            {
                var formattedDate = reservationDate.ToString("yyyy-MM-dd (ddd)", new CultureInfo("zh-TW")); // 格式化為 "2024-11-16 (週六)"
                reservationDetails["Date"] = formattedDate;
            }

            return View(reservationDetails);
        }



        // 輔助方法：用於載入 ViewModel 資料
        private async Task<BookingPageViewModel> GetBookingPageViewModel(int restaurantID)
        {
            // 從資料庫取得資料並填充 ViewModel
            var reservations = await _context.Reservations.ToListAsync();
            var settings = await _context.ReservationControlSettings.ToListAsync();
            var restaurantAvailabilities = await _context.RestaurantAvailabilities.ToListAsync();
            var restaurantBusinessHours = await _context.RestaurantBusinessHours.ToListAsync();
            var restaurantUsers = await _context.RestaurantUsers.ToListAsync();
            var reviews = await _context.Reviews.ToListAsync();
            var users = await _context.Users.ToListAsync();
            var photos = await _context.Photos.ToListAsync();
            var announcements = await _context.Announcements.ToListAsync();
            var favorites = await _context.Favorites.ToListAsync();
            var passwordResetRequests = await _context.PasswordResetRequests.ToListAsync();

            var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.RestaurantID == restaurantID);
            var restaurants = restaurantID > 0
                ? await _context.Restaurants.Where(r => r.RestaurantID == restaurantID).ToListAsync()
                : new List<Restaurant>();

            var viewModel = new BookingPageViewModel
            {
                Reservations = reservations,
                ReservationControlSettings = settings,
                Restaurants = restaurants,
                RestaurantAvailabilities = restaurantAvailabilities,
                RestaurantBusinessHours = restaurantBusinessHours,
                RestaurantUsers = restaurantUsers,
                Reviews = reviews,
                Users = users,
                Photos = photos,
                Announcements = announcements,
                Favorites = favorites,
                PasswordResetRequests = passwordResetRequests
            };

            if (restaurant != null)
            {
                ViewData["RestaurantName"] = restaurant.Name;
                ViewData["BusinessHoursStart"] = restaurant.BusinessHoursStart.ToString("HH:mm");
                ViewData["LastCheckInTime"] = restaurant.LastCheckInTime?.ToString("HH:mm");
            }
            else
            {
                ViewData["RestaurantName"] = "無法找到餐廳";
                ViewData["BusinessHoursStart"] = "09:30";
                ViewData["LastCheckInTime"] = "23:30";
            }

            return viewModel;
        }



    }
}


