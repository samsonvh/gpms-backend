using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GPMS.Backend.Services.DTOs.ResponseDTOs;

namespace GPMS.Backend.Services.Services
{
    public interface IBaseService<I, CU, L, D, F>
    where I : class
    where CU : class
    where L : class
    where D : class
    where F : class
    {
        Task<CU> Add(I inputDTO);
        Task AddList(List<I> inputDTOs, Guid? parentEntityId = null);
        Task<CU> Update(I inputDTO);
        Task UpdateList (List<I> inputDTOs);
        Task<DefaultPageResponseListingDTO<L>> GetAll(F filter);
        Task<D> Details(Guid id);
    }
}