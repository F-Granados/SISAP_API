using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SISAP.CORE.Entities;

namespace SISAP.CORE.Interfaces
{
   public interface IDetalleFacturaService
    {
        IEnumerable<DetalleFactura> GetAll();
        IDetalleFacturaService ObtenerDetalleFactura(int DetalleFacturaId, int ConsumoServicioId, int ServicioId, string UsuarioCreacion, string Cantidad, string CostoCubo, string CostoRefacturacion, string CostoIGV, string CostoTotal);
    }
}
