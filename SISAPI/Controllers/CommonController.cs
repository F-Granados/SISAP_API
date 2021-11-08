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
    public class CommonController : ControllerBase
    {
        private readonly IUrbanizacionService _commonService;
        private readonly ICiclosService _ciclosService;
        private SISAPDBContext _context;

        public CommonController(SISAPDBContext context)
        {
            _context = context;
            _commonService = new UrbanizacionService(_context);
            _ciclosService = new CliclosService(_context);
        }

        [HttpGet]
        [Route("/ListarAnnio")]
        public JsonResult ListarAnnio()
        {
            var annio = _ciclosService.ListarAnnios();

            var resultado = annio.GroupBy(a => new { Value = a.Annio, Text = a.Annio }).
                Select(a => new { Value = a.Key.Value, Text = a.Key.Text }).OrderByDescending(o => o.Value).ToList();
            var result = resultado;
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);

        }
        [HttpGet]
        [Route("/ListarMes/{Annio}")]
        public JsonResult ListarMes(int? Annio)
        {
            var mes = _ciclosService.ListarMesByAnnio(Annio);

            var resultado = mes.GroupBy(m => new { Month = m.Mes, NombreMes = m.NombreMes }).
                Select(m => new { Value = m.Key.Month, Text = m.Key.NombreMes }).ToList();
            var result = resultado;
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
           
        }
        [HttpGet]
        [Route("/ListarUrbanizacion")]
        public JsonResult ListarUrbanizacion()
        {
            var result = _commonService.GetAll(); 
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("/ListarUrbanizacionByNombre")]
        public JsonResult ListarUrbanizacionByNombre(string NombreUrbanizacion, string _draw, string _start, string _length)
        {
            var draw = _draw;
            var start = _start;
            var length = _length;

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int nroTotalRegistros = 0;

            var urbanizacions = _commonService.GetUrbByUrbanizacionNombre(NombreUrbanizacion, pageSize, skip, out nroTotalRegistros);
            var result = new { draw = draw, recordsFiltered = nroTotalRegistros, recordsTotal = nroTotalRegistros, data = urbanizacions };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result); 
        }

        [HttpPost]
        [Route("/ListarManzanaByNombre")]
        public JsonResult ListarManzanaByNombre(string NombreManzana, string _draw, string _start, string _length)
        {
            var draw = _draw;
            var start = _start;
            var length = _length;

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int nroTotalRegistros = 0;

            var manzanas = _commonService.GetUrbByManzanaNombre(NombreManzana, pageSize, skip, out nroTotalRegistros);

            var result = new { draw = draw, recordsFiltered = nroTotalRegistros, recordsTotal = nroTotalRegistros, data = manzanas };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);

        }
        [HttpPost]
        [Route("/UpdateUrbanizacion")]
        public JsonResult UpdateUrbanizacion(Urbanizacion objUrbanizacion)
        {
            _commonService.UpdateUrbanizacion(objUrbanizacion);
            return new JsonResult(new { msg = "success" });
        }
     
        [HttpPost]
        [Route("/DeleteUrbanizacion")]
        public JsonResult DeleteUrbanizacion(int UrbanizacionId)
        {
            _commonService.DeleteUrbanizacion(UrbanizacionId);
            return new JsonResult(new { msg = "success" });
        }



        [HttpPost]
        [Route("/ListarManzana")]
        public JsonResult ListarManzana(int UrbanizacionId)
        {
            var manzanas = _commonService.GetManzanaByUrbanizacionId(UrbanizacionId);
            return new JsonResult(new { msg = "success" });
        }

        [HttpPost]
        [Route("/ListarServicios")]
        public JsonResult ListarServicios()
        {
            var servicios = _commonService.GetAllServicios();
            return new JsonResult(servicios);
        }

        [HttpGet]
        [Route("/ListarCategoria")]
        public JsonResult ListarCategoria()
        {
            var categorias = _commonService.GetAllCategoria();
            return new JsonResult(categorias);
        }
        [HttpGet]
        [Route("/ListarEstadoServicio")]
        public JsonResult ListarEstadoServicio()
        {
            var eServicio = _commonService.GetListEstadoServicio();

            return new JsonResult(eServicio);
        }

        [HttpPost]
        [Route("/SaveUrbanizacion")]
        public JsonResult SaveUrbanizacion(Urbanizacion objUrbanizacion)
        {
            _commonService.Create(objUrbanizacion);
            return new JsonResult(new { ok = "ok" });
        }

    

    }
}
