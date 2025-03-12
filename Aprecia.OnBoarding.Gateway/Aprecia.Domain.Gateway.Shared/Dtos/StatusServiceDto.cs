namespace Aprecia.Domain.Gateway.Shared.Dtos;

public class StatusServiceDto
{
    public string Id { get; set; }
    public bool Status {  get; set; }
    public ErrorDto? Error { get; set; } = null;
}
