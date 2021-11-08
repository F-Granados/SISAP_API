using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SISAP.CORE.Entities;

namespace SISAP.CORE.Interfaces
{
    public interface IDetalleEstadoPagoService
    {
        IEnumerable<DetalleEstadoPago> GetAll();
        IDetalleEstadoPagoService ObtenerEstadoPago(int DetalleEstadoPagoId, int DetalleEstadoPagos);
    }
}
