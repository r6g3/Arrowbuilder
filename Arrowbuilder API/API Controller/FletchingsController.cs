using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arrowbuilder.Models;
using ORM.Services;

namespace Arrowbuilder_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FletchingsController : ControllerBase
{
    private readonly DbManager _context;

    public FletchingsController(DbManager context)
    {
        _context = context;
    }

    // GET: api/Fletchings
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Fletching>>> GetFletchings()
    {
        return await _context.Fletchings.ToListAsync();
    }

    // GET: api/Fletchings/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Fletching>> GetFletching(int id)
    {
        var fletching = await _context.Fletchings.FindAsync(id);

        if (fletching == null)
        {
            return NotFound();
        }

        return fletching;
    }

    // GET: api/Fletchings/search?manufacturer=Easton
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Fletching>>> SearchFletchings(
        [FromQuery] string? manufacturer,
        [FromQuery] MaterialType? materialType)
    {
        var query = _context.Fletchings.AsQueryable();

        if (!string.IsNullOrEmpty(manufacturer))
        {
            query = query.Where(f => f.Manufacturer.Contains(manufacturer));
        }

        if (materialType.HasValue)
        {
            query = query.Where(f => f.MaterialType == materialType.Value);
        }

        return await query.ToListAsync();
    }

    // POST: api/Fletchings
    [HttpPost]
    public async Task<ActionResult<Fletching>> CreateFletching(Fletching fletching)
    {
        _context.Fletchings.Add(fletching);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetFletching), new { id = fletching.Id }, fletching);
    }

    // PUT: api/Fletchings/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateFletching(int id, Fletching fletching)
    {
        if (id != fletching.Id)
        {
            return BadRequest();
        }

        _context.Entry(fletching).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await FletchingExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Fletchings/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFletching(int id)
    {
        var fletching = await _context.Fletchings.FindAsync(id);
        if (fletching == null)
        {
            return NotFound();
        }

        _context.Fletchings.Remove(fletching);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> FletchingExists(int id)
    {
        return await _context.Fletchings.AnyAsync(e => e.Id == id);
    }
}