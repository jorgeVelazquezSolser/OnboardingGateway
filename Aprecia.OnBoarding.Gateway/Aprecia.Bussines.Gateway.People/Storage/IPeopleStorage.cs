using Aprecia.Bussines.Gateway.People.ModelServices;

namespace Aprecia.Bussines.Gateway.People.Storage;

public interface IPeopleStorage
{
    Task<IEnumerable<PersonGetList>> GetPersonList(string curp);
}
