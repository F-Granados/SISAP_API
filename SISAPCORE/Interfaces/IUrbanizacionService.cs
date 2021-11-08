using SISAP.CORE.Entities;
using System.Collections.Generic;

namespace SISAP.CORE.Interfaces
{
	public interface IUrbanizacionService
    {
      
        IEnumerable<Manzana> GetUrbByManzanaNombre(string NombreManzana, int pageSize, int skip, out int nroTotalRegistros);
       
        void DeleteUrbanizacion(int UrbanizacionId);
        Urbanizacion UpdateUrbanizacion(Urbanizacion objUrbanizacion);
        IEnumerable<Urbanizacion> GetUrbByUrbanizacionNombre(string NombreUrbanizacion, int pageSize, int skip, out int nroTotalRegistros);
        Urbanizacion Create(Urbanizacion objUrbanizacion);
        IEnumerable<Urbanizacion> GetAll();
        IEnumerable<Manzana> GetManzanaByUrbanizacionId(int UrbanizacionId);
        IEnumerable<Servicio> GetAllServicios();
        IEnumerable<Categoria> GetAllCategoria();
        IEnumerable<EstadoServicio> GetListEstadoServicio();
        IEnumerable<Urbanizacion> GetById(int urbanizacionId);
    }
}
