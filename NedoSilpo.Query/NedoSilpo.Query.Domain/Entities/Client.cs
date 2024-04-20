namespace NedoSilpo.Query.Domain.Entities;

public class Client
{
    public Guid Id { get; set; }
    public string FullName { get; set; }
    public string PhoneNumber { get; set; }
    public string Email { get; set; }
    public string Address { get; set; }
    public bool IsInactive { get; set; }
}
