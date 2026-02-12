using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmkhontweniCounselling.Models;

namespace EmkhontweniCounselling.Controllers
{
    public class AdminController : Controller
    {
        private readonly EmkhontweniCounsellingDbContext _context;

        public AdminController(EmkhontweniCounsellingDbContext context)
        {
            _context = context;
        }

        // ===============================
        // LOGIN
        // ===============================
        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            if (username == "Princess" && password == "123456")
            {
                HttpContext.Session.SetString("Admin", "LoggedIn");
                return RedirectToAction("Dashboard");
            }

            ViewBag.Error = "Invalid credentials";
            return View();
        }

        // ===============================
        // DASHBOARD
        // ===============================
        public async Task<IActionResult> Dashboard()
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");

            var appointments = await _context.Appointments
                .Include(a => a.Client)
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            return View(appointments);
        }

        // ===============================
        // VERIFY PAYMENT
        // ===============================
        public async Task<IActionResult> TogglePayment(int id)
        {
            var appt = await _context.Appointments.FindAsync(id);
            appt.IsPaymentVerified = !appt.IsPaymentVerified;
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        // ===============================
        // ACCEPT / REJECT
        // ===============================
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var appt = await _context.Appointments.FindAsync(id);
            if (!appt.IsPaymentVerified) return RedirectToAction("Dashboard");

            appt.Status = status;
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }

        private bool IsLoggedIn()
            => HttpContext.Session.GetString("Admin") != null;
    }
}
