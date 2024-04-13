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
        Task<List<Group>> GetAllWhithExpression(Func<Group, bool> predicate);
    }
}
