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
    public class CiclosController : ControllerBase
    {

        private readonly ICiclosService ciclosService;
        private readonly SISAPDBContext _context;
     
        public CiclosController(SISAPDBContext context)
        {
            _context = context;
            ciclosService = new CliclosService(context);
        }


        [HttpPost]
        [Route("/RegistrarCiclos")]
        public JsonResult RegistrarCiclos(Ciclos objCiclos)
        {
            ciclosService.Save(objCiclos);
            var result = new {  ok = "ok" };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }
        
        
        [HttpPut]
        [Route("/Update")]
        public JsonResult Update(Ciclos objCiclos)
        {
            ciclosService.Update(objCiclos);
            var result = new { ok = "success" };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);

        }


        [HttpPost]
        [Route("/Delete")]
        public JsonResult Delete(int CiclosId)
        {
            ciclosService.Delete(CiclosId);
            var result = new { ok = "success" };
            Response.StatusCode = StatusCodes.Status200OK;

            return new JsonResult(result);
            
        }

        [HttpPost]
        [Route("/ListarCiclos")]
        public JsonResult ListarCiclos(string draw,string start,string length)
        {
 

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int nroTotalRegistros = 0;
            var ciclos = ciclosService.ListarCiclos(pageSize, skip, out nroTotalRegistros);

            var result = new { draw = draw, recordsFiltered = nroTotalRegistros, recordsTotal = nroTotalRegistros, data = ciclos };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }
 
    }
}
