namespace Aprecia.Domain.Gateway.Shared.Dtos.Request;

public class ResourceRequestBaseDto<T>
{
    public T? Data { get; set; } = default;    
}
