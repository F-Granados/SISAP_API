using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SISAP.CORE.Entities;

namespace SISAP.CORE.Interfaces
{
    public interface IDireccionService
    {
        IEnumerable<Direccion> GetAll();
        IDireccionService ObtenerDireccion(string Manzana, string Lote, int ClienteId);
    }
}
