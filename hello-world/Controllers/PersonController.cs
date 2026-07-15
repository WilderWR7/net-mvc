using hello_world.Data.Repositories;
using hello_world.DTOs;
using hello_world.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class PersonController : Controller
{
    private readonly PersonRepository _personResitory;

    public PersonController(PersonRepository personResitory)
    {
        _personResitory = personResitory;
    }

    // GET: PERSONENTITYS
    public async Task<IActionResult> Index()    
    {
        return View(await _personResitory.GetAllAsync());
    }

    // GET: PERSONENTITYS/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var personentity = await _personResitory.GetByIdAsync((Guid)id);
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
            await _personResitory.AddAsync(personEntity);
            await _personResitory.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    // GET: PERSONENTITYS/Edit/5
    public async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var personentity = await _personResitory.GetByIdAsync((Guid)id);
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
    public async Task<IActionResult> Edit(Guid? id, UpdatePersonDto dto)
    {
        if (id != dto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var person = await _personResitory.GetByIdAsync(dto.Id);
                if (person == null)
                {
                    throw new InvalidOperationException($"No exsite la persona");
                }
                person.UpdatePersonEntity(dto.FirstName, dto.LastName, dto.Email, dto.PhoneNumber);
                await _personResitory.UpdateAsync(person);
                await _personResitory.SaveChangesAsync();
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

        var personentity = await _personResitory.GetByIdAsync((Guid)id);
        if (personentity == null)
        {
            return NotFound();
        }

        return View(personentity);
    }

    // POST: PERSONENTITYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        var personentity = await _personResitory.GetByIdAsync((Guid)id);
        if (personentity != null)
        {
            await _personResitory.DeleteAsync(personentity);
        }

        await _personResitory.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool PersonEntityExists(Guid? id)
    {
        return _personResitory.ExistsWithId(id);
    }
}
