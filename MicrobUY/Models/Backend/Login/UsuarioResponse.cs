using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobUY.Models.Backend.Login;

public class UsuarioResponse
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Biografia { get; set; }
    public string Ocupacion { get; set; }
    public string FechaNacimiento { get; set; }
    public string Ubicacion { get; set; }
    public int TenantId { get; set; }
    public string Token { get; set; }
}
