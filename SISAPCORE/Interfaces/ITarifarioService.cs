using SISAP.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.CORE.Interfaces
{
	public interface ITarifarioService
	{
		IEnumerable<Tarifario> getTarifario();
		void Update(Tarifario tarifario);
		void Delete(int TarifarioId);
		Tarifario Save(Tarifario objTarifario);
		IEnumerable<Tarifario> ListarTarifario(int pageSize, int skip, out int nroTotalRegistros);

		Tarifario GetDataTarifario(int categoriaID, decimal? consumo);
	}
}
