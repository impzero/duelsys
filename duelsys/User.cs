namespace duelsys;
public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool IsAdmin { get; set; }

    public User(int id, string firstName, string lastName, string email, string password, bool isAdmin)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        IsAdmin = isAdmin;
    }
    public User(string firstName, string lastName, string email, string password)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        IsAdmin = false;
    }


    public bool ValidatePassword(string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, this.Password);
    }
}

public struct UserBase
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string SecondName { get; private set; }
    public readonly string Names => FirstName + " " + SecondName;

    public UserBase(int id, string firstName, string secondName)
    {
        Id = id;
        FirstName = firstName;
        SecondName = secondName;
    }
}

public class LeaderboardUser
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public int WonGames { get; set; }
    public int DrawGames { get; set; }
    public int LostGames { get; set; }

    public string Stringify => $"{FirstName} {LastName} ({WonGames} won, {LostGames} lost)";

    public LeaderboardUser(int id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }
}
