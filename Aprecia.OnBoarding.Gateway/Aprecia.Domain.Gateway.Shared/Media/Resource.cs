using System.Text.Json.Serialization;
using Aprecia.Domain.Gateway.Shared.Dtos;

namespace Aprecia.Domain.Gateway.Shared.Media;

public class Resource<T>
{
    public Resource() { }
    public Resource(T data) 
    {
        Data = data;
    }

    public T? Data { get; set; }

    public StatusServiceDto StatusService { get; set; } = new StatusServiceDto();

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public Exception Exception { get; set; } = null;

    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public int StatusCode { get; set; } = 200;

    public object ToSerializableObject()
    {
        return new
        {
            Data,
            StatusService

        };
    }
}
