using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SistemaAPIRest.Entities;
using SistemaAPIRest.Models;
using SistemaAPIRest.Persistence;

namespace SistemaAPIRest.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EventsController : ControllerBase
{
    private readonly EventDbContext _eventDbContext;
    private readonly IMapper _mapper;

    public EventsController(EventDbContext eventDbContext, IMapper mapper)
    {
        _eventDbContext = eventDbContext;
        _mapper = mapper;
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
        
        var viewModel = _mapper.Map<List<EventViewModel>>(Events);
        return Ok(viewModel);
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

        var viewModel = _mapper.Map<EventViewModel>(Event);
        return Ok(viewModel);
    }

    /// <summary>
    /// Cadastrar um evento
    /// </summary>
    /// <remarks>
    /// {"title":"string","description":"string","startDate":"2023-02-27T17:59:14.141Z","endDate":"2023-02-27T17:59:14.141Z"}
    /// </remarks>
    /// <param name="eventInputModel">Dados do evento</param>
    /// <returns>Objeto evento recém-criado</returns>
    /// <response code="201">Sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult Post(EventInputModel eventInputModel)
    {
        var Event = _mapper.Map<Event>(eventInputModel);

        _eventDbContext.Events.Add(Event);
        _eventDbContext.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { eventId = Event.Id }, Event);
    }

    /// <summary>
    /// Atualizar um evento
    /// </summary>
    /// <remarks>
    /// {"title":"string","description":"string","startDate":"2023-02-27T17:59:14.141Z","endDate":"2023-02-27T17:59:14.141Z"}
    /// </remarks>
    /// <param name="eventId">Identificado do evento</param>
    /// <param name="eventInputModel">Dados do evento</param>
    /// <returns>Contéudo vazio.</returns>
    /// <response code="204">Sucesso</response>
    /// <response code="404">Evento não encontrado</response>
    [HttpPut("{eventId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult Update(Guid eventId, EventInputModel eventInputModel)
    {
        var Event = _eventDbContext.Events.SingleOrDefault(e => e.Id == eventId);

        if (Event == null)
        {
            return NotFound();
        }

        Event.Update(eventInputModel.Title, eventInputModel.Description, eventInputModel.CreatedDate, eventInputModel.EndDate);
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
    /// <param name="speakerInpuModel">Dados do palestrante</param>
    /// <param name="eventId">Identificador do evento</param>
    /// <returns>Conteúdo vazio</returns>
    /// <response code="204">Sucesso</response>
    /// <response code="404">Evento não encontrado</response>
    [HttpPost("{eventId}/speakers")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult PostSpeaker(EventSpeakerInputModel speakerInpuModel, Guid eventId)
    {
        var speaker = _mapper.Map<EventSpeaker>(speakerInpuModel);
        
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
