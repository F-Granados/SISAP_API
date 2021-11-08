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

    [Route("api/[controller]")]
    [ApiController]
    public class UrbanizacionController : ControllerBase
    {
        private readonly IUrbanizacionService _urbanizacionService;
        private SISAPDBContext _context;


        public UrbanizacionController(SISAPDBContext context)
        {
            _urbanizacionService = new UrbanizacionService(context);
        }

        [HttpGet]
        [Route("/api/Urbanizacion/ListarId")]
        public IEnumerable<Urbanizacion> ListarId(int idUrbanizacion)
        {
            return _urbanizacionService.GetById(idUrbanizacion);
        }

        [HttpGet]
        [Route("/api/Urbanizacion/ListarTodos")]
        public IEnumerable<Urbanizacion> ListarTodos()
        {
            return _urbanizacionService.GetAll();
        }

        [HttpPost]
        [Route("/api/Urbanizacion/Insertar")]
        public Urbanizacion Insertar(Urbanizacion urbanizacion)
        {
            return _urbanizacionService.Create(urbanizacion);
        }

        [HttpPost]
        [Route("/api/Urbanizacion/Actualizar")]
        public Urbanizacion Actualizar(Urbanizacion urbanizacion)
        {
            return _urbanizacionService.UpdateUrbanizacion(urbanizacion);
        }

    }
}
