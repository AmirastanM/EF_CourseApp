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

        public async Task<List<Education>> GetAllWithGroupsAsync()
        {
            try
            {
                return await _context.Educations.Include(g => g.Groups).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);               
                return new List<Education>();
            }
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

        public async Task<List<Education>> SearchAsync(string txt)
        {
            try
            {
                return await _context.Educations.Where(e => e.Name.Contains(txt)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);                
                return new List<Education>();
            }
        }

        //public async Task<List<Education>> SortWhithCreateDateAsync(string text)
        //{
        //    return await _context.Educations.SortWhithCreateDateAsync(text);
        //}

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
