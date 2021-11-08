using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SISAP.CORE.Entities;

namespace SISAP.CORE.Interfaces
{
    public interface IConsumoServicioService
    {
        IEnumerable<ConsumoServicio> GetAll();

        ConsumoServicio ObtenerConsumo(int ClienteId, int ServicioId, int PagoId);
    }
}
