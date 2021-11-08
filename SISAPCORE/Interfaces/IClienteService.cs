using SISAP.CORE.Entities;
using System.Collections.Generic;

namespace SISAP.CORE.Interfaces
{
	public interface IClienteService
    {
        IEnumerable<Cliente> GetAll(string FilterNombre, string FilterCodigo, string FilterMedidor, int pageSize, int skip, out int nroTotalRegistros);
        IEnumerable<Cliente> GetById(int ClienteId);
        Cliente Create(Cliente cliente);
        void Update(Cliente cliente);
        void Delete(int ClienteId);
    }
}


