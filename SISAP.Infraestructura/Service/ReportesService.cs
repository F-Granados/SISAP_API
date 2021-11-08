using SISAP.CORE.Entities;
using SISAP.CORE.Enum;
using SISAP.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.INFRAESTRUCTURA.Service
{
  public class ReportesService: IReportesService
	{
		private SISAPDBContext _context;

		public ReportesService(SISAPDBContext context)
		{
			_context = context;
		}
		public decimal? getDeudaRuta(int? Annio, int? Mes, int? UrbanizacionId)
		{
	 
				var sql = (from c in _context.Cliente
						   join p in _context.Facturacion on c.ClienteId equals p.ClienteId
						   join u in _context.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
						   where p.Annio == Annio && p.Mes == Mes && u.UrbanizacionId == UrbanizacionId && p.EstadoPagado == (int)EstadoPay.Pendiente
						   select p).Sum(p => p.Total);

				return sql;
		 
		}
		public decimal? getDeudaDistrito(int? Annio)
		{
			var total = _context.Facturacion.Where(l => l.Annio == Annio && l.EstadoPagado == (int)EstadoPay.Pendiente).Sum(l => l.Total);
			return total;


		}
		public decimal? getIngresoAnual(int? Annio)
		{

			var total = _context.Pago.Where(l => l.PeriodoAnnio == Annio && l.EstadoPago == (int)EstadoPay.Pagado).Sum(l => l.Total);
			return total;

		}
		public decimal? getIngresoMensual(int? Annio, int? Mes)
		{
			var total = _context.Pago.Where(l => l.PeriodoAnnio == Annio && l.PeriodoMes == Mes && l.EstadoPago == (int)EstadoPay.Pagado).Sum(l => l.Total);
			return total;


		}
		public decimal? getProcessLectura(int? Annio, int? Mes)
		{
			var total = _context.Lectura.Where(l => l.Annio == Annio && l.Mes == Mes).Sum(l => l.Consumo);
			return total;


		}
		public IEnumerable<Cliente> ListReporte(int? Annio, string FilterNombre, int pageSize, int skip, out int nroTotalRegistros)
		{
			var sql = (from c in _context.Cliente
					   join srv in _context.servicio on c.ServicioId equals srv.ServicioId
					   join cat in _context.Categoria on c.CategoriaId equals cat.CategoriaId
					   join l in _context.Lectura on c.ClienteId equals l.ClienteId
					   join u in _context.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
					   join m in _context.Manzana on c.ManzanaId equals m.ManzanaId
					   join fa in _context.Facturacion on c.ClienteId equals fa.FacturacionId


					   where (fa.Annio == Annio)

							&& (l.Annio == Annio)

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

		public IEnumerable<Facturacion> GetClienteDeudor(int? ClienteId, int pageSize, int skip, out int nroTotalRegistros)
		{
 
				var sql = (from f in _context.Facturacion.Where(o => o.EstadoPagado != (int)EstadoPay.Pagado)
						   join c in _context.Cliente on f.ClienteId equals c.ClienteId
						   join serv in _context.servicio on c.ServicioId equals serv.ServicioId
						   join cat in _context.Categoria on c.CategoriaId equals cat.CategoriaId
						   where c.ClienteId == ClienteId
						   orderby f.Mes descending
						   select new
						   {
							   c.ClienteId,
							   f.FacturacionId,
							   c.Nombre,
							   c.Apellido,
							   f.Annio,
							   f.Mes,
							   cat.CategoriaId,
							   cat.TipoCategoria,
							   serv.ServicioId,
							   serv.ServicioNombre,
							   f.Total,
							   f.EstadoPagado,
							   EstadoPagoDesc = (f.EstadoPagado == 1 ? "Pagado" : "Pendiente"),

						   });
				nroTotalRegistros = sql.Count();
				sql = sql.Skip(skip).Take(pageSize);
				var ListadoFinal = (from f in sql.ToList()
									select new Facturacion()
									{
										ClienteId = f.ClienteId,
										FacturacionId = f.FacturacionId,
										Annio = f.Annio,
										Mes = f.Mes,
										TipoCategoria = f.TipoCategoria,
										ServicioNombre = f.ServicioNombre,
										Total = f.Total,
										EstadoPagado = f.EstadoPagado,
										EstadoPagoDesc = f.EstadoPagoDesc,
										Nombre = f.Nombre,
										Apellido = f.Apellido

									}).ToList();
				return ListadoFinal;
	 
		}

		public IEnumerable<Cliente> GetAllCF(int? UrbanizacionId, string FilterNombre, int pageSize, int skip, out int nroTotalRegistros)
		{
			var sql = (from c in _context.Cliente
					   join srv in _context.servicio on c.ServicioId equals srv.ServicioId
					   join cat in _context.Categoria on c.CategoriaId equals cat.CategoriaId
					   join u in _context.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
					   join fa in _context.Facturacion on c.ClienteId equals fa.FacturacionId
					   join m in _context.Manzana on c.ManzanaId equals m.ManzanaId
					   where (u.UrbanizacionId == UrbanizacionId || UrbanizacionId == null) &&
							(string.IsNullOrEmpty(FilterNombre) || (c.Nombre + " " + c.Apellido + c.NumeroMedidor + "" + c.CodigoCliente + "" + c.DNI + "" + m.NombreManzana).Contains(FilterNombre.ToUpper()))
					   orderby m.NombreManzana ascending
					   select new
					   {
						   fa.FacturacionId,
						   fa.Total,
						   fa.Mes,
						   fa.EstadoPagado,
						   c.ClienteId,
						   c.UsuarioCreacion,
						   c.CodigoCliente,
						   c.Nombre,
						   c.Apellido,
						   c.DNI,
						   c.Telefono,
						   c.Direccion,
						   c.UrbanizacionId,
						   c.ManzanaId,
						   c.Complemento,
						   c.ServicioId,
						   c.CategoriaId,
						   c.NumeroMedidor,
						   c.EstadoServicioId,
						   c.Observaciones,
						   u.NombreUrbanizacion,
						   m.NombreManzana,
						   srv.ServicioNombre,
						   cat.TipoCategoria,
						   cat.ClaseId

					   });
			nroTotalRegistros = sql.Count();
			sql = sql.Skip(skip).Take(pageSize);

			var ListadoFinal = (from c in sql.ToList()
								select new Cliente()
								{
									FacturacionId = c.FacturacionId,
									Total = c.Total,
									Mes = c.Mes,
									ClienteId = c.ClienteId,
									CodigoCliente = c.CodigoCliente,
									Nombre = c.Nombre,
									Apellido = c.Apellido,
									DNI = c.DNI,
									Telefono = c.Telefono,
									Direccion = c.Direccion,
									UrbanizacionId = c.UrbanizacionId,
									ManzanaId = c.ManzanaId,
									ServicioId = c.ServicioId,
									CategoriaId = c.CategoriaId,
									Complemento = c.Complemento,
									NumeroMedidor = c.NumeroMedidor,
									EstadoServicioId = c.EstadoServicioId,
									UrbanizacionNombre = c.NombreUrbanizacion,
									ManzanaNombre = c.NombreManzana,
									Observaciones = c.Observaciones,
									ServicioNombre = c.ServicioNombre,
									TipoCategoria = c.TipoCategoria,
									ClaseId = c.ClaseId,

								}).ToList();
			return ListadoFinal;



		}
	}
}
