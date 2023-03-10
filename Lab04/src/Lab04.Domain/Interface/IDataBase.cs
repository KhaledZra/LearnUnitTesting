using Lab04.Domain.Model;

namespace Lab04.Domain.Interface;

public interface IDataBase
{
    // forces crud features

    public bool SaveToDatabase(BookingDocument booking);
    public BookingDocument GetFromDatabase(int id);
    public bool UpdateToDatabase(BookingDocument booking);
    public bool RemoveFromDatabase(int id);
}