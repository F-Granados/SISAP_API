using SISAP.CORE.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.CORE.Interfaces
{
    public interface IManzanaService
    {
        Manzana DeleteManzana(int ManzanaId);
        Manzana UpdateManzana(Manzana objManzana);
        Manzana CreateManzana(Manzana objManzana);

        IEnumerable<Manzana> GetManzanas();
        IEnumerable<Manzana> GetManzana(int manzanaId);
    }
}
