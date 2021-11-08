using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SISAP.CORE.Entities;

namespace SISAP.CORE.Interfaces
{
    public interface IMedidorService
    {
        IEnumerable<Medidor> GetAll();
        IMedidorService ObtenerMedidor(int MedidorId, int ClienteId, int EstadoId, int LecturaId, string NumeroMedidor);
    }
}
