using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.CORE.Entities
{
   public class Categoria
    {
        [Key]
        public int CategoriaId { get; set; }
        public string TipoCategoria { get; set; }
        public int ClaseId { get; set; }
    }
}
