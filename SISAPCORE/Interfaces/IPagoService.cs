using SISAP.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.CORE.Interfaces
{
	public interface IPagoService
	{
		IEnumerable<Facturacion> PayAllMonth(int? ClienteId);
		IEnumerable<Pago> GetPay(int? ClienteId, int pageSize, int skip, out int nroTotalRegistros);
		Pago Pagar(Pago objPago);
		IEnumerable<Cliente> GetAllCF(int? UrbanizacionId, string FilterNombre, int pageSize, int skip, out int nroTotalRegistros);
		IEnumerable<Facturacion> GetClienteDeudor(int? ClienteId, int pageSize, int skip, out int nroTotalRegistros);
	}
}
