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
    public class ClienteService :IClienteService
    {
        private SISAPDBContext _context;

        public ClienteService(SISAPDBContext context)
        {
            _context = context;
        }


        public IEnumerable<Cliente> GetAll(string FilterNombre, string FilterCodigo, string FilterMedidor, int pageSize, int skip, out int nroTotalRegistros)
        {
            var sql = (from c in _context.Cliente
                       join s in _context.EstadoServicio on c.EstadoServicioId equals s.EstadoServicioId
                       join u in _context.Urbanizacion on c.UrbanizacionId equals u.UrbanizacionId
                       join m in _context.Manzana on c.ManzanaId equals m.ManzanaId
                       where
                            (string.IsNullOrEmpty(FilterNombre) || (c.Nombre + " " + c.Apellido + c.NumeroMedidor + "" + c.CodigoCliente).Contains(FilterNombre.ToUpper()))
                       orderby c.Nombre ascending
                       select new
                       {
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
                           s.EstadoDescripcion,
                           c.Observaciones,
                           u.NombreUrbanizacion,
                           m.NombreManzana,
                           c.CapacidadMaxima

                       });
            nroTotalRegistros = sql.Count();
            sql = sql.Skip(skip).Take(pageSize);

            var ListadoFinal = (from c in sql.ToList()
                                select new Cliente()
                                {
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
                                    EstadoServicio = new EstadoServicio()
                                    {
                                        EstadoServicioId = c.EstadoServicioId,
                                        EstadoDescripcion = c.EstadoDescripcion
                                    },
                                    CapacidadMaxima = c.CapacidadMaxima


                                }).ToList();
            return ListadoFinal;

        }

        public IEnumerable<Cliente> GetById(int ClienteId)
        {
            return _context.Cliente.Where(o => o.ClienteId == ClienteId).ToList();
        }
        public Cliente Create(Cliente cliente)
        {
            _context.Cliente.Add(cliente);
            _context.SaveChanges();
            return cliente;

        }
        public void Update(Cliente cliente)
        {

            _context.Cliente.Attach(cliente);
            _context.Entry(cliente).State = EntityState.Modified;
            _context.SaveChanges();
            
        }
        public void Delete(int ClienteId)
        {

            var clientes = _context.Cliente;
            var obj = clientes.FirstOrDefault(c => c.ClienteId == ClienteId);

            _context.Cliente.Remove(obj);
            _context.SaveChanges();

        }

    }
}
