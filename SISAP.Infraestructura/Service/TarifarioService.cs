using Microsoft.EntityFrameworkCore;
using SISAP.CORE.Entities;
using SISAP.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.INFRAESTRUCTURA.Service
{
    public class TarifarioService : ITarifarioService
	{
        private SISAPDBContext _context;

        public TarifarioService(SISAPDBContext context)
        {
            _context = context;
        }

		public IEnumerable<Tarifario> getTarifario()
		{
			return _context.Tarifario.OrderBy(t => t.TarifarioId).ToList();
		}


		public IEnumerable<Tarifario> ListarTarifario(int pageSize, int skip, out int nroTotalRegistros)
		{

			var sql = (from t in _context.Tarifario
					   orderby t.TarifarioId ascending
					   select new
					   {
						   t.TarifarioId,
						   t.CategoriaId,
						   t.RangoMin,
						   t.RangoMax,
						   t.TarifaAgua,
						   t.TarifaAlcantarillado,
						   t.CargoFijo,
						   t.ClaseId

					   });
			nroTotalRegistros = sql.Count();
			sql = sql.Skip(skip).Take(pageSize);
			var ListFinal = (from t in sql.ToList()
							 select new Tarifario()
							 {
								 TarifarioId = t.TarifarioId,
								 CategoriaId = t.CategoriaId,
								 RangoMin = t.RangoMin,
								 RangoMax = t.RangoMax,
								 TarifaAgua = t.TarifaAgua,
								 TarifaAlcantarillado = t.TarifaAlcantarillado,
								 CargoFijo = t.CargoFijo,
								 ClaseId = t.ClaseId
							 }).ToList();
			return ListFinal;
 
		}

		public Tarifario Save(Tarifario objTarifario)
		{
			_context.Tarifario.Add(objTarifario);
			_context.SaveChanges();
			return objTarifario;

		}
		public void Update(Tarifario tarifario)
		{
			_context.Tarifario.Attach(tarifario);
			_context.Entry(tarifario).State = EntityState.Modified;
			_context.SaveChanges();
		}


		public void Delete(int TarifarioId)
		{
			var tarifarios = _context.Tarifario;
			var tarifario = tarifarios.FirstOrDefault(o => o.TarifarioId == TarifarioId);
			_context.Tarifario.Remove(tarifario);
			_context.SaveChanges();
		}

		public IEnumerable<Categoria> GetAllCategoria()
		{
			return _context.Categoria.OrderBy(o => o.CategoriaId).ToList();

		}

		public Tarifario GetDataTarifario(int categoriaID, decimal? consumo)
		{
			var data = _context.Tarifario.Where(t => t.CategoriaId == categoriaID).Where(d => consumo >= d.RangoMin && consumo <= d.RangoMax).FirstOrDefault();
			return data;
		}
	}
}
