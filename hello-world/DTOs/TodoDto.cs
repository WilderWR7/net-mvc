namespace hello_world.DTOs
{
    public class TodoDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool IsCompleted { get; set; }
        public string UserId { get; set; } = string.Empty;
        public TodoIdentityUserDto? User { get; set; }
    }

    public class TodoIdentityUserDto
    {
        public string? Id { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public bool? EmailConfirmed { get; set; }
    }
}
