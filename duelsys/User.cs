namespace duelsys
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string SecondName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }

        public User(int id, string firstName, string secondName, string email, string password, bool isAdmin)
        {
            Id = id;
            FirstName = firstName;
            SecondName = secondName;
            Email = email;
            Password = password;
            IsAdmin = isAdmin;
        }

        public bool ValidatePassword(string password)
        {
            return BCrypt.Net.BCrypt.Verify(password, this.Password);
        }

    }
}
