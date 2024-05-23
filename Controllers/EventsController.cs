using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

    /// <summary>
    /// Obter todos os eventos
    /// </summary>
    /// <returns>Coleção de eventos</returns>
    /// <response code="200">Sucesso</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetAll()
    {
        var Events = _eventDbContext.Events
                                    .Include(es => es.Speakers)
                                    .Where(e => !e.IsDeleted).ToList();
        return Ok(Events);
    }

    /// <summary>
    /// Retorna um evento baseado no ID passado
    /// </summary>
    /// <param name="eventId">Identificado do evento</param>
    /// <returns>Dados de um evento específico</returns>
    /// <response code="200">Sucesso</response>
    /// <response code="404">Evento não encontrado</response>
    [HttpGet("{eventId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetById(Guid eventId)
    {
        var Event = _eventDbContext.Events
                                   .Include(es => es.Speakers)
                                   .SingleOrDefault(e => e.Id == eventId);
        if (Event == null)
        {
            return NotFound();
        }

        return Ok(Event);
    }

    /// <summary>
    /// Cadastrar um evento
    /// </summary>
    /// <remarks>
    /// {"title":"string","description":"string","startDate":"2023-02-27T17:59:14.141Z","endDate":"2023-02-27T17:59:14.141Z"}
    /// </remarks>
    /// <param name="evento">Dados do evento</param>
    /// <returns>Objeto evento recém-criado</returns>
    /// <response code="201">Sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post(Event evento)
    {
        _eventDbContext.Events.Add(evento);
        _eventDbContext.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { eventId = evento.Id }, evento);
    }

    /// <summary>
    /// Atualizar um evento
    /// </summary>
    /// <remarks>
    /// {"title":"string","description":"string","startDate":"2023-02-27T17:59:14.141Z","endDate":"2023-02-27T17:59:14.141Z"}
    /// </remarks>
    /// <param name="eventId">Identificado do evento</param>
    /// <param name="evento">Dados do evento</param>
    /// <returns>Contéudo vazio.</returns>
    /// <response code="204">Sucesso</response>
    /// <response code="404">Evento não encontrado</response>
    [HttpPut("{eventId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid eventId, Event evento)
    {
        var Event = _eventDbContext.Events.SingleOrDefault(e => e.Id == eventId);

        if (Event == null)
        {
            return NotFound();
        }

        Event.Update(evento.Title, evento.Description, evento.CreatedDate, evento.EndDate);
        _eventDbContext.Events.Update(Event);
        _eventDbContext.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Excluir um evento
    /// </summary>
    /// <param name="eventId">Identificador de evento</param>
    /// <returns>Conteúdo vazio</returns>
    /// <response code="204">Sucesso</response>
    /// <response code="404">Evento não encontrado</response>
    [HttpDelete("{eventId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Delete(Guid eventId)
    {
        var Event = _eventDbContext.Events.SingleOrDefault(e => e.Id == eventId);

        if (Event == null)
        {
            return NotFound();
        }

        Event.Delete();
        _eventDbContext.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Cadastrar palestrante
    /// </summary>
    /// <remarks>
    /// {"name":"string","talkTitle":"string","talkDescription":"string","linkedInProfile":"string"}
    /// </remarks>
    /// <param name="speaker">Dados do palestrante</param>
    /// <param name="eventId">Identificador do evento</param>
    /// <returns>Conteúdo vazio</returns>
    /// <response code="204">Sucesso</response>
    /// <response code="404">Evento não encontrado</response>
    [HttpPost("{eventId}/speakers")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PostSpeaker(EventSpeaker speaker, Guid eventId)
    {
        speaker.EventSpeakerId = eventId;
        var Event = _eventDbContext.Events.Any(e => e.Id == eventId);

        if (Event == null)
        {
            return NotFound();
        }

        _eventDbContext.Speakers.Add(speaker);
        _eventDbContext.SaveChanges();

        return NoContent();
    }
}
