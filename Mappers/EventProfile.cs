using AutoMapper;
using SistemaAPIRest.Entities;
using SistemaAPIRest.Models;

namespace SistemaAPIRest.Mappers;

public class EventProfile : Profile
{
    public EventProfile()
    {
        CreateMap<Event, EventViewModel>();
        CreateMap<EventSpeaker, EventSpeakerViewModel>();

        CreateMap<EventInputModel, Event>();
        CreateMap<EventSpeakerInputModel, EventSpeaker>();
    }
}
