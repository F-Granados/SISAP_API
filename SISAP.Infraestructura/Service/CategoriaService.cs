using SISAP.CORE.Entities;
using SISAP.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.INFRAESTRUCTURA.Service
{
    public class CategoriaService : _BaseContext, ICategoriaService
    {
        private SISAPDBContext _context;

        public CategoriaService(SISAPDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Categoria> GetAll()
        {
           
            return _context.Categoria.OrderBy(o => o.CategoriaId).ToList();
            
        }
        public  int Insertar(Categoria categoria)
        {
             _context.Categoria.Add(categoria);
            var res = _context.SaveChanges();

            return categoria.CategoriaId;

        }
    }
}
