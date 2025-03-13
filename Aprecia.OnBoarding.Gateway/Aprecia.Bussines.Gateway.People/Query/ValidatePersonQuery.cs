using Aprecia.Domain.Gateway.People.Dto.Request;
using Aprecia.Domain.Gateway.People.Media;
using MediatR;

namespace Aprecia.Bussines.Gateway.People.Query;

public class ValidatePersonQuery : IRequest<ValidatePersonResourceImpl>
{
    public ValidatePersonQuery(DataValidatePersonRquestDto _validate) => validate = _validate;

    public DataValidatePersonRquestDto validate { get; }
}
