using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
    public class FacturacionController : ControllerBase
    {
        private readonly IFacturaService facturaService;
        private SISAPDBContext _context;
        public FacturacionController(SISAPDBContext context)
        {
            facturaService = new FacturaService(context);
        }

        [HttpPost]
        [Route("/ListDetalleFacturacion")]
        public JsonResult ListDetalleFacturacion(int? ClienteId, string _draw, string _start, string _length)
        {
            var draw = _draw;
            var start = _start;
            var length = _length;

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int nroTotalRegistros = 0;

            var dlecturas = facturaService.ListDetalleFacturacion(ClienteId, pageSize, skip, out nroTotalRegistros);

            var result = new { draw = draw, recordsFiltered = nroTotalRegistros, recordsTotal = nroTotalRegistros, data = dlecturas };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("/ListMainFactura")]
        public JsonResult ListMainFactura(int? Annio, int? Mes, int? UrbanizacionId, string FilterNombre, string _draw, string _start, string _length)
        {
            var draw = _draw;
            var start = _start;
            var length = _length;

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int nroTotalRegistros = 0;

            var lecturas = facturaService.ListFactura(Annio, Mes, UrbanizacionId, FilterNombre, pageSize, skip, out nroTotalRegistros);
            var result = new { draw = draw, recordsFiltered = nroTotalRegistros, recordsTotal = nroTotalRegistros, data = lecturas };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

    }
}
