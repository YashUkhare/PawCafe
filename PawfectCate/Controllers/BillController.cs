using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawfectCate.Models;

namespace PawfectCate.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class BillController : ControllerBase
    {
        private readonly PawfectCareContext _context;

        public BillController(PawfectCareContext context)
        {
            _context = context;
        }

        [HttpPost("processpayment")]
        public async Task<IActionResult> ProcessPayment([FromBody] PaymentDTO paymentDto)
        {
            if (paymentDto == null)
            {
                return BadRequest("Invalid payment data.");
            }

            var appointment = await _context.Appointments.FindAsync(paymentDto.AppointmentId);
            if (appointment == null)
            {
                return NotFound("Appointment not found.");
            }

            // Simulate payment processing (this could be integrated with a payment gateway)
            var Bill = new Bill
            {
                AppointmentId = paymentDto.AppointmentId,
                BillAmount = paymentDto.Amount,
                Cvvno = paymentDto.Cvvno,
                Cardno = paymentDto.Cardno,
                Status = "completed",
                PaymentDate=DateTime.Now
            };

            _context.Bills.Add(Bill);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Payment successful", paymentId = Bill.BillId });
        }


    }

    public class PaymentDTO
    {
        public int AppointmentId { get; set; }
        public decimal Amount { get; set; }
        public string Cardno { get; set; }
        public string Cvvno { get; set; }
        public string Status { get; set; }
    }
}
