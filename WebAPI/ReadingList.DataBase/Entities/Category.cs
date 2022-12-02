namespace ReadingList.DataBase.Entities;

public class Category : IBaseEntity
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}