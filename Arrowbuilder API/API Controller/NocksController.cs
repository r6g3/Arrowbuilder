using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arrowbuilder.Models;
using ORM.Services;

namespace Arrowbuilder_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NocksController : ControllerBase
{
    private readonly DbManager _context;

    public NocksController(DbManager context)
    {
        _context = context;
    }

    // GET: api/Nocks
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Nock>>> GetNocks()
    {
        return await _context.Nocks.ToListAsync();
    }

    // GET: api/Nocks/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Nock>> GetNock(int id)
    {
        var nock = await _context.Nocks.FindAsync(id);

        if (nock == null)
        {
            return NotFound();
        }

        return nock;
    }

    // GET: api/Nocks/type/Lighted
    [HttpGet("type/{type}")]
    public async Task<ActionResult<IEnumerable<Nock>>> GetNocksByType(NockType type)
    {
        return await _context.Nocks
            .Where(n => n.Type == type)
            .ToListAsync();
    }

    // POST: api/Nocks
    [HttpPost]
    public async Task<ActionResult<Nock>> CreateNock(Nock nock)
    {
        _context.Nocks.Add(nock);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetNock), new { id = nock.Id }, nock);
    }

    // PUT: api/Nocks/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateNock(int id, Nock nock)
    {
        if (id != nock.Id)
        {
            return BadRequest();
        }

        _context.Entry(nock).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await NockExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Nocks/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteNock(int id)
    {
        var nock = await _context.Nocks.FindAsync(id);
        if (nock == null)
        {
            return NotFound();
        }

        _context.Nocks.Remove(nock);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> NockExists(int id)
    {
        return await _context.Nocks.AnyAsync(e => e.Id == id);
    }
}