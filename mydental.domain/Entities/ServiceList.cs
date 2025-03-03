namespace mydental.domain.Entities
{
    public class ServiceList
    {
        public int Id { get; private set; }
        public string ServiceName { get; private set; }
        public string Title { get; private set; }
        public string Content { get; private set; }
        public string Photo { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        // ✅ Private constructor for EF Core
        private ServiceList() { }

        // ✅ Public constructor for manual entity creation
        public ServiceList(string serviceName, string title, string content, string photo)
        {
            ServiceName = serviceName ?? throw new ArgumentException("Service name is required.");
            Title = title ?? throw new ArgumentException("Title is required.");
            Content = content ?? throw new ArgumentException("Content is required.");
            Photo = photo ?? throw new ArgumentException("Photo is required.");

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        // ✅ Update method
        public void Update(string serviceName, string title, string content, string photo)
        {
            ServiceName = serviceName ?? throw new ArgumentException("Service name is required.");
            Title = title ?? throw new ArgumentException("Title is required.");
            Content = content ?? throw new ArgumentException("Content is required.");
            Photo = photo ?? throw new ArgumentException("Photo is required.");
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
