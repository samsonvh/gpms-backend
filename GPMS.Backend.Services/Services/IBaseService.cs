using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GPMS.Backend.Services.Services
{
    public interface IBaseService<I, CU, L, D>
    where I : class
    where CU : class
    where L : class
    where D : class
    {
        Task<CU> Add(I inputDTO);
        Task AddList(List<I> inputDTOs, Guid? parentEntityId = null);
        Task<CU> Update(I inputDTO);
        Task UpdateList (List<I> inputDTOs);
        Task<List<L>> GetAll();
        Task<D> Details(Guid id);
    }
}