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
using System.Xml.Linq;

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

        public async Task<List<Group>> FilterByEducationNameAsync(string name)
        {
            return await _context.Groups.Where(group => group.Education.Name == name).ToListAsync();
        }
              
        public async Task<List<Group>> GetAllAsync()
        {
           
            return await _context.Groups.ToListAsync();     
         
        }

        public async Task<List<Group>> GetAllWithEducationIdAsync(int id)
        {
           
                return await _context.Groups.Include(group => group.Education).Where(group => group.EducationId == id).ToListAsync();
            
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
            IQueryable<Group> query = _context.Groups;

            if (sort == "asc")
            {
                query = query.OrderBy(g => g.Capacity);
            }
            else if (sort == "desc")
            {
                query = query.OrderByDescending(g => g.Capacity);
            }
            else
            {
                throw new ArgumentException("Invalid sort type. Please choose 'asc' or 'desc'.");
            }

            return await query.ToListAsync();
        }
        public async Task UpdateAsync(Group group)
        {           
                _context.Groups.Update(group);
                await _context.SaveChangesAsync();            
        }
    }

}
