using Domain.Models;
using Service.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class GroupService : IGroupService
    {
        public Task CreateAsync(Group group)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Group>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Group>> GetAllWhithExpression(Func<Group, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public Task<Group> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(Group group)
        {
            throw new NotImplementedException();
        }
    }
}
