using SISAP.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.CORE.Interfaces
{
	public interface IReportesService
	{
		decimal? getDeudaDistrito(int? Annio);
		decimal? getDeudaRuta(int? Annio, int? Mes, int? UrbanizacionId);
		decimal? getIngresoAnual(int? Annio);
		decimal? getIngresoMensual(int? Annio, int? Mes);
		decimal? getProcessLectura(int? Annio, int? Mes);
		IEnumerable<Cliente> ListReporte(int? Annio, string FilterNombre, int pageSize, int skip, out int nroTotalRegistros);
		IEnumerable<Cliente> GetAllCF(int? UrbanizacionId, string FilterNombre, int pageSize, int skip, out int nroTotalRegistros);
	}
}
