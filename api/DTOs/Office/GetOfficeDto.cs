namespace api.DTOs.Office;

public class GetOfficeDto
{
    public GetOfficeDto(int id, string name, string location)
    {
        Id = id;
        Name = name;
        Location = location;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
}