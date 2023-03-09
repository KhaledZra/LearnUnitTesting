using Lab04.Domain.Model;

namespace Lab04.Domain.Interface;

public interface IDataBase
{
    // forces crud features

    public bool SaveToDatabase(Booking booking);
    public Booking GetFromDatabase(int id);
    public bool UpdateToDatabase(Booking booking);
    public bool RemoveFromDatabase(int id);
}