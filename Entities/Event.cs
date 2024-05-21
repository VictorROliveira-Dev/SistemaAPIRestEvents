namespace SistemaAPIRest.Entities;

public class Event
{
    public Event()
    {
        Speakers = new List<EventSpeaker>();
        IsDeleted = false;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime EndDate { get; set;}
    public List<EventSpeaker> Speakers { get; set; }
    public bool IsDeleted { get; set; }

    public void Update(string title, string description, DateTime createdDate, DateTime endDate)
    {
        Title = title;
        Description = description;
        CreatedDate = createdDate;
        EndDate = endDate;
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}