using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using PawfectCate.Models;

namespace PawfectCate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly PawfectCareContext _context;

        public CustomerController(PawfectCareContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterCustomerRequest registerRequest)
        {
            if (ModelState.IsValid)
            {
                var customer = registerRequest.ToCustomer();
                customer.Password = registerRequest.Password;
                _context.Customers.Add(customer);
                _context.SaveChanges();

                return Ok(new { Message = "Customer registered successfully" });
            }
            return BadRequest("Invalid data");
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Ok(customer);
        }

        // Update customer profile
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCustomer(int id, [FromBody] Customer updatedCustomer)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            customer.Name = updatedCustomer.Name;
            customer.Email = updatedCustomer.Email;
            customer.Phone = updatedCustomer.Phone;

            // Only update password if it's provided
            if (!string.IsNullOrWhiteSpace(updatedCustomer.Password))
            {
                customer.Password = updatedCustomer.Password;
                    
            }

            _context.Entry(customer).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully!" });
        }

        [HttpGet("all")]
        public IActionResult GetAllCustomers()
        {
            var customers = _context.Customers
                .Select(c => new
                {
                    c.CustomerId,
                    c.Name,
                    c.Email,
                    c.Phone,
                    c.Role,
                    c.CreatedAt
                })
                .ToList();

            return Ok(customers);
        }
    }



    public class RegisterCustomerRequest
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string? Phone { get; set; }

        public Customer ToCustomer()
        {
            return new Customer
            {
                Name = Name,
                Email = Email,
                Password = Password,  
                Role = "customer",    
                CreatedAt = DateTime.Now,
                Phone = Phone
            };
        }
    }


}
