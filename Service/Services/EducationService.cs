using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Services.Interface;
using Repository;

namespace Service.Services
{
    public class EducationService : IEducationService
    {
        private readonly AppDbContext _context;

        public EducationService()
        {
            _context = new AppDbContext();
        }


        public async Task CreateAsync(Education education)
        {
            try
            {
                if (education == null)
                {
                    throw new ArgumentNullException();
                }

                await _context.Educations.AddAsync(education);
                await _context.SaveChangesAsync();
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine("Error: data can't be null");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteAsync(int id)
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
            try
            {
                return await _context.Educations.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null; 
            }
        }

        public Task<List<Education>> GetAllWhithExpression(Func<Education, bool> predicate)
        {
            var educations = _context.Educations.Where(predicate).ToList();

            if (educations == null)
            {
                throw new Exception("No educations found.");
            }
            return Task.FromResult(_context.Educations.Where(predicate).ToList());
        }

        public async Task<Education> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Educations.FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task UpdateAsync(Education education)
        {
            try
            {
                _context.Educations.Update(education);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
               
            }
        }

              

       
    }
}
