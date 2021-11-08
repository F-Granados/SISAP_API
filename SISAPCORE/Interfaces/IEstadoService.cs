using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SISAP.CORE.Entities;

namespace SISAP.CORE.Interfaces
{
   public interface IEstadoService
    {
        IEnumerable<Estado> GetAll();
        IEstadoService ObtenetEstado(int EstadoId, int EstadoMedidor);
    }
}
