namespace Monitor.SPA.Models.Interface
{
    public interface IBaseEntity
    {
        string Id { get; set; }
        long SubscriptionId { get; set; }
    }
}
