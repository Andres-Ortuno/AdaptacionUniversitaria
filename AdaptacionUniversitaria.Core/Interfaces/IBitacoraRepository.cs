using System;
using System.Collections.Generic;
using System.Text;
using AdaptacionUniversitaria.Core.Entities;

namespace AdaptacionUniversitaria.Core.Interfaces
{
    public interface IBitacoraRepository
    {
        Task<IEnumerable<Bitacora>> GetAllBitacorasAsync();
        Task<Bitacora> GetBitacoraByIdAsync(int id);
        Task InsertBitacora(Bitacora bitacora) ;
        Task UpdateBitacora(Bitacora bitacora) ;
        Task DeleteBitacora(Bitacora bitacora) ;
    }
}
