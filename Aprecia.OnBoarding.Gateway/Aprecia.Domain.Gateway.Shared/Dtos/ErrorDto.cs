namespace Aprecia.Domain.Gateway.Shared.Dtos;

public class ErrorDto
{
    public string ErrorMensaje { get; set; }
    public string ErrorCodigo { get; set; }
    public int ErrorNivel { get; set; }
    public string ErrorDescripcionTecnica { get; set; }

}
