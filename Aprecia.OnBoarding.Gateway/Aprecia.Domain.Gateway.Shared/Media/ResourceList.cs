using System.Text.Json.Serialization;
using Aprecia.Domain.Gateway.Shared.Dtos;

public class ResourceList<T>
{
    public ResourceList(IEnumerable<T> data)
    {
        Data = data;
    }

    public IEnumerable<T> Data { get; set; }
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
