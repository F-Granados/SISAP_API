using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.CORE.Entities
{
    public class Medidor
    {
        public int MedidorId { get; set; }
        public int ClienteId { get; set; }
        public int EstadoId { get; set; }
        public int LecturaId { get; set; }
        public int NumeroMedidor { get; set; }

    }
}
