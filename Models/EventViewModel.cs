﻿namespace SistemaAPIRest.Models;

public class EventViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<EventSpeakerViewModel> Speakers { get; set; }
}

public class EventSpeakerViewModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string TalkTitle { get; set; }
    public string TalkDescription { get; set; }
    public string LinkedinProfile { get; set; }
}
