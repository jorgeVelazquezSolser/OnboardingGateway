namespace Aprecia.Bussines.Gateway.People.ModelServices;

public class PersonGetList
{
    public long id_Persona { get; set; }
    public string nombre { get; set; }
    public string rfc { get; set; }
    public long sdo_Insoluto { get; set; }
    public long cuota_Amortizacion { get; set; }
    public long cuota_Mensual_Amortizacion { get; set; }
    public long cuenta_creditos_activos { get; set; }
    public string fecha_inicio_ultmo_credito { get; set; }
    public string curp { get; set; }
}
