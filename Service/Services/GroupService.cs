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
    public class GroupService : IGroupService
    {

        private readonly AppDbContext _context;

        public GroupService()
        {
            _context = new AppDbContext();
        }

        public async Task CreateAsync(Group group)
        {
            try
            {
                if (group == null)
                {
                    throw new ArgumentNullException();
                }
                await _context.Groups.AddAsync(group);
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
                var data = await _context.FindAsync<Group>(id);
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

        public async Task<List<Group>> FilterByEducationNameAsync()
        {
            return await _context.Groups.Include(m => m.Education.Name).ToListAsync();
        }

        public async Task<List<Group>> GetAllAsync()
        {
            try
            {
                return await _context.Groups.ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Group>> GetAllWithEducationIdAsync(int EducationId)
        {
            try
            {
                return await _context.Groups.Include(m => m.EducationId).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Group>();
            }
        }

        public async Task<Group> GetByIdAsync(int id)
        {
            try
            {
                return await _context.Groups.FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<List<Group>> SearchAsync(string txt)
        {
            try
            {
                return await _context.Groups.Where(e => e.Name.Contains(txt)).ToListAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Group>();
            }
        }

        public Task<List<Group>> SortWithCapacityAsync()
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(Group group)
        {
            try
            {
                _context.Groups.Update(group);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }
        }
    }
}
