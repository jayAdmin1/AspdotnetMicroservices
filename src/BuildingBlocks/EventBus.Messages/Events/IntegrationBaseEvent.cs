namespace EventBus.Messages.Events
{
    public class IntegrationBaseEvent
    {
        public IntegrationBaseEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }
        public IntegrationBaseEvent(Guid id,DateTime createdDate)
        {
            Id = id;
            CreationDate = createdDate;
        }
        public Guid Id { get; private set; }
        public DateTime CreationDate { get; private set; }
    }
}
