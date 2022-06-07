namespace duelsys.ApplicationLayer.Views;
public struct UserBase
{
    public int Id { get; private set; }
    public string FirstName { get; private set; }
    public string SecondName { get; private set; }

    public UserBase(int id, string firstName, string secondName)
    {
        Id = id;
        FirstName = firstName;
        SecondName = secondName;
    }
}
