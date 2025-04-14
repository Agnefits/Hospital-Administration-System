using Hospital_Administration_System.Data;
using Hospital_Administration_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Hospital_Administration_System.Services
{
    public class NurseService
    {
        private readonly ApplicationDbContext context;

        public NurseService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task AddNurseAsync(Nurse nurse)
        {
            await context.Nurses.AddAsync(nurse);
        }

        public async Task DeleteNurseAsync(int id)
        {
            var nurse = await context.Nurses.FindAsync(id);
            context.Nurses.Remove(nurse);
            await context.SaveChangesAsync();
        }

        public async Task<Nurse> GetNurseAsync(int id)
        {
            return await context.Nurses.FindAsync(id);
        }

        public async Task<IEnumerable<Nurse>> GetNursesAsync()
        {
            return await context.Nurses.ToListAsync();
        }


        public async Task UpdateNurseAsync(Nurse nurse)
        {
            context.Nurses.Update(nurse);
            await context.SaveChangesAsync();
        }
    }
}
