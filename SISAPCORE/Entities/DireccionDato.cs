using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.CORE.Entities
{
    public class DireccionDato
    {
        public int DireccionDatoId { get; set; }
        public int ClienteId { get; set; }
        public int DireccionId { get; set; }
        public int UrbanizacionId { get; set; }
    }
}
