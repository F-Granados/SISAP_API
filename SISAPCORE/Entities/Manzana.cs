using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.CORE.Entities
{
	public class Manzana
	{
		public int ManzanaId { get; set; }
		public string NombreManzana { get; set; }
		public int UrbanizacionId { get; set; }

		[NotMapped]
		public string NombreUrbanizacion { get; set; }
	}
}
