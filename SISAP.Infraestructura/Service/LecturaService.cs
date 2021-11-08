using Microsoft.EntityFrameworkCore;
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
    public  class LecturaService : ILecturaService
    {
        private SISAPDBContext dbContext;
        public LecturaService(SISAPDBContext context)
        {
            dbContext = context;
        }

		public IEnumerable<Lectura> ValidateNullRow(int? Annio, int? Mes, int? UrbanizacionId)
		{
			var sql = (from l in dbContext.Lectura
					   join c in dbContext.Cliente on l.ClienteId equals c.ClienteId
					   join u in dbContext.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
					   where (l.Annio == Annio && l.Mes == Mes && u.UrbanizacionId == UrbanizacionId && l.Actualizado == 0)
					   select l).ToList();
			return sql;
		}

		public IEnumerable<Cliente> ListLecturaMain(int? Annio, int? Mes, int? UrbanizacionId, string FilterNombre, int pageSize, int skip, out int nroTotalRegistros)
		{
			var sql = (from c in dbContext.Cliente
					   join l in dbContext.Lectura on c.ClienteId equals l.ClienteId into dcl
					   from lec in dcl.DefaultIfEmpty()
					   join u in dbContext.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
					   join m in dbContext.Manzana on c.ManzanaId equals m.ManzanaId
					   where (c.UrbanizacionId == UrbanizacionId || UrbanizacionId == null)
							&& (lec.Annio == Annio || Annio == null)
							&& (lec.Mes == Mes || Mes == null)
							&& (c.EstadoServicioId == (int)EstadoServicioMaestro.Activo)
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
						   LecturaId = lec == null ? 0 : lec.LecturaId,
						   Annio = lec == null ? 00 : lec.Annio,
						   Mes = lec == null ? 00 : lec.Mes,
						   CantidadLectura = lec == null ? 0 : lec.CantidadLectura,
						   Consumo = lec == null ? 00 : lec.Consumo,
						   Promedio = lec == null ? 00 : lec.Promedio,
						   Alerta = lec == null ? String.Empty : lec.Alerta,
						   //CantidadLecturaAntigua = lec == null ? 00 : lec.CantidadLecturaAntigua

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
									Promedio = c.Promedio,
									Alerta = c.Alerta,
									//CantidadLecturaAntigua = c.CantidadLecturaAntigua
								}).ToList();
			return ListadoFinal;
		}

		public IEnumerable<Cliente> ListarLecturasPendientesPago(string numeroMedidor, string codCliente, int pageSize, int skip, out int nroTotalRegistros)
		{
			var sql = (from l in dbContext.Lectura
					   join c in dbContext.Cliente on l.ClienteId equals c.ClienteId
					   from f in dbContext.Facturacion.Where(x => x.Annio == l.Annio && x.Mes == l.Mes && x.ClienteId == c.ClienteId)
					   join u in dbContext.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
					   join m in dbContext.Manzana on c.ManzanaId equals m.ManzanaId
					   where (c.EstadoServicioId == (int)EstadoServicioMaestro.Activo)

					   && (c.CodigoCliente == codCliente || c.NumeroMedidor == numeroMedidor)
					   && (f.EstadoPagado == 0)
					   orderby f.FacturacionId
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
						   LecturaId = l == null ? 0 : l.LecturaId,
						   Annio = l == null ? 00 : l.Annio,
						   Mes = l == null ? 00 : l.Mes,
						   CantidadLectura = l == null ? 0 : l.CantidadLectura,
						   Consumo = l == null ? 00 : l.Consumo,
						   Promedio = l == null ? 00 : l.Promedio,
						   Alerta = l == null ? String.Empty : l.Alerta,
						   //CantidadLecturaAntigua = lec == null ? 00 : lec.CantidadLecturaAntigua

						   f.Total
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
									Promedio = c.Promedio,
									Alerta = c.Alerta,
									Total = c.Total
									//CantidadLecturaAntigua = c.CantidadLecturaAntigua
								}).ToList();
			return ListadoFinal;
		}
		public void UpdateLecturaProcesada(UpdateLecturaProcess updateLecturaProcess)
		{
			var data = dbContext.Lectura.First(l => l.Annio == updateLecturaProcess.Annio && l.Mes == updateLecturaProcess.Mes && l.ClienteId == updateLecturaProcess.ClienteId);
			data.Procesado = updateLecturaProcess.Procesado;
			dbContext.SaveChanges();
		}
		public void UpdateProcessLectura(UpdateLectura updateLectura)
		{
			var data = dbContext.Lectura.First(l => l.Annio == updateLectura.Annio && l.Mes == updateLectura.Mes && l.ClienteId == updateLectura.ClienteId);

			//data.CantidadLecturaAntigua = updateLectura.CantidadLecturaActualizar;
			dbContext.SaveChanges();
		}
		public IEnumerable<Lectura> CheckIfExistLectura(int? Annio, int? Mes, int? UrbanizacionId)
		{
			var sql = (from l in dbContext.Lectura
					   join c in dbContext.Cliente on l.ClienteId equals c.ClienteId
					   join u in dbContext.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
					   where (l.Annio == Annio && (l.Mes == Mes) && u.UrbanizacionId == UrbanizacionId && l.Procesado == 1)
					   select l).ToList();
			return sql;

		}

		public IEnumerable<Lectura> ValidateNextYearUpdateLectura(int? Annio, int? Mes, int? UrbanizacionId)
		{
			var newY = Annio + 2;
			var sql = (from l in dbContext.Lectura
					   join c in dbContext.Cliente on l.ClienteId equals c.ClienteId
					   join u in dbContext.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
					   where (l.Annio > Annio && l.Annio < newY && u.UrbanizacionId == UrbanizacionId)
					   select l).ToList();
			return sql;
		}
		public IEnumerable<Lectura> ValidateValueNoNullable(int? Annio, int? Mes, int? UrbanizacionId)
		{
 
				var sql = (from l in dbContext.Lectura
						   join c in dbContext.Cliente on l.ClienteId equals c.ClienteId
						   join u in dbContext.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
						   where (l.Annio == Annio && l.Mes == Mes && u.UrbanizacionId == UrbanizacionId)
						   select l).ToList();
				return sql;
		 
		}

		public void UpdateDataExistLectura(Lectura objLectura)
		{
			

				var dat = getLecturaPr(objLectura);

				var proc = 0;
				foreach (var item in dat)
				{
					proc = item.Procesado;
				}

				objLectura.Procesado = proc;

				dbContext.Lectura.Attach(objLectura);
				dbContext.Entry(objLectura).State = EntityState.Modified;
				dbContext.SaveChanges();

			
		}
		public IEnumerable<Lectura> getLecturaPr(Lectura objLectura)
		{

				return dbContext.Lectura.Where(l => objLectura.Annio == l.Annio && objLectura.Mes == l.Mes && objLectura.ClienteId == l.ClienteId).ToList();
			
		}
		public Lectura Create(Lectura objLectura)
		{
			dbContext.Lectura.Add(objLectura);
			dbContext.SaveChanges();
			return objLectura;
		}

		public IEnumerable<Lectura> GetFirst6Data(int? ClienteId)
		{
			var top6 = dbContext.Lectura.Where(l => l.ClienteId == ClienteId).OrderByDescending(u => u.LecturaId).Take(6);
			var listadoFinal = top6.ToList();
			return listadoFinal;
		}
		public IEnumerable<Cliente> ListarClienteLectura(int? Annio, int? Mes, int? UrbanizacionId, string FilterNombre, int pageSize, int skip, out int nroTotalRegistros)
		{
 
				var sql = (from c in dbContext.Cliente
						   join l in dbContext.Lectura on c.ClienteId equals l.ClienteId into dcl
						   from lec in dcl.DefaultIfEmpty()
						   join u in dbContext.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
						   join m in dbContext.Manzana on c.ManzanaId equals m.ManzanaId
						   //join fac in dbContext.Facturacions on lec.ClienteId equals fac.ClienteId into cf
						   //from clfa in cf.DefaultIfEmpty()
						   where (c.UrbanizacionId == UrbanizacionId || null == UrbanizacionId)
								&& (lec.Annio == Annio || lec.Annio == null)
								&& (lec.Mes == Mes || lec.Mes == null)
								&& (String.IsNullOrEmpty(FilterNombre) || c.Nombre.Contains(FilterNombre))
						   orderby c.Nombre
						   select new
						   {
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
							   LecturaId = lec == null ? 0 : lec.LecturaId,
							   Annio = lec == null ? 00 : lec.Annio,
							   Mes = lec == null ? 00 : lec.Mes,
							   CantidadLectura = lec == null ? 0 : lec.CantidadLectura,
							   Consumo = lec == null ? 00 : lec.Consumo,
							   Promedio = lec == null ? 00 : lec.Promedio,
							   Alerta = lec == null ? String.Empty : lec.Alerta,
							   //CantidadLecturaAntigua = lec == null? 00 : lec.CantidadLecturaAntigua,
							   //FacturacionId = clfa == null? 0 : clfa.FacturacionId

						   });
				nroTotalRegistros = sql.Count();
				sql = sql.Skip(skip).Take(pageSize);

				var ListadoFinal = (from c in sql.ToList()
									select new Cliente()
									{
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
										Promedio = c.Promedio,
										Alerta = c.Alerta,
										//CantidadLecturaAntigua = c.CantidadLecturaAntigua,
										//FacturacionId = c.FacturacionId
									}).ToList();
				return ListadoFinal;
			
		}

		public decimal? ObtenerLecturaAnterior(int clienteId, int lecturaId)
		{
			decimal? resultado = dbContext.Lectura.Where(l => l.ClienteId == clienteId && l.LecturaId < lecturaId).Sum(c => c.Consumo);
			return resultado;
		}

		public IEnumerable<Lectura> ListarLecturaAnnioMesCliente(int annio, int mes, int clienteId)
		{
			var lectura = dbContext.Lectura.Where(l => l.ClienteId == clienteId && l.Annio == annio && l.Mes == mes);
			var listadoFinal = lectura.ToList();
			return listadoFinal;
		}
	}
}
