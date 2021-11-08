using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SISAP.CORE.Entities;
using SISAP.CORE.Enum;
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
    public class LecturaController : ControllerBase
    {
        private readonly ILecturaService _lecturaService;
        private readonly ICiclosService _ciclosService;
        private readonly ITarifarioService _tarifarioService;
        private readonly IClienteService _clienteService;
        private readonly IFacturaService _facturaService;
        private SISAPDBContext _context;
        public LecturaController(SISAPDBContext context)
        {
            _lecturaService = new LecturaService(context);
            _ciclosService = new CliclosService(context);
            _tarifarioService = new TarifarioService(context);
            _clienteService = new ClienteService(context);
            _facturaService = new FacturaService(context);
        }

        [HttpPost]
        [Route("/ListLecturaMain")]
        public JsonResult ListLecturaMain(int? Annio, int? Mes, int? UrbanizacionId, string FilterNombre, string _draw, string _start, string _length)
        {
            var draw = _draw;
            var start = _start;
            var length = _length;
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int nroTotalRegistros = 0;

            var lecturas = _lecturaService.ListLecturaMain(Annio, Mes, UrbanizacionId, FilterNombre, pageSize, skip, out nroTotalRegistros);
            decimal? cantidadLectura = 0;
            foreach (var item in lecturas)
            {
                cantidadLectura = _lecturaService.ObtenerLecturaAnterior(item.ClienteId, item.LecturaId);
                item.CantidadLecturaAntigua = cantidadLectura == null ? 0 : cantidadLectura;
            }

            var result = new { draw = draw, recordsFiltered = nroTotalRegistros, recordsTotal = nroTotalRegistros, data = lecturas };
            Response.StatusCode = StatusCodes.Status200OK;
            return new JsonResult(result);
        }


        [HttpPost]
        [Route("/UpdateProcessLectura")]
        public JsonResult UpdateProcessLectura(ValidateLectura objValidate)
        {
            int Annio = objValidate.Annio;
            int Mes = objValidate.Mes;
            int UrbanizacionId = objValidate.UrbanizacionId;

            int newY = Annio;
            int newM = Mes + 1;

            var dataNextYar = _lecturaService.ValidateNextYearUpdateLectura(Annio, Mes, UrbanizacionId);
            var existNextYear = dataNextYar.Count();
            var dataFromLecturaActually = _lecturaService.ValidateValueNoNullable(Annio, Mes, UrbanizacionId);
            if (existNextYear > 0)
            {
                newM = newM - 12;
                newY = Annio + 1;
            }
            decimal? value = 0;
            int ClienteId = 0;

            foreach (var item in dataFromLecturaActually)
            {
                value = item.CantidadLectura;
                ClienteId = item.ClienteId;
                var dataCliente = _clienteService.GetById(item.ClienteId);
                if (dataCliente.Count() > 0)
                {

                }
                var updateLectura = new UpdateLectura()
                {
                    Annio = newY,
                    Mes = newM,
                    ClienteId = ClienteId,

                    CantidadLecturaActualizar = value
                };

                _lecturaService.UpdateProcessLectura(updateLectura);
            }

            return new JsonResult(new { mensaje = "success" });

        }
        [HttpPost]
        [Route("/ExistLectura")]
        public JsonResult ExistLectura(ValidateLectura objValidate)
        {
            int Annio = objValidate.Annio;
            int Mes = objValidate.Mes;
            int UrbanizacionId = objValidate.UrbanizacionId;

            var dato = _lecturaService.CheckIfExistLectura(Annio, Mes, UrbanizacionId);
            var nroRegistros = dato.Count();

            return new JsonResult(new { mensaje = nroRegistros });

        }

        [HttpPost]
        [Route("/ProcesarLectura")]
        public JsonResult ProcesarLectura(ValidateLectura objValidate)
        {


            int Annio = objValidate.Annio;
            int Mes = objValidate.Mes;
            int UrbanizacionId = objValidate.UrbanizacionId;

            var output = "";
            var datos = _ciclosService.EnableToNextPrecess(Annio, Mes);
            var datosnullable = _lecturaService.ValidateNullRow(Annio, Mes, UrbanizacionId);


            if (datos.Count() == 0)
            {
                output = "10";
            }
            else if (datosnullable.Count() != 0)
            {
                output = "15";
            }
            else
            {
                int nextM = 0;
                int nextY = 0;
                decimal? cantAntigua = 0;
                var netxtYM = _ciclosService.EnableToNextPrecess(Annio, Mes);
                foreach (var item in netxtYM)
                {
                    nextY = item.Annio;
                    nextM = item.Mes;
                }
                int? ClienteId = 0;
                var objLectura = _lecturaService.ValidateValueNoNullable(Annio, Mes, UrbanizacionId);
                foreach (var item in objLectura)
                {
                    var updateLecturaProcess = new UpdateLecturaProcess()
                    {
                        Annio = Annio,
                        Mes = Mes,
                        ClienteId = item.ClienteId,
                        Procesado = 1
                    };
                    _lecturaService.UpdateLecturaProcesada(updateLecturaProcess);

                    cantAntigua = item.CantidadLectura;

                    item.Annio = nextY;
                    item.Mes = nextM;
                    item.CantidadLectura = 0;
                    item.Consumo = 0;
                    item.Promedio = 0;
                    item.FechaRegistro = DateTime.Now;
                    item.Actualizado = 0;
                    _lecturaService.Create(item);

                }
                output = "success";
            }

            return new JsonResult(new { msg = output });
        }

        [HttpPost]
        [Route("/ValidateEnableNextMonth")]
        public JsonResult ValidateEnableNextMonth(int? Annio, int? Mes)
        {

            var datos = _ciclosService.EnableToNextPrecess(Annio, Mes);
            var count = datos.Count();
            return new JsonResult(new { mensaje = count });
        }

        //[HttpPost]
        //[Route("/ValidateNullableRow")]
        //public JsonResult ValidateNullableRow(ValidateLectura objValidate)
        //{
        //    int Annio = objValidate.Annio;
        //    int Mes = objValidate.Mes;
        //    int UrbanizacionId = objValidate.UrbanizacionId;
        //    var datos = _lecturaService.ValidateValueNoNullable(Annio, Mes, UrbanizacionId);
        //    var msg = 0;
        //    foreach (var item in datos)
        //    {
        //        if (item.CantidadLectura == 0)
        //        {
        //            msg = 1;
        //        }
        //    }
        //    return new JsonResult(new { mensaje = msg });
        //}

        //[HttpPost]
        //[Route("/UpdateDataExistLectura")]
        //public JsonResult UpdateDataExistLectura(Lectura objLectura)
        //{
        //    Cuando procesa la lectura
        //    string error = "";
        //    string mensaje = "";
        //    int ClienteId = objLectura.ClienteId;
        //    var top6 = _lecturaService.GetFirst6Data(ClienteId);
        //    var cliente = _clienteService.GetById(ClienteId);
        //    var top6Count = top6.Count();

        //    if (objLectura.CantidadLectura == 0)
        //        objLectura.CantidadLectura = objLectura.CantidadLecturaAntigua;

        //    var consumo = objLectura.CantidadLectura - objLectura.CantidadLecturaAntigua;
        //    if (consumo < 0)
        //    {
        //        consumo = cliente.First().CapacidadMaxima - objLectura.CantidadLecturaAntigua + objLectura.CantidadLectura;
        //    }

        //    objLectura.Consumo = consumo;
        //    objLectura.FechaRegistro = DateTime.Now;
        //    decimal? c = 0;

        //    if (top6Count > 5)
        //    {
        //        foreach (var items in top6)
        //        {
        //            var value = items.Consumo;
        //            c += value;
        //        }

        //        objLectura.Promedio = (c / 6);
        //    }
        //    var facturaExistente = _facturaService.ValidateIfExists(objLectura.Annio, objLectura.Mes, ClienteId);
        //    if (facturaExistente != null && facturaExistente.Count() == 0)
        //    {
        //        objLectura.Consumo = consumo;
        //        objLectura.FechaRegistro = DateTime.Now;
        //        _lecturaService.UpdateDataExistLectura(objLectura);
        //        int CategoriaId = cliente.First().CategoriaId;
        //        int ServicioId = cliente.First().ServicioId;
        //        var tarifarioItem = _tarifarioService.GetDataTarifario(CategoriaId, objLectura.Consumo);


        //        if (ServicioId == (int)Servicios.AguaAlcantarillado)
        //        {
        //            var objFacturacion = new Facturacion()
        //            {
        //                ClienteId = ClienteId,
        //                Annio = objLectura.Annio,
        //                Mes = objLectura.Mes,
        //                SubTotal = (objLectura.Consumo * tarifarioItem.TarifaAgua + objLectura.Consumo * tarifarioItem.TarifaAlcantarillado) + tarifarioItem.CargoFijo,
        //                Total = (objLectura.Consumo * tarifarioItem.TarifaAgua + objLectura.Consumo * tarifarioItem.TarifaAlcantarillado) + tarifarioItem.CargoFijo,
        //                EstadoPagado = (int)EstadoPay.Pendiente

        //            };
        //            _facturaService.Create(objFacturacion);
        //        }
        //        if (ServicioId == (int)Servicios.Agua)
        //        {
        //            var objFacturacion = new Facturacion()
        //            {
        //                ClienteId = ClienteId,
        //                Annio = objLectura.Annio,
        //                Mes = objLectura.Mes,
        //                SubTotal = (objLectura.Consumo * tarifarioItem.TarifaAgua) + tarifarioItem.CargoFijo,
        //                Total = (objLectura.Consumo * tarifarioItem.TarifaAgua) + tarifarioItem.CargoFijo,
        //                EstadoPagado = (int)EstadoPay.Pendiente

        //            };
        //            _facturaService.Create(objFacturacion);
        //        }
        //        error = "00";
        //        mensaje = "Se Guardo correctamente";
        //    }
        //    else
        //    {


        //        if (facturaExistente.First().EstadoPagado != 1)
        //        {
        //            DateTime fechaActual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 01);
        //            DateTime fechaFactura = new DateTime(facturaExistente.First().Annio, facturaExistente.First().Mes, 01);
        //            decimal restaFechas = MonthDifference(fechaActual, fechaFactura);
        //            if (restaFechas <= 5)
        //            {

        //                _lecturaService.UpdateDataExistLectura(objLectura);

        //                var FacturacionId = facturaExistente.First().FacturacionId;

        //                int CategoriaId = cliente.First().CategoriaId;
        //                int ServicioId = cliente.First().ServicioId;

        //                Reglas de negocio
        //                var tarifarioItem = _tarifarioService.GetDataTarifario(CategoriaId, objLectura.Consumo);

        //                if (ServicioId == (int)Servicios.AguaAlcantarillado)
        //                {
        //                    var objFacturacion = new Facturacion()
        //                    {
        //                        FacturacionId = FacturacionId,
        //                        ClienteId = ClienteId,
        //                        Annio = objLectura.Annio,
        //                        Mes = objLectura.Mes,
        //                        SubTotal = (objLectura.Consumo * tarifarioItem.TarifaAgua + objLectura.Consumo * tarifarioItem.TarifaAlcantarillado) + tarifarioItem.CargoFijo,
        //                        Total = (objLectura.Consumo * tarifarioItem.TarifaAgua + objLectura.Consumo * tarifarioItem.TarifaAlcantarillado) + tarifarioItem.CargoFijo,
        //                        EstadoPagado = (int)EstadoPay.Pendiente

        //                    };
        //                    _facturaService.UpdateDataExistFactura(objFacturacion);
        //                }
        //                if (ServicioId == (int)Servicios.Agua)
        //                {
        //                    var objFacturacion = new Facturacion()
        //                    {
        //                        FacturacionId = FacturacionId,
        //                        ClienteId = ClienteId,
        //                        Annio = objLectura.Annio,
        //                        Mes = objLectura.Mes,
        //                        SubTotal = (objLectura.Consumo * tarifarioItem.TarifaAgua) + tarifarioItem.CargoFijo,
        //                        Total = (objLectura.Consumo * tarifarioItem.TarifaAgua) + tarifarioItem.CargoFijo,
        //                        EstadoPagado = (int)EstadoPay.Pendiente

        //                    };
        //                    _facturaService.UpdateDataExistFactura(objFacturacion);
        //                }

        //                error = "00";
        //                mensaje = "Se Modifico correctamente";
        //            }
        //            else
        //            {
        //                error = "01";
        //                mensaje = "No se puede modificar una factura con mas de 5 meses de antiguedad";
        //            }
        //        }
        //        else
        //        {
        //            error = "01";
        //            mensaje = "No se puede modificar una factura en estado pagado";
        //        }
        //    }



        //    return new JsonResult(new { msg = mensaje, errorCode = error });


        //}

        //public decimal MonthDifference(DateTime FechaFin, DateTime FechaInicio)
        //{
        //    return Math.Abs((FechaFin.Month - FechaInicio.Month) + 12 * (FechaFin.Year - FechaInicio.Year));

        //}

        //[HttpPost]
        //[Route("/SaveFirstDataLectura")]
        //public JsonResult SaveFirstDataLectura(Lectura objLectura)
        //{

        //    int ClienteId = objLectura.ClienteId;
        //    var top6 = _lecturaService.GetFirst6Data(ClienteId);
        //    var top6Count = top6.Count();
        //    var fecchaRegistro = DateTime.Now;
        //    var consumo = objLectura.CantidadLectura - objLectura.CantidadLecturaAntigua;
        //    objLectura.Consumo = consumo;
        //    objLectura.FechaRegistro = fecchaRegistro;
        //    decimal? c = 0;
        //    if (top6Count > 5)
        //    {
        //        foreach (var items in top6)
        //        {
        //            var value = items.Consumo;
        //            c += value;
        //        }

        //        objLectura.Promedio = (c / 6);
        //    }

        //    _lecturaService.Create(objLectura);

        //    var cliente = _clienteService.GetById(ClienteId);

        //    int CategoriaId = cliente.First().CategoriaId;
        //    int ServicioId = cliente.First().ServicioId;


        //    var tarifarioItem = _tarifarioService.GetDataTarifario(CategoriaId, objLectura.Consumo);


        //    if (ServicioId == (int)Servicios.AguaAlcantarillado)
        //    {
        //        var objFacturacion = new Facturacion()
        //        {
        //            ClienteId = ClienteId,
        //            Annio = objLectura.Annio,
        //            Mes = objLectura.Mes,
        //            SubTotal = (objLectura.Consumo * tarifarioItem.TarifaAgua + objLectura.Consumo * tarifarioItem.TarifaAlcantarillado) + tarifarioItem.CargoFijo,
        //            Total = (objLectura.Consumo * tarifarioItem.TarifaAgua + objLectura.Consumo * tarifarioItem.TarifaAlcantarillado) + tarifarioItem.CargoFijo,
        //            EstadoPagado = (int)EstadoPay.Pendiente

        //        };
        //        _facturaService.Create(objFacturacion);
        //    }
        //    if (ServicioId == (int)Servicios.Agua)
        //    {
        //        var objFacturacion = new Facturacion()
        //        {
        //            ClienteId = ClienteId,
        //            Annio = objLectura.Annio,
        //            Mes = objLectura.Mes,
        //            SubTotal = (objLectura.Consumo * tarifarioItem.TarifaAgua) + tarifarioItem.CargoFijo,
        //            Total = (objLectura.Consumo * tarifarioItem.TarifaAgua) + tarifarioItem.CargoFijo,
        //            EstadoPagado = (int)EstadoPay.Pendiente

        //        };
        //        _facturaService.Create(objFacturacion);
        //    }
        //    return new JsonResult(new { msg = "success" });

        //}

        //[HttpPost]
        //[Route("/ListaLecturaByFilters")]
        //public JsonResult ListaLecturaByFilters(int? Annio, int? Mes, int? UrbanizacionId, string FilterNombre, string _draw, string _start, string _length)
        //{
        //    var draw = _draw;
        //    var start = _start;
        //    var length = _length;

        //    int pageSize = length != null ? Convert.ToInt32(length) : 0;
        //    int skip = start != null ? Convert.ToInt32(start) : 0;
        //    int nroTotalRegistros = 0;

        //    var lecturas = _lecturaService.ListarClienteLectura(Annio, Mes, UrbanizacionId, FilterNombre, pageSize, skip, out nroTotalRegistros);
        //    decimal? cantidadLectura = 0;
        //    foreach (var item in lecturas)
        //    {
        //        cantidadLectura = _lecturaService.ObtenerLecturaAnterior(item.ClienteId, item.LecturaId);
        //        item.CantidadLecturaAntigua = cantidadLectura == null ? 0 : cantidadLectura;
        //    }
        //    return new JsonResult(new { draw = draw, recordsFiltered = nroTotalRegistros, recordsTotal = nroTotalRegistros, data = lecturas });
        //}


    }
}
