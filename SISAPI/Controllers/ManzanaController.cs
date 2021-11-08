using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISAP.CORE.Entities;
using SISAP.CORE.Interfaces;
using SISAP.INFRAESTRUCTURA;
using SISAP.INFRAESTRUCTURA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SISAP.API.Controllers
{
    [Route("api/Manzana/")]
    [ApiController]
    public class ManzanaController : ControllerBase
    {
        private SISAPDBContext dbContext;
        private readonly IManzanaService _manzanaService;

        public ManzanaController(SISAPDBContext context)
        {

            _manzanaService = new ManzanaService(context);
        }


        [HttpGet]
        [Route("/ListarManzanaId/{idManzana}")]
        public IEnumerable<Manzana> ListarManzanaId(int idManzana)
        {
            return _manzanaService.GetManzana(idManzana);
        }

        [HttpGet]
        [Route("/ListarManzanas")]
        public IEnumerable<Manzana> ListarManzanas()
        {
            return _manzanaService.GetManzanas();
        }

        [HttpPost]
        [Route("/InsertarManzana")]
        public Manzana InsertarManzana(Manzana manzana)
        {
            return _manzanaService.CreateManzana(manzana);
        }

        [HttpPost]
        [Route("/ActualizarManzana")]
        public Manzana ActualizarManzana(Manzana manzana)
        {
            return _manzanaService.UpdateManzana(manzana);
        }




    }
}
