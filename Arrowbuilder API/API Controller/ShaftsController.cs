using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arrowbuilder.Models;
using ORM.Services;

namespace Arrowbuilder_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ShaftsController : ControllerBase
{
    private readonly DbManager _context;

    public ShaftsController(DbManager context)
    {
        _context = context;
    }

    // GET: api/Shafts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Shaft>>> GetShafts()
    {
        return await _context.Shafts.ToListAsync();
    }

    // GET: api/Shafts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Shaft>> GetShaft(int id)
    {
        var shaft = await _context.Shafts.FindAsync(id);

        if (shaft == null)
        {
            return NotFound();
        }

        return shaft;
    }

    // GET: api/Shafts/spine/500
    [HttpGet("spine/{spine}")]
    public async Task<ActionResult<IEnumerable<Shaft>>> GetShaftsBySpine(int spine)
    {
        return await _context.Shafts
            .Where(s => s.Spine == spine)
            .ToListAsync();
    }

    // POST: api/Shafts
    [HttpPost]
    public async Task<ActionResult<Shaft>> CreateShaft(Shaft shaft)
    {
        _context.Shafts.Add(shaft);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetShaft), new { id = shaft.Id }, shaft);
    }

    // PUT: api/Shafts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShaft(int id, Shaft shaft)
    {
        if (id != shaft.Id)
        {
            return BadRequest();
        }

        _context.Entry(shaft).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ShaftExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Shafts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteShaft(int id)
    {
        var shaft = await _context.Shafts.FindAsync(id);
        if (shaft == null)
        {
            return NotFound();
        }

        _context.Shafts.Remove(shaft);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ShaftExists(int id)
    {
        return await _context.Shafts.AnyAsync(e => e.Id == id);
    }
}