using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISAP.CORE.Entities;
using SISAP.CORE.Interfaces;
using SISAP.INFRAESTRUCTURA;
using SISAP.INFRAESTRUCTURA.Service;
using System;
using System.Collections.Generic;
using System.Linq;
 

namespace SISAP.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService categoriaService;
        private readonly SISAPDBContext _context;

        public CategoriasController(SISAPDBContext context)
        {
            _context = context;
            categoriaService = new CategoriaService(context);
        }

        [HttpGet]
      
        public IEnumerable<Categoria> Get()
        {
          
            return categoriaService.GetAll();
        }

        [HttpPost]
        public JsonResult RegistrarCategoria(Categoria categoria)
        {
            var result = new { msg = categoriaService.Insertar(categoria)};   
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);

        }
    }
}
