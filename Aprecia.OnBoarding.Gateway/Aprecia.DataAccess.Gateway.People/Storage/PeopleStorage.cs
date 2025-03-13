using System.Net.Http.Headers;
using Aprecia.Bussines.Gateway.People.ModelServices;
using Aprecia.Bussines.Gateway.People.Storage;
using Aprecia.DataAccess.Gateway.People.Consts;
using Newtonsoft.Json;

namespace Aprecia.DataAccess.Gateway.People.Storage;

public class PeopleStorage: IPeopleStorage
{
    private readonly string _url;
    public PeopleStorage(string url)
    {
        _url = url;
    }
    public async Task<IEnumerable<PersonGetList>> GetPersonList(string curp) 
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Post,
            RequestUri = new Uri($"{_url}PersonagetList"),            
            Content = new StringContent($"{ContsPeople.REQUEST_GET_PERSON_LIS.Replace("##CURP", curp)}")
            {
                Headers =
                {
                    ContentType = new MediaTypeHeaderValue("application/json")
                }
            }
        };

        var response = await client.SendAsync(request);
        if (!response.IsSuccessStatusCode) 
        {
            throw new Exception($"Error en la solicitud: {response.StatusCode} {await response.Content.ReadAsStringAsync()}");        
        }

        var body = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<IEnumerable<PersonGetList>>(body) ?? new List<PersonGetList>();

    }
}
