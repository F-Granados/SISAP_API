using SISAP.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.INFRAESTRUCTURA.Service
{
    public class OpcionService 
    {
		private SISAPDBContext _context;

		public OpcionService(SISAPDBContext context)
		{
			_context = context;
		}
        public List<Opcion> ListarOpciones()
        {
            var lista = (from o in _context.Opcion
                         select o).ToList();
            return lista;


        }

    }
}
