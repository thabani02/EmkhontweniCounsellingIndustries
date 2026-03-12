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
        public IActionResult Index()
        {
            return RedirectToAction("Login");
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
        // DASHBOARD WITH SEARCH & FILTER
        // ===============================
        public async Task<IActionResult> Dashboard(
            int? year,
            int? month,
            string? search)
        {
            if (!IsLoggedIn())
                return RedirectToAction("Login");

            var query = _context.Appointments
                .Include(a => a.Client)
                .AsQueryable();

            // SEARCH (Client Name or Email)
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(a =>
                    a.Client.FullName.Contains(search) ||
                    a.Client.Email.Contains(search));
            }

            // YEAR FILTER
            if (year.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentDate.HasValue &&
                    a.AppointmentDate.Value.Year == year.Value);
            }

            // MONTH FILTER
            if (month.HasValue)
            {
                query = query.Where(a =>
                    a.AppointmentDate.HasValue &&
                    a.AppointmentDate.Value.Month == month.Value);
            }

            var appointments = await query
                .OrderByDescending(a => a.AppointmentDate)
                .ToListAsync();

            // Populate Year Folder List
            ViewBag.Years = await _context.Appointments
                .Where(a => a.AppointmentDate.HasValue)
                .Select(a => a.AppointmentDate!.Value.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToListAsync();

            ViewBag.SelectedYear = year;
            ViewBag.SelectedMonth = month;
            ViewBag.Search = search;

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
            if (!appt.IsPaymentVerified)
                return RedirectToAction("Dashboard");

            appt.Status = status;
            await _context.SaveChangesAsync();
            return RedirectToAction("Dashboard");
        }
        // ===============================
        // GET: RESCHEDULE
        // ===============================
        public async Task<IActionResult> Reschedule(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");

            var appointment = await _context.Appointments
                .Include(a => a.Client)
                .FirstOrDefaultAsync(a => a.AppointmentId == id);

            if (appointment == null) return NotFound();

            return View(appointment);
        }

        // ===============================
        // POST: RESCHEDULE
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reschedule(int id, DateTime newDateTime)
        {
            var appointment = await _context.Appointments.FindAsync(id);
            if (appointment == null) return NotFound();

            appointment.AppointmentDate = newDateTime;
            await _context.SaveChangesAsync();

            return RedirectToAction("Dashboard");
        }

        private bool IsLoggedIn()
            => HttpContext.Session.GetString("Admin") != null;
    

    // ===============================
// DELETE CLIENT (CONFIRM PAGE)
// ===============================
public async Task<IActionResult> DeleteClient(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");

            var client = await _context.Clients
                .Include(c => c.Appointments)
                .FirstOrDefaultAsync(c => c.ClientId == id);

            if (client == null) return NotFound();

            return View(client);
        }


        // ===============================
        // DELETE APPOINTMENT (ONE AT A TIME)
        // ===============================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAppointment(int id)
        {
            if (!IsLoggedIn()) return RedirectToAction("Login");

            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment != null)
            {
                _context.Appointments.Remove(appointment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Dashboard");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear(); // clear admin session
            return RedirectToAction("Welcome", "Home");
        }
    }

}
