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
    public class ClienteController : ControllerBase
    {
        private readonly IClienteService _clienteService;
        private SISAPDBContext _context;

        public ClienteController(SISAPDBContext context)
        {
            _context = context;
            _clienteService = new ClienteService(context);
        }
        
        [HttpPost]
        [Route("/ListarClientes")]
        public JsonResult ListarClientes(string FilterNombre, string FilterCodigo, string FilterMedidor, string drawR, string startR , string lengthR)
        {
            var draw = drawR;
            var start = startR;
            var length = lengthR;

            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int nroTotalRegistros = 0;

            var clientes = _clienteService.GetAll(FilterNombre, FilterCodigo, FilterMedidor, pageSize, skip, out nroTotalRegistros);
            var result = new { draw = draw, recordsFiltered = nroTotalRegistros, recordsTotal = nroTotalRegistros, data = clientes };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }
        
        [HttpPost]
        [Route("/GetById")]
        public IEnumerable<Cliente> GetById(int ClienteId)
        {
           return _clienteService.GetById(ClienteId);
        }

        [HttpPost]
        [Route("/RegistrarCliente")]
        public JsonResult RegistrarCliente(Cliente objCliente)
        {

            _clienteService.Create(objCliente);
            var result = new { msg = "success" };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

        [HttpPost]
        [Route("/Update")]
        public JsonResult Update(Cliente objCliente)
        {
            _clienteService.Update(objCliente);
            var result = new { msg = "success" };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

        [HttpPut]
        [Route("/Delete")]
        public JsonResult Delete(int ClienteId)
        {
            _clienteService.Delete(ClienteId);
            var result = new { msg = "success" };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }

    }
}
