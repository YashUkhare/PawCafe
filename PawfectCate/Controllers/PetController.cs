using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PawfectCate.Models;

[Route("api/[controller]")]
[ApiController]
public class PetController : ControllerBase
{
    private readonly PawfectCareContext _context;

    public PetController(PawfectCareContext context)
    {
        _context = context;
    }


    [HttpGet("pets/{customerId}")]
    public async Task<IActionResult> GetPetsByCustomer(int customerId)
    {
        var pets = await _context.Pets
            .Where(p => p.CustomerId == customerId)
            .Select(p => new PetDTO
            {
                PetId = p.PetId,
                Name = p.Name,
                Breed = p.Breed,
                Age = p.Age,
                CreatedAt = p.CreatedAt
            })
            .ToListAsync();

        if (pets == null || pets.Count == 0)
        {
            return NotFound(new { message = "No pets found for this customer." });
        }

        return Ok(pets);
    }


    [HttpPost("CreatePet")]
    public async Task<IActionResult> CreatePet([FromBody] PetDTO petDto)
    {
        if (petDto == null)
        {
            return BadRequest(new { message = "Invalid pet data." });
        }

        var pet = new Pet
        {
            CustomerId = petDto.CustomerId, // Use the CustomerId from the DTO
            Name = petDto.Name,
            Breed = petDto.Breed,
            Age = petDto.Age,
            CreatedAt = DateTime.UtcNow // Store the creation time
        };

        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();

        petDto.PetId = pet.PetId;
        petDto.CreatedAt = pet.CreatedAt; // Return created time

        return Ok(petDto);
    }



    [HttpPut("{petId}")]
    public async Task<IActionResult> UpdatePet(int petId, [FromBody] PetDTO petDto)
    {
        var pet = await _context.Pets.FindAsync(petId);
        if (pet == null)
        {
            return NotFound(new { message = "Pet not found." });
        }

        pet.Name = petDto.Name;
        pet.Breed = petDto.Breed;
        pet.Age = petDto.Age;
        // pet.CreatedAt should not be modified

        _context.Entry(pet).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(new { message = "Pet updated successfully!" });
    }


    [HttpDelete("{petId}")]
    public async Task<IActionResult> DeletePet(int petId)
    {
        var pet = await _context.Pets.FindAsync(petId);
        if (pet == null)
        {
            return NotFound(new { message = "Pet not found." });
        }

        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();

        return Ok(new { message = "Pet deleted successfully!" });
    }


    public class PetDTO
    {
        public int PetId { get; set; }
        public int CustomerId { get; set; } 
        public string Name { get; set; } = null!;
        public string Breed { get; set; } = null!;
        public int Age { get; set; }
        public DateTime CreatedAt { get; set; } =DateTime.Now;
    }

}
