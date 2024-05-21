using SistemaAPIRest.Entities;

namespace SistemaAPIRest.Persistence;

public class EventDbContext
{
    public List<Event> Events { get; set; }

    public EventDbContext()
    {
        Events = new List<Event>();
    }
}
