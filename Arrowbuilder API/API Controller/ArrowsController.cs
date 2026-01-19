using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arrowbuilder.Models;
using ORM.Services;

namespace Arrowbuilder_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArrowsController : ControllerBase
{
    private readonly DbManager _context;

    public ArrowsController(DbManager context)
    {
        _context = context;
    }

    // GET: api/Arrows
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Arrow>>> GetArrows()
    {
        return await _context.Arrows
            .Include(a => a.Shaft)
            .Include(a => a.Fletching)
            .Include(a => a.Nock)
            .Include(a => a.Insert)
            .Include(a => a.Point)
            .ToListAsync();
    }

    // GET: api/Arrows/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Arrow>> GetArrow(int id)
    {
        var arrow = await _context.Arrows
            .Include(a => a.Shaft)
            .Include(a => a.Fletching)
            .Include(a => a.Nock)
            .Include(a => a.Insert)
            .Include(a => a.Point)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (arrow == null)
        {
            return NotFound();
        }

        return arrow;
    }
    // GET: api/Arrows/user/5
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Arrow>>> GetArrowsByUser(int userId)
    {
        // ACHTUNG: Arrow benötigt eine UserId-Eigenschaft!
        // Beispiel: .Where(a => a.UserId == userId)
        // Wenn Arrow keine UserId hat, muss sie hinzugefügt werden:
        // public int UserId { get; set; }
        return await _context.Arrows
            //.Where(a => a.UserId == userId) // Nur aktivieren, wenn UserId existiert!
            .Include(a => a.Shaft)
            .Include(a => a.Fletching)
            .Include(a => a.Nock)
            .Include(a => a.Insert)
            .Include(a => a.Point)
            .ToListAsync();
    }

    // POST: api/Arrows
    [HttpPost]
    public async Task<ActionResult<Arrow>> CreateArrow(Arrow arrow)
    {
        _context.Arrows.Add(arrow);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetArrow), new { id = arrow.Id }, arrow);
    }

    // PUT: api/Arrows/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateArrow(int id, Arrow arrow)
    {
        if (id != arrow.Id)
        {
            return BadRequest();
        }

        _context.Entry(arrow).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ArrowExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Arrows/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteArrow(int id)
    {
        var arrow = await _context.Arrows.FindAsync(id);
        if (arrow == null)
        {
            return NotFound();
        }

        _context.Arrows.Remove(arrow);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> ArrowExists(int id)
    {
        return await _context.Arrows.AnyAsync(e => e.Id == id);
    }
}