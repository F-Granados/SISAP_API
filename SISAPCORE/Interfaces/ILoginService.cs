using SISAP.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.CORE.Interfaces
{
    public interface ILoginService
    {
        Usuario ValUserLogIn(string user, string password);
    }
}
