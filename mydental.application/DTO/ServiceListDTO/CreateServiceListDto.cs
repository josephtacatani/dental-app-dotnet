namespace mydental.application.DTO.ServiceListDTO
{
    public class CreateServiceListDto
    {
        public string ServiceName { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Photo { get; set; }

        // ✅ Parameterless constructor for model binding
        public CreateServiceListDto() { }

        // ✅ Constructor for explicit assignments
        public CreateServiceListDto(string serviceName, string title, string content, string photo)
        {
            ServiceName = serviceName;
            Title = title;
            Content = content;
            Photo = photo;
        }
    }
}
