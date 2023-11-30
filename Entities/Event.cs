namespace ITentikaTest.Entities
{
    public class Event
    {
        //вообще это модель, но у нас будет сущность
        public Event(EventType eventType)
        {
            Id = Guid.NewGuid();
            EventType = eventType;
            Time = DateTime.Now;
        }

        public Guid Id { get; private set; }
        public EventType EventType { get; private set; }
        public DateTime Time { get; private set; }
    }

    public enum EventType
    {
        First = 1,
        Second = 2,
        Third = 3,
        Fourth = 4
    }

}
