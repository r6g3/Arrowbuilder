using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Arrowbuilder.Models;
using ORM.Services;

namespace Arrowbuilder_API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InsertsController : ControllerBase
{
    private readonly DbManager _context;

    public InsertsController(DbManager context)
    {
        _context = context;
    }

    // GET: api/Inserts
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Insert>>> GetInserts()
    {
        return await _context.Inserts.ToListAsync();
    }

    // GET: api/Inserts/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Insert>> GetInsert(int id)
    {
        var insert = await _context.Inserts.FindAsync(id);

        if (insert == null)
        {
            return NotFound();
        }

        return insert;
    }

    // POST: api/Inserts
    [HttpPost]
    public async Task<ActionResult<Insert>> CreateInsert(Insert insert)
    {
        _context.Inserts.Add(insert);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetInsert), new { id = insert.Id }, insert);
    }

    // PUT: api/Inserts/5
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInsert(int id, Insert insert)
    {
        if (id != insert.Id)
        {
            return BadRequest();
        }

        _context.Entry(insert).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await InsertExists(id))
            {
                return NotFound();
            }
            throw;
        }

        return NoContent();
    }

    // DELETE: api/Inserts/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInsert(int id)
    {
        var insert = await _context.Inserts.FindAsync(id);
        if (insert == null)
        {
            return NotFound();
        }

        _context.Inserts.Remove(insert);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    private async Task<bool> InsertExists(int id)
    {
        return await _context.Inserts.AnyAsync(e => e.Id == id);
    }
}