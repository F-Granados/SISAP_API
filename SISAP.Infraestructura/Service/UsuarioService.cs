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
  public class UsuarioService : IUsuarioService
    {
        private SISAPDBContext _context;

        public UsuarioService(SISAPDBContext context)
        {
            _context = context;
        }
        public IEnumerable<Usuario> GetAll()
        {
            return _context.Usuario.OrderBy(o => o.UsuarioId).ToList();

        }
        public IEnumerable<Usuario> ListarUsuarios(int pageSize, int skip, out int nroTotalRegistros)
        {
 
                var sql = (from u in _context.Usuario
                           orderby u.UsuarioId descending
                           select new
                           {

                               u.UsuarioId,
                               u.usuario,
                               u.Nombre,
                               u.Password,
                               u.Rol,
                               u.Estado
                           });
                nroTotalRegistros = sql.Count();
                sql = sql.Skip(skip).Take(pageSize);
                var ListFinal = (from c in sql.ToList()
                                 select new Usuario()
                                 {
                                     UsuarioId = c.UsuarioId,
                                     usuario = c.usuario,
                                     Nombre = c.Nombre,
                                     Password = c.Password,
                                     Rol = c.Rol,
                                     Estado = c.Estado

                                 }).ToList();
                return ListFinal;

         
        }
        public Usuario SingIn(string user, string password)
        {
            return _context.Usuario.FirstOrDefault(u => u.usuario == user && u.Password == password);
        }

        public Usuario Save(Usuario objUsuario)
        {
            _context.Usuario.Add(objUsuario);
            _context.SaveChanges();
            return objUsuario;


        }

        public void Update(Usuario objUsuario)
        {
            _context.Usuario.Attach(objUsuario);
            _context.Entry(objUsuario).State = EntityState.Modified;
            _context.SaveChanges();

        }

        public void Delete(int UsuarioId)
        {

            var usuarios = _context.Usuario;
            var usuario = usuarios.FirstOrDefault(o => o.UsuarioId == UsuarioId);
            _context.Usuario.Remove(usuario);
            _context.SaveChanges();


        }

    }
}
