using SISAP.CORE.Entities;
using SISAP.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SISAP.INFRAESTRUCTURA.Service
{
   public class ManzanaService : IManzanaService
    {
        private SISAPDBContext dbContext;

        public ManzanaService(SISAPDBContext context)
        {
            dbContext = context;
        }

        public Manzana CreateManzana(Manzana objManzana)
        {

            dbContext.Manzana.Add(objManzana);
            dbContext.SaveChanges();
            return objManzana;
        }

        public Manzana DeleteManzana(int ManzanaId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Manzana> GetManzana(int manzanaId)
        {
            return dbContext.Manzana.Where(u => u.ManzanaId== manzanaId).ToList();
        }

        public IEnumerable<Manzana> GetManzanas()
        {
            return dbContext.Manzana.OrderBy(u => u.ManzanaId).ToList();
        }

        public Manzana UpdateManzana(Manzana objManzana)
        {
            throw new NotImplementedException();
        }
    }
}
