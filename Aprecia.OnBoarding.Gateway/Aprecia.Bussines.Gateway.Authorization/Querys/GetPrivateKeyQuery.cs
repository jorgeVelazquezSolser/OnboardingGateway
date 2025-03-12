using Aprecia.Domain.Gateway.Authorization.Media;
using MediatR;

namespace Aprecia.Bussines.Gateway.Authorization.Querys;
public class GetPrivateKeyQuery:IRequest<AuthorizationResourceImpl>
{
    public GetPrivateKeyQuery(string _thumbprint) => Thumbprint = _thumbprint;
    public string Thumbprint { get; set; }
}
