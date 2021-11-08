using SISAP.CORE.Entities;
using SISAP.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.INFRAESTRUCTURA.Service
{
    public class LoginService :ILoginService
    {
        private SISAPDBContext dbContext;

        public LoginService(SISAPDBContext context)
        {
            dbContext = context;
        }
        public Usuario ValUserLogIn(string user, string password)
        {
            return dbContext.Usuario.FirstOrDefault(o => o.usuario.Contains(user) && o.Password.Contains(password));
        }
        public bool LogOut()
        {
            return true;
        }
    }
}
