
using hello_world.Data;
using hello_world.Data.Repositories;
using hello_world.DTOs;
using hello_world.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public class TodoController : Controller
{
    private static readonly HashSet<string> AllowedUserFields = new(StringComparer.OrdinalIgnoreCase)
    {
        "id",
        "userId",
        "userName",
        "email",
        "phoneNumber",
        "emailConfirmed"
    };

    private readonly TodoRepository _todoResitory;
    private readonly UserManager<IdentityUser> _userManger;

    public TodoController(TodoRepository todoResitory, UserManager<IdentityUser> userManger)
    {
        _todoResitory = todoResitory;
        _userManger = userManger;
    }

    // GET: TODOENTITYS
    public async Task<IActionResult> Index()    
    {
        return View(await _todoResitory.GetAllWithUserAsync(["userName"]));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllWithUser([FromQuery] string? userFields)
    {
        var selectedUserFields = ParseUserFields(userFields);
        var invalidUserFields = selectedUserFields
            .Where(field => !AllowedUserFields.Contains(field))
            .ToArray();

        if (invalidUserFields.Length > 0)
        {
            return BadRequest(new
            {
                Message = "Uno o mas campos de IdentityUser no son validos.",
                InvalidFields = invalidUserFields,
                AllowedFields = AllowedUserFields.OrderBy(field => field)
            });
        }

        var todos = await _todoResitory.GetAllWithUserAsync(selectedUserFields);
        return Ok(todos);
    }

    // GET: TODOENTITYS/Details/5
    public async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoentity = await _todoResitory.GetByIdAsync((Guid)id);
        if (todoentity == null)
        {
            return NotFound();
        }

        return View(todoentity);
    }

    // GET: TODOENTITYS/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: TODOENTITYS/Create
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateTodoDto dto)
    {
        var userId = _userManger.GetUserId(User);
        if (userId == null)
        {
            return Challenge();
        }

        if (ModelState.IsValid)
        {
            var todoEntity = new TodoEntity(dto.Title.Trim(), dto.Description.Trim(),userId);
            await _todoResitory.AddAsync(todoEntity);
            await _todoResitory.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(dto);
    }

    // GET: TODOENTITYS/Edit/5
    public async Task<IActionResult> Edit(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoentity = await _todoResitory.GetByIdAsync((Guid)id);
        if (todoentity == null)
        {
            return NotFound();
        }
        return View(todoentity);
    }

    // POST: TODOENTITYS/Edit/5
    // To protect from overposting attacks, enable the specific properties you want to bind to.
    // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(System.Guid? id, UpdateTodoDto dto)
    {
        if (id != dto.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                var todoEntity = await _todoResitory.GetByIdAsync((Guid)id);
                if (todoEntity == null) {
                    throw new DbUpdateConcurrencyException();
                }
                todoEntity.UpdateTask(dto.Title, dto.Description);
                await _todoResitory.UpdateAsync(todoEntity);
                await _todoResitory.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoEntityExists(id))
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

    // GET: TODOENTITYS/Delete/5
    public async Task<IActionResult> Delete(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoentity = await _todoResitory.GetByIdAsync((Guid)id);
        if (todoentity == null)
        {
            return NotFound();
        }

        return View(todoentity);
    }

    // POST: TODOENTITYS/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(System.Guid? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var todoentity = await _todoResitory.GetByIdAsync(id.Value);
        if (todoentity != null)
        {
            await _todoResitory.DeleteAsync(todoentity);
        }

        await _todoResitory.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool TodoEntityExists(System.Guid? id)
    {
        return _todoResitory.ExistsWithId(id);
    }

    private static string[] ParseUserFields(string? userFields)
    {
        if (string.IsNullOrWhiteSpace(userFields))
        {
            return [];
        }

        return userFields
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(field => !string.IsNullOrWhiteSpace(field))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
    }
}
