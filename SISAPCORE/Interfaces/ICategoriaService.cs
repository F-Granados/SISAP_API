using SISAP.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.CORE.Interfaces
{
    public interface  ICategoriaService
    {
        IEnumerable<Categoria> GetAll();
        int Insertar(Categoria categoria);
    }
}
