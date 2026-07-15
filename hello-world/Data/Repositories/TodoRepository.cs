using hello_world.DTOs;
using hello_world.Models;
using Microsoft.EntityFrameworkCore;

namespace hello_world.Data.Repositories
{
    public class TodoRepository
    {
        private readonly ApplicationDbContext _context;

        public TodoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TodoEntity> AddAsync(TodoEntity entity)
        {
            await _context.Todos.AddAsync(entity);
            return entity;
        }

        public async Task DeleteAsync(TodoEntity entity)
        {
            _context.Todos.Remove(entity);
            await Task.CompletedTask;
        }

        public async Task<IEnumerable<TodoEntity>> GetAllAsync()
        {
            return await _context.Todos.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TodoDto>> GetAllWithUserAsync(IEnumerable<string>? userFields = null)
        {
            var fields = new HashSet<string>(
                userFields ?? Enumerable.Empty<string>(),
                StringComparer.OrdinalIgnoreCase);

            var includeDefaultFields = fields.Count == 0;
            var includeUserId = includeDefaultFields || fields.Contains("id") || fields.Contains("userId");
            var includeUserName = includeDefaultFields || fields.Contains("userName");
            var includeEmail = fields.Contains("email");
            var includePhoneNumber = fields.Contains("phoneNumber");
            var includeEmailConfirmed = fields.Contains("emailConfirmed");

            return await _context.Todos
                .AsNoTracking()
                .Select(todo => new TodoDto
                {
                    Id = todo.Id,
                    Title = todo.Title,
                    Description = todo.Description,
                    IsCompleted = todo.IsCompleted,
                    UserId = todo.UserId,
                    User = new TodoIdentityUserDto
                    {
                        Id = includeUserId ? todo.User.Id : null,
                        UserName = includeUserName ? todo.User.UserName : null,
                        Email = includeEmail ? todo.User.Email : null,
                        PhoneNumber = includePhoneNumber ? todo.User.PhoneNumber : null,
                        EmailConfirmed = includeEmailConfirmed ? todo.User.EmailConfirmed : null
                    }
                })
                .ToListAsync();
        }

        public async Task<TodoEntity?> GetByIdAsync(Guid id)
        {
            return await _context.Todos.FindAsync(id);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public bool ExistsWithId(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentException("El Id no puede estar vacio", nameof(id));
            }
            return _context.Todos.Any(todo => todo.Id == id);
        }

        public async Task UpdateAsync(TodoEntity entity)
        {
            _context.Todos.Update(entity);
            await Task.CompletedTask;
        }
    }
}
