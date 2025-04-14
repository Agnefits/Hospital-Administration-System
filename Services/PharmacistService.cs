using Hospital_Administration_System.Data;
using Hospital_Administration_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Administration_System.Services
{
    public class PharmacistService
    {
        private readonly ApplicationDbContext context;

        public PharmacistService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddPharmacistAsync(Pharmacist pharmacist)
        {
            await context.Pharmacists.AddAsync(pharmacist);
        }

        public async Task DeletePharmacistAsync(int id)
        {
            var pharmacist = await context.Pharmacists.FindAsync(id);
            context.Pharmacists.Remove(pharmacist);
            await context.SaveChangesAsync();
        }

        public async Task<Pharmacist> GetPharmacistAsync(int id)
        {
            return await context.Pharmacists.FindAsync(id);
        }

        public async Task<IEnumerable<Pharmacist>> GetPharmacistsAsync()
        {
            return await context.Pharmacists.ToListAsync();
        }


        public async Task UpdatePharmacistAsync(Pharmacist pharmacist)
        {
            context.Pharmacists.Update(pharmacist);
            await context.SaveChangesAsync();
        }
    }
}
