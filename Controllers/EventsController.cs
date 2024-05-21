using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SistemaAPIRest.Entities;
using SistemaAPIRest.Persistence;

namespace SistemaAPIRest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly EventDbContext _eventDbContext;

    public EventsController(EventDbContext eventDbContext)
    {
        _eventDbContext = eventDbContext;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var Events = _eventDbContext.Events.Where(e => !e.IsDeleted).ToList();
        return Ok(Events);
    }

    [HttpGet("{eventId}")]
    public IActionResult GetById(Guid eventId)
    {
        var Event = _eventDbContext.Events.SingleOrDefault(e => e.Id == eventId);
        if (Event == null)
        {
            return NotFound();
        }

        return Ok(Event);
    }

    [HttpPost]
    public IActionResult Post(Event evento)
    {
        _eventDbContext.Events.Add(evento);

        return CreatedAtAction(nameof(GetById), new { eventId = evento.Id }, evento);
    }

    [HttpPut("{eventId}")]
    public IActionResult Update(Guid eventId, Event evento)
    {
        var Event = _eventDbContext.Events.SingleOrDefault(e => e.Id == eventId);

        if (Event == null)
        {
            return NotFound();
        }

        Event.Update(evento.Title, evento.Description, evento.CreatedDate, evento.EndDate);

        return NoContent();
    }

    [HttpDelete("{eventId}")]
    public IActionResult Delete(Guid eventId)
    {
        var Event = _eventDbContext.Events.SingleOrDefault(e => e.Id == eventId);

        if (Event == null)
        {
            return NotFound();
        }

        Event.Delete();

        return NoContent();
    }

    [HttpPost("{eventId}/speakers")]
    public IActionResult PostSpeaker(EventSpeaker speaker, Guid eventId)
    {
        var Event = _eventDbContext.Events.SingleOrDefault(e => e.Id == eventId);

        if (Event == null)
        {
            return NotFound();
        }

        Event.Speakers.Add(speaker);

        return NoContent();
    }
}
