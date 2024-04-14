using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.Interface;
using Repository;
using Domain.Common;
using static System.Net.Mime.MediaTypeNames;

namespace Service.Services
{
    public class EducationService : IEducationService
    {
        private readonly AppDbContext _context;

        public EducationService()
        {
            _context = new AppDbContext();
        }

        public async Task CreateEducationAsync(Education education)
        {
            await _context.Educations.AddAsync(education);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteEducationAsync(int id)
        {
            try
            {
                var data = await _context.FindAsync<Education>(id);
                if (data != null)
                {
                    _context.Remove(data);
                    await _context.SaveChangesAsync();
                    Console.WriteLine("Successfully deleted");
                }
                else
                {
                    Console.WriteLine("Id not found");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task<List<Education>> GetAllAsync()
        {

            return await _context.Educations.ToListAsync();

        }

        public async Task<List<Education>> GetAllWithGroupsAsync()
        {

            return await _context.Educations.Include(g => g.Groups).ToListAsync();

        }

        public async Task<Education> GetByIdAsync(int id)
        {
            return await _context.Educations.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<List<Education>> SearchAsync(string txt)
        {
            return await _context.Educations.Where(e => e.Name.Contains(txt)).ToListAsync();
        }

        public async Task<List<Education>> SortWhithCreateDateAsync(string sortType)
        {
            return await _context.Educations.OrderBy(m => m.Date).ToListAsync();
        }

        public async Task UpdateEducationAsync(Education education)
        {
            _context.Educations.Update(education);
            await _context.SaveChangesAsync();
        }
    }
}