using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SISAP.CORE.Entities;

namespace SISAP.CORE.Interfaces
{
   public interface IEstadoPagoService
    {
        IEnumerable<EstadoPago> GetAll();
        IEstadoPagoService ObtenerEstadoPago(int EstadoPagoId, int DetalleEstadoPagoId);
    }
}
