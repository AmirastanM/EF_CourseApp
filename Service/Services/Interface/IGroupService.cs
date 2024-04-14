using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Interface
{
    internal interface IGroupService
    {
        Task<List<Group>> GetAllAsync();
        Task<Group> GetByIdAsync(int id);
        Task CreateAsync(Group group);
        Task UpdateAsync(Group group);
        Task DeleteAsync(int id);
        Task<List<Group>> SearchAsync(string txt);
        Task<List<Group>> GetAllWithEducationIdAsync(int EducationId);
        Task<List<Group>> FilterByEducationNameAsync();
        Task<List<Group>> SortWithCapacityAsync(string sort);

    }
}

 