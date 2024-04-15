
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
        Task CreateEducationAsync(Education education);
        Task UpdateEducationAsync(Education education);
        Task DeleteEducationAsync(int id);
        Task<List<Education>> SearchAsync(string txt);
        Task<List<Education>> GetAllWithGroupsAsync(string name);
        Task<List<Education>> SortWhithCreateDateAsync(string sortType);

    }
}


