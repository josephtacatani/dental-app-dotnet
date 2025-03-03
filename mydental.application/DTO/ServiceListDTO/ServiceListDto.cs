namespace mydental.application.DTO.ServiceListDTO;

public class ServiceListDto
{
    public int Id { get; set; }
    public string ServiceName { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string Photo { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // ✅ Parameterless constructor for model binding (required by ASP.NET Core)
    public ServiceListDto() { }

    // ✅ Constructor for explicit assignments
    public ServiceListDto(int id, string serviceName, string title, string content, string photo, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        ServiceName = serviceName;
        Title = title;
        Content = content;
        Photo = photo;
        CreatedAt = createdAt;
    }
}