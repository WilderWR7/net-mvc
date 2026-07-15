using hello_world.Models;
using Microsoft.EntityFrameworkCore;

namespace hello_world.Data.Repositories
{
    public class PersonRepository
    {
        private readonly ApplicationDbContext _context;
        public PersonRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PersonEntity?> GetByIdAsync(Guid id)
        {
            var person = await _context.Persons.FirstOrDefaultAsync(person => person.Id == id);
            if (person == null)
            {
                throw new InvalidOperationException($"No existe una persona con el Id: {id}");
            }
            return person;
        }


        public async Task<IEnumerable<PersonEntity>> GetAllAsync()
        {
            return await _context.Persons
                .AsNoTracking()
                .OrderBy(person => person.FirstName)
                .ThenBy(person => person.LastName)
                .ToListAsync();
        }
        public async Task<PersonEntity> AddAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            await _context.Persons.AddAsync(entity);
            return entity;

        }
        public Task UpdateAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Persons.Update(entity);

            return Task.CompletedTask;
        }
        public Task DeleteAsync(PersonEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
            _context.Persons.Remove(entity);
            return Task.CompletedTask;
        }

        public async Task<PersonEntity?> GetByCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("El codigo no puede estar vacio", nameof(code));
            }
            var normalizedCode = code.ToUpperInvariant();
            return await _context.Persons.FirstOrDefaultAsync(person => person.Code == normalizedCode);
        }
        public async Task<bool> ExistsWithCodeAsync(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("El codigo no puede estar vacio", nameof(code));
            }
            var normalizedCode = code.ToUpperInvariant();
            return await _context.Persons.AnyAsync(person => person.Code == normalizedCode);
        }

        public async Task<bool> ExistsWithIdAsync(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentException("El Id no puede estar vacio", nameof(id));
            }
            return await _context.Persons.AnyAsync(person =>person.Id == id);
        }
        public bool ExistsWithId(Guid? id)
        {
            if (id == null)
            {
                throw new ArgumentException("El Id no puede estar vacio", nameof(id));
            }
            return _context.Persons.Any(person => person.Id == id);
        }
        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
