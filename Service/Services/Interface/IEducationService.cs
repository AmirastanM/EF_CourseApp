
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Service.Services.Interface
{
    internal interface IEducationService
    {
        Task<List<Education>> GetAllAsync();
        Task<Education> GetByIdAsync(int id);
        Task CreateAsync(Education education);
        Task UpdateAsync(Education education);
        Task DeleteAsync(int id);
        Task<List<Education>> GetAllWhithExpression(Func<Education, bool> predicate);
    }
}
