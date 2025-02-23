using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawfectCate.Models;

namespace PawfectCate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly PawfectCareContext _context;

        public AppointmentController(PawfectCareContext context)
        {
            _context = context;
        }

        [HttpGet("viewslots")]
        public IActionResult CheckSlots([FromQuery] DateOnly appointmentDate, [FromQuery] string appointmentTime)
        {
            // Count the number of appointments for the given date and time
            int appointmentCount = _context.Appointments
                .Where(a => a.AppointmentDate.Equals(appointmentDate) && a.AppointmentTime.Equals( appointmentTime))
                .Count();

            int availableSlots = 3 - appointmentCount;

            if (availableSlots > 0)
            {
                return Ok(new { message = "Slots available", availableSlots = availableSlots });
            }
            else
            {
                return Ok(new { message = "No slots available" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> BookAppointment([FromBody] AppointmentDTO appointmentDto)
        {
            if (appointmentDto == null)
            {
                return BadRequest("Invalid appointment data.");
            }

            // Create a new appointment
            var appointment = new Appointment
            {
                CustomerId = appointmentDto.CustomerId,
                PetId = appointmentDto.PetId,
                AppointmentDate = appointmentDto.AppointmentDate,
                AppointmentTime = appointmentDto.TimeSlot,
                ServiceId = appointmentDto.ServiceId
            };

            // Add appointment to the database
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            // Return the appointmentId in the response
            return Ok(new { appointmentId = appointment.AppointmentId });
        }

        public class AppointmentDTO
        {
            public int CustomerId { get; set; }
            public int PetId { get; set; }
            public DateOnly AppointmentDate { get; set; }
            public string? TimeSlot { get; set; }
            public int ServiceId { get; set; }
        }

        [HttpGet("upcoming")]
        public async Task<IActionResult> GetUpcomingAppointments()
        {
            
                var appointments = await _context.Appointments
                    .Include(a => a.Customer)
                    .Include(a => a.Pet)
                    .Include(a => a.Service)
                    .Select(a => new
                    {
                        a.AppointmentId,
                        Date = a.AppointmentDate.ToString("yyyy-MM-dd"), // Format date properly
                        CustomerName = a.Customer.Name,
                        PetName = a.Pet.Name,
                        ServiceName = a.Service.Name
                    })
                    .ToListAsync();
            return Ok(appointments);

            }
        [HttpGet("customer/{customerId}")]
        public async Task<IActionResult> GetAppointmentsByCustomerId(int customerId)
        {
            var appointments = await _context.Appointments
                .Where(a => a.CustomerId == customerId)
                .Include(a => a.Customer)
                .Include(a => a.Pet)
                .Include(a => a.Service)
                .Select(a => new
                {
                    a.AppointmentId,
                    a.AppointmentDate,
                    a.AppointmentTime,
                    a.Service.Name,
                    PetName = a.Pet.Name,
                    CustomerName = a.Customer.Name
                })
                .ToListAsync();

            if (appointments == null || !appointments.Any())
            {
                return NotFound("No appointments found for this customer.");
            }

            return Ok(appointments);
        }

        // Cancel an appointment
        [HttpDelete("cancel/{appointmentId}")]
        public async Task<IActionResult> CancelAppointment(int appointmentId)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);

            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok("Appointment canceled successfully.");
        }

    }
}
