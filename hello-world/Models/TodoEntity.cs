using Microsoft.AspNetCore.Identity;

namespace hello_world.Models
{
    public class TodoEntity
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; private set; } = string.Empty;
        public bool IsCompleted { get; private set; }

        public string UserId { get; set; } = string.Empty;
        public IdentityUser User { get; set; } = null!;

        public TodoEntity(string title, string description, string userId)
        {
            ValidateTitle(title);
            Title = title.Trim();
            Description = description?.Trim() ?? string.Empty;
            UserId = userId;
            IsCompleted = false;
        }

        public void UpdateTask(string title, string description)
        {
            ValidateTitle(title);
            Title = title.Trim();
            Description = description?.Trim() ?? string.Empty;
        }

        public void CompleteTask()
        {
            IsCompleted = true;
        }

        public void UncompleteTask()
        {
            IsCompleted = false;
        }

        private void ValidateTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
            {
                throw new ArgumentException("El titulo de la tarea no puede estar vacio", nameof(title));
            }
            if (title.Trim().Length > 100)
            {
                throw new ArgumentException("El titulo no puede exceder los 100 caracteres", nameof(title));
            }
        }
    }
}
