using System.ComponentModel.DataAnnotations;

namespace Aprecia.Domain.Gateway.People.Dto.Request;

public class DataValidatePersonRquestDto
{
	[Required]
    public string PrimerNombre{get; set;}
	public string SegundoNombre{get; set;}
    [Required]
    public string PrimerApellido{get; set;}
	public string SegundoApellido{get; set;}
    [Required]
    public string Curp { get; set; }
}
