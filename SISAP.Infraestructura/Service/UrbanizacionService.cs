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
   public class UrbanizacionService : IUrbanizacionService
	{
        private SISAPDBContext dbContext;

        public UrbanizacionService(SISAPDBContext context)
        {
			dbContext = context;
        }

		public Urbanizacion Create(Urbanizacion objUrbanizacion)
		{
			dbContext.Urbanizacion.Add(objUrbanizacion);
			dbContext.SaveChanges();

			return objUrbanizacion;

		}
		public Manzana CreateManzana(Manzana objManzana)
		{
			dbContext.Manzana.Add(objManzana);
			dbContext.SaveChanges();

			return objManzana;

		}

		public Urbanizacion UpdateUrbanizacion(Urbanizacion objUrbanizacion)
		{
			dbContext.Urbanizacion.Attach(objUrbanizacion);
			dbContext.Entry(objUrbanizacion).State = EntityState.Modified;
			dbContext.SaveChanges();
			return dbContext.Urbanizacion.Where(o => o.UrbanizacionId == objUrbanizacion.UrbanizacionId).FirstOrDefault();
		}

		public void UpdateManzana(Manzana objManzana)
		{
			dbContext.Manzana.Attach(objManzana);
			dbContext.Entry(objManzana).State = EntityState.Modified;
			dbContext.SaveChanges();
		}

		public void DeleteUrbanizacion(int UrbanizacionId)
		{
			var urbanizacions = dbContext.Urbanizacion;
			var id = urbanizacions.FirstOrDefault(c => c.UrbanizacionId == UrbanizacionId);

			dbContext.Urbanizacion.Remove(id);
			dbContext.SaveChanges();


		}

		public void DeleteManzana(int ManzanaId)
		{

			var manzanas = dbContext.Manzana;
			var id = manzanas.FirstOrDefault(c => c.ManzanaId == ManzanaId);

			dbContext.Manzana.Remove(id);
			dbContext.SaveChanges();

		}

		public IEnumerable<Urbanizacion> GetAll()
		{
			return dbContext.Urbanizacion.OrderBy(u => u.UrbanizacionId).ToList();
		}

		public IEnumerable<Urbanizacion> GetById(int urbanizacionId)
		{
			
			return dbContext.Urbanizacion.Where(o => o.UrbanizacionId == urbanizacionId).ToList();
		}
		public IEnumerable<Manzana> GetManzanaByUrbanizacionId(int UrbanizacionId)
		{
			var sql = (from m in dbContext.Manzana
					   join u in dbContext.Urbanizacion on m.UrbanizacionId equals u.UrbanizacionId
					   where m.UrbanizacionId == UrbanizacionId
					   select m).ToList();
			return sql;


		}

		public IEnumerable<Urbanizacion> GetUrbByUrbanizacionNombre(string NombreUrbanizacion, int pageSize, int skip, out int nroTotalRegistros)
		{
 
				var sql = (from u in dbContext.Urbanizacion
						   where string.IsNullOrEmpty(NombreUrbanizacion) || u.NombreUrbanizacion.Contains(NombreUrbanizacion.ToUpper())
						   orderby u.UrbanizacionId

						   select new
						   {
							   u.UrbanizacionId,
							   u.NombreUrbanizacion,
							   u.Codigo
						   });
				nroTotalRegistros = sql.Count();
				sql = sql.Skip(skip).Take(pageSize);
				var ListFinal = (from c in sql.ToList()
								 select new Urbanizacion()
								 {
									 UrbanizacionId = c.UrbanizacionId,
									 NombreUrbanizacion = c.NombreUrbanizacion,
									 Codigo = c.Codigo
								 }).ToList();
				return ListFinal;

			
		}

		public IEnumerable<Manzana> GetUrbByManzanaNombre(string NombreManzana, int pageSize, int skip, out int nroTotalRegistros)
		{
			var sql = (from m in dbContext.Manzana
					   join u in dbContext.Urbanizacion on m.UrbanizacionId equals u.UrbanizacionId
					   where string.IsNullOrEmpty(NombreManzana) || m.NombreManzana.Contains(NombreManzana.ToUpper())
					   orderby m.ManzanaId

					   select new
					   {
						   m.ManzanaId,
						   m.UrbanizacionId,
						   m.NombreManzana,
						   u.NombreUrbanizacion
					   });
			nroTotalRegistros = sql.Count();
			sql = sql.Skip(skip).Take(pageSize);
			var ListFinal = (from c in sql.ToList()
							 select new Manzana()
							 {
								 ManzanaId = c.ManzanaId,
								 UrbanizacionId = c.UrbanizacionId,
								 NombreManzana = c.NombreManzana,
								 NombreUrbanizacion = c.NombreUrbanizacion

							 }).ToList();
			return ListFinal;

		}

		public IEnumerable<Servicio> GetAllServicios()
		{
			return dbContext.servicio.OrderBy(o => o.ServicioId).ToList();
		}

		public IEnumerable<Categoria> GetAllCategoria()
		{
			return dbContext.Categoria.OrderBy(o => o.CategoriaId).ToList();


		}

		public IEnumerable<EstadoServicio> GetListEstadoServicio()
		{
			return dbContext.EstadoServicio.OrderBy(o => o.EstadoServicioId).ToList();


		}

	}
}
