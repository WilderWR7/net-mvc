using System.Text.RegularExpressions;

namespace hello_world.Models
{
    public class PersonEntity
    {
        public Guid Id { get; private set; }
        public string Code { get; private set; } = string.Empty;
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string PhoneNumber { get; private set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}";

        //private readonly List<TodoEntity> _todos = new();
        //public IReadOnlyCollection<TodoEntity> Todos => _todos.AsReadOnly();

        public PersonEntity(string code, string firstName, string lastName, string email, string phoneNumber)
        {
            validateCode(code);
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidatePhoneNumber(phoneNumber);
            ValidateEmail(email);
            Code = code.Trim().ToUpper();
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            PhoneNumber = phoneNumber.Trim();
        }

        public void UpdatePersonEntity(string firstName, string lastName, string email, string phoneNumber)
        {
            ValidateFirstName(firstName);
            ValidateLastName(lastName);
            ValidatePhoneNumber(phoneNumber);
            FirstName = firstName.Trim();
            LastName = lastName.Trim();
            Email = email.Trim();
            PhoneNumber = phoneNumber.Trim();
        }

        private void validateCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("El código no puede estar vacío", nameof(code));
            }
            if (code.Trim().Length < 3)
            {
                throw new ArgumentException("El código debe tener al menos 3 caracteres", nameof(code));
            }
            if (code.Trim().Length > 20)
            {
                throw new ArgumentException("El código no debe tener mas de 20 caracteres", nameof(code));
            }
        }

        private void ValidateFirstName(string firtName)
        {
            if (string.IsNullOrWhiteSpace(firtName))
            {
                throw new ArgumentException("El Nombre no puede estar vacío", nameof(firtName));
            }
            if (firtName.Trim().Length < 2)
            {
                throw new ArgumentException("El nombre debe tener al menos 2 caracteres.", nameof(firtName));
            }
            if (firtName.Trim().Length > 20)
            {
                throw new ArgumentException("El nombre no puede tener mas de 20 caracteres", nameof(firtName));
            }

        }
        private void ValidateLastName(string firtName)
        {
            if (string.IsNullOrWhiteSpace(firtName))
            {
                throw new ArgumentException("El apellido no puede estar vacío", nameof(firtName));
            }
            if (firtName.Trim().Length < 2)
            {
                throw new ArgumentException("El apellido debe tener al menos 2 caracteres.", nameof(firtName));
            }
            if (firtName.Trim().Length > 20)
            {
                throw new ArgumentException("El apellido no puede tener mas de 20 caracteres", nameof(firtName));
            }

        }
        private void ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("El email no puede estar vacio", nameof(email));
            }

            if (email.Length > 100)
            {
                throw new ArgumentException("El correo electronio no puede exceder los 100 caracteres.", nameof(email));
            }
            var emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            if (!Regex.IsMatch(email, emailPattern))
            {
                throw new ArgumentException("El formato del correo electrónico es inválido.", nameof(email));
            }
        }
        private void ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                throw new ArgumentException("El numero de telefono no puede estar vacío", nameof(phoneNumber));
            }
            if (phoneNumber.Trim().Length < 7)
            {
                throw new ArgumentException("El numero de telefono debe tener el menos 7 caracteres", nameof(phoneNumber));
            }
            if (phoneNumber.Trim().Length > 20)
            {
                throw new ArgumentException("El numero de telefono no debe tener mas de 20 caracteres", nameof(phoneNumber));
            }
        }


    }
}
