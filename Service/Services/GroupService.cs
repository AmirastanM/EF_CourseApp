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
            if (group == null)
            {
                throw new ArgumentNullException();
            }
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
        }
          

        public async Task DeleteAsync(int id)
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

        public async Task<List<Group>> FilterByEducationNameAsync()
        {
            return await _context.Groups.Include(m => m.Education.Name).ToListAsync();
        }

        public async Task<List<Group>> GetAllAsync()
        {
           
            return await _context.Groups.ToListAsync();     
         
        }

        public async Task<List<Group>> GetAllWithEducationIdAsync(int EducationId)
        {           
                return await _context.Groups.Include(m => m.EducationId).ToListAsync();            
        }

        public async Task<Group> GetByIdAsync(int id)
        {
                return await _context.Groups.FirstOrDefaultAsync(m => m.Id == id);            
        }

        public async Task<List<Group>> SearchAsync(string txt)
        {           
                return await _context.Groups.Where(e => e.Name.Contains(txt)).ToListAsync();            
        }

        public async Task<List<Group>> SortWithCapacityAsync(string sort)
        {            
                return await _context.Groups.OrderBy(g => g.Capacity).ToListAsync();          
        }

        public async Task UpdateAsync(Group group)
        {           
                _context.Groups.Update(group);
                await _context.SaveChangesAsync();            
        }
    }

}
