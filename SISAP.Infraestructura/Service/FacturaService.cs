﻿using Microsoft.EntityFrameworkCore;
using SISAP.CORE.Entities;
using SISAP.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.INFRAESTRUCTURA.Service
{
   public class FacturaService :IFacturaService
    {
		private SISAPDBContext dbContext;

		public FacturaService(SISAPDBContext context)
		{
			dbContext = context;
		}
		public IEnumerable<Facturacion> ListDetalleFacturacion(int? ClienteId, int pageSize, int skip, out int nroTotalRegistros)
		{
			var sql = (from f in dbContext.Facturacion

					   where f.ClienteId == ClienteId
					   orderby f.Mes descending
					   select new
					   {
						   f.FacturacionId,
						   f.ClienteId,
						   f.Annio,
						   f.Mes,
						   f.SubTotal,
						   f.Total,
						   f.NroBoleta
					   });
			nroTotalRegistros = sql.Count();
			sql = sql.Skip(skip).Take(pageSize);

			var ListadoFinal = (from c in sql.ToList()
								select new Facturacion()
								{
									FacturacionId = c.FacturacionId,
									ClienteId = c.ClienteId,
									Annio = c.Annio,
									Mes = c.Mes,
									SubTotal = c.SubTotal,
									Total = c.Total,
									NroBoleta = c.NroBoleta

								}).ToList();
			return ListadoFinal;
		}

		public IEnumerable<Facturacion> ValidateIfExists(int? Annio, int? Mes, int? ClienteId)
		{
			return dbContext.Facturacion.Where(f => f.Annio == Annio && f.Mes == Mes && f.ClienteId == ClienteId).ToList();

		}
		public IEnumerable<Cliente> ListFactura(int? Annio, int? Mes, int? UrbanizacionId, string FilterNombre, int pageSize, int skip, out int nroTotalRegistros)
		{
			var sql = (from c in dbContext.Cliente
					   join srv in dbContext.servicio on c.ServicioId equals srv.ServicioId
					   join cat in dbContext.Categoria on c.CategoriaId equals cat.CategoriaId
					   join l in dbContext.Lectura on c.ClienteId equals l.ClienteId
					   join u in dbContext.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
					   join m in dbContext.Manzana on c.ManzanaId equals m.ManzanaId
					   join fa in dbContext.Facturacion on c.ClienteId equals fa.ClienteId

					   where (c.UrbanizacionId == UrbanizacionId)
							&& (fa.Annio == Annio)
							&& (fa.Mes == Mes)
							&& (l.Annio == Annio)
							&& (l.Mes == Mes)
							&& (String.IsNullOrEmpty(FilterNombre) || c.Nombre.Contains(FilterNombre))
					   orderby c.Nombre
					   select new
					   {
						   c.CodigoCliente,
						   c.ClienteId,
						   c.Nombre,
						   c.Apellido,
						   c.Telefono,
						   c.Direccion,
						   c.UrbanizacionId,
						   c.ManzanaId,
						   c.NumeroMedidor,
						   c.Complemento,
						   u.NombreUrbanizacion,
						   m.NombreManzana,
						   l.LecturaId,
						   l.Annio,
						   l.Mes,
						   l.CantidadLectura,
						   l.Consumo,
						   //l.CantidadLecturaAntigua,
						   fa.FacturacionId,
						   fa.Total,
						   srv.ServicioId,
						   srv.ServicioNombre,
						   cat.CategoriaId,
						   cat.TipoCategoria,
						   cat.ClaseId
						   /*
						   LecturaId = lec == null ? 0 : lec.LecturaId,
						   Annio = lec == null ? 00 : lec.Annio,
						   Mes = lec == null ? 00 : lec.Mes,
						   CantidadLectura = lec == null ? 0 : lec.CantidadLectura,
						   Consumo = lec == null ? 00 : lec.Consumo,
						   Promedio = lec == null ? 00 : lec.Promedio,
						   Alerta = lec == null ? String.Empty : lec.Alerta,
						   CantidadLecturaAntigua = lec == null ? 00 : lec.CantidadLecturaAntigua
						   */
					   });
			nroTotalRegistros = sql.Count();
			sql = sql.Skip(skip).Take(pageSize);

			var ListadoFinal = (from c in sql.ToList()
								select new Cliente()
								{
									CodigoCliente = c.CodigoCliente,
									ClienteId = c.ClienteId,
									Nombre = c.Nombre,
									Apellido = c.Apellido,
									Telefono = c.Telefono,
									Direccion = c.Direccion,
									UrbanizacionId = c.UrbanizacionId,
									ManzanaId = c.ManzanaId,
									NumeroMedidor = c.NumeroMedidor,
									UrbanizacionNombre = c.NombreUrbanizacion,
									Complemento = c.Complemento,
									ManzanaNombre = c.NombreManzana,
									LecturaId = c.LecturaId,
									Annio = c.Annio,
									Mes = c.Mes,
									CantidadLectura = c.CantidadLectura,
									Consumo = c.Consumo,
									//CantidadLecturaAntigua = c.CantidadLecturaAntigua,
									FacturacionId = c.FacturacionId,
									Total = c.Total,
									ServicioId = c.ServicioId,
									ServicioNombre = c.ServicioNombre,
									CategoriaId = c.CategoriaId,
									TipoCategoria = c.TipoCategoria,
									ClaseId = c.ClaseId

								}).ToList();
			return ListadoFinal;
		}

		public void UpdateDataExistFactura(Facturacion objFacturacion)
		{
			dbContext.Facturacion.Attach(objFacturacion);
			dbContext.Entry(objFacturacion).State = EntityState.Modified;
			dbContext.SaveChanges();
		}
		public Facturacion Create(Facturacion objFacturacion)
		{
			dbContext.Facturacion.Add(objFacturacion);
			dbContext.SaveChanges();
			return objFacturacion;

		}
	}
}
