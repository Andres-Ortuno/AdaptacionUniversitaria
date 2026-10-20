using AdaptacionUniversitaria.Core.Entities;
using AdaptacionUniversitaria.Core.Interfaces;
using AdaptacionUniversitaria.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace AdaptacionUniversitaria.Infrastructure.Repositories
{
    public class BitacoraRepository : IBitacoraRepository
    {
        private readonly AdaptacionUniversitariaContext _adaptacionUniversitariaContext;

        public BitacoraRepository(AdaptacionUniversitariaContext adaptacionUniversitariaContext)
        {
            _adaptacionUniversitariaContext = adaptacionUniversitariaContext;
        }

        public async Task<IEnumerable<Bitacora>> GetAllBitacorasAsync()
        {
            var bitacoras = await _adaptacionUniversitariaContext.Bitacoras.ToListAsync();
            return bitacoras;
        }

        public async Task<Bitacora> GetBitacoraByIdAsync(int id)
        {
            var bitacora = await _adaptacionUniversitariaContext.Bitacoras.FirstOrDefaultAsync(x => x.Id == id);
            return bitacora;
        }

        public async Task InsertBitacora(Bitacora bitacora)
        {
            _adaptacionUniversitariaContext.Bitacoras.Add(bitacora);
            await _adaptacionUniversitariaContext.SaveChangesAsync();
        }

        public async Task UpdateBitacora(Bitacora bitacora)
        {
            _adaptacionUniversitariaContext.Bitacoras.Update(bitacora);
            await _adaptacionUniversitariaContext.SaveChangesAsync();
        }

        public async Task DeleteBitacora(Bitacora bitacora)
        {
            _adaptacionUniversitariaContext.Bitacoras.Remove(bitacora);
            await _adaptacionUniversitariaContext.SaveChangesAsync();
        }
    }
}
