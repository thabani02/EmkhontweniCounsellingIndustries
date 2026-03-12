using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmkhontweniCounselling.Models;

namespace EmkhontweniCounselling.Controllers
{
    public class AppointmentController : Controller
    {
        private readonly EmkhontweniCounsellingDbContext _context;

        public AppointmentController(EmkhontweniCounsellingDbContext context)
        {
            _context = context;
        }

        // ============================================
        // CENTRAL SERVICE + PRICING CATALOG
        // ============================================
        private readonly Dictionary<string, decimal> _services = new()
        {
            { "Student Session", 150 },
            { "Individual Session", 250 },
            { "Couples Session", 300 },
            { "Stress Management", 250 },
            { "Trauma Counselling", 250 },
            { "African Spirituality & Healing", 250 }
        };

        // ============================================
        // GET: Book Appointment
        // ============================================
        public IActionResult Book(string service)
        {
            // Validate service
            // If no service is provided or invalid, default to the first service
            if (string.IsNullOrEmpty(service) || !_services.ContainsKey(service))
            {
                service = _services.Keys.First(); // Default to "Student Session"
            }

            // Pass service + amount to view
            ViewBag.Service = service;
            ViewBag.Amount = _services[service];

            return View();
        }

        // ============================================
        // POST: Book Appointment
        // ============================================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Book(
            Appointment appointment,
            string FullName,
            string Email,
            string PhoneNumber)
        {
            // -------------------------------
            // SERVER-SIDE VALIDATION
            // -------------------------------
            if (string.IsNullOrWhiteSpace(FullName) ||
                string.IsNullOrWhiteSpace(Email) ||
                appointment.AppointmentDate == null ||
                string.IsNullOrWhiteSpace(appointment.ServiceType))
            {
                ModelState.AddModelError("", "Please complete all required fields.");
                ViewBag.Service = appointment.ServiceType;
                ViewBag.Amount = appointment.Amount;
                return View(appointment);
            }

            // -------------------------------
            // ENSURE SERVICE IS VALID
            // -------------------------------
            if (!_services.ContainsKey(appointment.ServiceType))
            {
                ModelState.AddModelError("", "Invalid service selected.");
                return RedirectToAction("Pricing", "Home");
            }

            // Enforce correct amount from server
            appointment.Amount = _services[appointment.ServiceType];

            // -------------------------------
            // FIND OR CREATE CLIENT
            // -------------------------------
            var client = await _context.Clients
                .FirstOrDefaultAsync(c => c.Email == Email);

            if (client == null)
            {
                client = new Client
                {
                    FullName = FullName,
                    Email = Email,
                    PhoneNumber = PhoneNumber
                };

                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }

            // -------------------------------
            // CREATE APPOINTMENT
            // -------------------------------
            appointment.ClientId = client.ClientId;
            appointment.Status = "Pending";
            appointment.IsPaymentVerified = false;

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            return RedirectToAction("Confirmation");
        }

        // ============================================
        // BOOKING CONFIRMATION
        // ============================================
        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
