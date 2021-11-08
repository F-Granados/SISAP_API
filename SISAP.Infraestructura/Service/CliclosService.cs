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
	public class CliclosService : ICiclosService
    {
		private SISAPDBContext _context;

		public CliclosService(SISAPDBContext context)
		{
			_context = context;
		}

		public IEnumerable<Ciclos> EnableToNextPrecess(int? Annio, int? Mes)
		{

			int? NextMonth = Mes + 2;
			int? NextYear = Annio + 2;
			var sql = (from c in _context.Ciclos
					   where c.Annio > Annio
					   select c).ToList();
			if (sql.Count() > 0)
			{
				if (Mes < 12)
					return _context.Ciclos.Where(c => c.Annio == Annio).ToList();
				return _context.Ciclos.Where(c => c.Annio > Annio).ToList();
			}
			else
			{
				return _context.Ciclos.Where(c => c.Annio == Annio && c.Mes > Mes && (c.Mes < NextMonth)).ToList();

			}

		}

		public Ciclos Save(Ciclos objCiclos)
		{

			_context.Ciclos.Add(objCiclos);
			_context.SaveChanges();

			return objCiclos;
		}
		public void Update(Ciclos objCiclos)
		{

			_context.Ciclos.Attach(objCiclos);
			_context.Entry(objCiclos).State = EntityState.Modified;
			_context.SaveChanges();

		}

		public void Delete(int CiclosId)
		{

			var ciclos = _context.Ciclos;
			var ciclo = ciclos.FirstOrDefault(o => o.CiclosId == CiclosId);

			_context.Ciclos.Remove(ciclo);
			_context.SaveChanges();

		}

		public IEnumerable<Ciclos> ListarCiclos(int pageSize, int skip, out int nroTotalRegistros)
		{

			var sql = (from c in _context.Ciclos
					   orderby c.CiclosId descending
					   select new
					   {
						   c.CiclosId,
						   c.Annio,
						   c.Mes,
						   c.LecturaInicial,
						   c.LecturaFinal,
						   c.EmisionInicial,
						   c.EmisionFinal,
						   c.DistribucionInicial,
						   c.DistribucionFinal,
						   c.CorteInicial,
						   c.CorteFinal
					   });

			nroTotalRegistros = sql.Count();
			sql = sql.Skip(skip).Take(pageSize);

			var ListFinal = (from c in sql.ToList()
							 select new Ciclos()
							 {
								 CiclosId = c.CiclosId,
								 Annio = c.Annio,
								 Mes = c.Mes,
								 LecturaInicialStr = string.Format("{0:dd/MM/yyyy}", c.LecturaInicial),
								 LecturaFinalStr = string.Format("{0:dd/MM/yyyy}", c.LecturaFinal),
								 EmisionInicialStr = string.Format("{0:dd/MM/yyyy}", c.EmisionInicial),
								 EmisionFinalStr = string.Format("{0:dd/MM/yyyy}", c.EmisionFinal),
								 DistribucionInicialStr = string.Format("{0:dd/MM/yyyy}", c.DistribucionInicial),
								 DistribucionFinalStr = string.Format("{0:dd/MM/yyyy}", c.DistribucionFinal),
								 CorteInicialStr = string.Format("{0:dd/MM/yyyy}", c.CorteInicial),
								 CorteFinalStr = string.Format("{0:dd/MM/yyyy}", c.CorteFinal)
							 }).ToList();

			return ListFinal;


		}

		public IEnumerable<Ciclos> ListarMesByAnnio(int? Annio)
		{
			var sql = (from c in _context.Ciclos
					   where Annio == c.Annio
					   select new
					   {
						   c.CiclosId,
						   c.Annio,
						   c.Mes
					   }).ToList().Select(o => new Ciclos()
					   {
						   CiclosId = o.CiclosId,
						   Annio = o.Annio,
						   Mes = o.Mes
					   });
			return sql.ToList();
		}

        public IEnumerable<Ciclos> ListarAnnios()
        {
 
				var sql = (from c in _context.Ciclos
						   select new
						   {
							   c.CiclosId,
							   c.Annio
						   }).ToList().Select(o => new Ciclos()
						   {
							   CiclosId = o.CiclosId,
							   Annio = o.Annio
						   });
				return sql.ToList();
			 
		}
    }
}
