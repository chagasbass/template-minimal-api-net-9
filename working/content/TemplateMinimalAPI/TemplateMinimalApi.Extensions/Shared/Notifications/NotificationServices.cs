namespace TemplateMinimalApi.Extensions.Shared.Notifications;

public class NotificationServices : INotificationServices
{
    public StatusCodeOperation? StatusCode { get; set; }

    private readonly ICollection<Notification> _notifications;

    public NotificationServices()
    {
        _notifications = new List<Notification>();
    }

    public void AddNotification(Notification notification) => _notifications.Add(notification);

    public void AddNotification(Notification notification, StatusCodeOperation statusCode)
    {
        AddNotification(notification);
        AddStatusCode(statusCode);
    }

    public void AddNotifications(IEnumerable<Notification> notifications)
    {
        foreach (var notification in notifications)
        {
            _notifications.Add(notification);
        }
    }

    public void AddNotifications(IEnumerable<Notification> notifications, StatusCodeOperation statusCode)
    {
        AddNotifications(notifications);
        AddStatusCode(statusCode);
    }

    public void AddStatusCode(StatusCodeOperation statusCode) => StatusCode = statusCode;

    public bool HasNotifications() => GetNotifications().Any();

    public IEnumerable<Notification> GetNotifications() => _notifications;

    public void ClearNotifications() => _notifications.Clear();
}
