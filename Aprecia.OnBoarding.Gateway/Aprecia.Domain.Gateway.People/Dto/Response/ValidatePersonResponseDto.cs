namespace Aprecia.Domain.Gateway.People.Dto.Response;

public class ValidatePersonResponseDto
{
    public string Curp { get; set; }
    public string Nombre { get; set; }
    public bool TieneCreditos { get; set; }
    public double PorcentajeCoincidencia { get; set; }

}
