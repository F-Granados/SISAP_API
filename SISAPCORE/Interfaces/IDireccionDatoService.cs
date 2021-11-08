using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SISAP.CORE.Entities;

namespace SISAP.CORE.Interfaces
{
    public interface IDireccionDatoService
    {
        IEnumerable<DireccionDato> GetAll();
        IDireccionDatoService ObtenerDireccionDato(int DireccionDatoId, int ClienteId, int DireccionId, int UrbanizacionId);
    }
}
