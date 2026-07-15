
using hello_world.Data;
using hello_world.DTOs;
using hello_world.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PersonController : Controller
{
    private readonly ApplicationDbContext _context;

    public PersonController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: PERSONENTITYS
    public async Task<IActionResult> Index()    
    {
        return View(await _context.Persons.ToListAsync());
    }

    // GET: PERSONENTITYS/Details/5
    public async Task<IActionResult> Details(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var personentity = await _context.Persons
            .FirstOrDefaultAsync(m => m.Id == id);
        if (personentity == null)
        {
            return NotFound();
        }

        return View(personentity);
    }

    // GET: PERSONENTITYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: PERSONENTITYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePersonDto dto)
    {
        if (ModelState.IsValid)
        {
            var personEntity = new PersonEntity(
                dto.Code,
                dto.FirstName,
                dto.LastName,
                dto.Email,
                dto.PhoneNumber
            );
            _context.Add(personEntity);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    // GET: PERSONENTITYS/Edit/5
    public async Task<IActionResult> Edit(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var personentity = await _context.Persons.FindAsync(id);
        if (personentity == null)
        {
            return NotFound();
        }
        return View(personentity);
    }

    // POST: PERSONENTITYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(System.Guid? id, UpdatePersonDto dto)
    {
        if (id != dto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var person = await _context.Persons.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (person == null)
                {
                    throw new InvalidOperationException($"No exsite la persona");
                }
                person.UpdatePersonEntity(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber);
                _context.Update(person);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonEntityExists(dto.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    // GET: PERSONENTITYS/Delete/5
    public async Task<IActionResult> Delete(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var personentity = await _context.Persons
            .FirstOrDefaultAsync(m => m.Id == id);
        if (personentity == null)
        {
            return NotFound();
        }

        return View(personentity);
    }

    // POST: PERSONENTITYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(System.Guid? id)
    {
        var personentity = await _context.Persons.FindAsync(id);
        if (personentity != null)
        {
            _context.Persons.Remove(personentity);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PersonEntityExists(System.Guid? id)
    {
        return _context.Persons.Any(e => e.Id == id);
    }
}
