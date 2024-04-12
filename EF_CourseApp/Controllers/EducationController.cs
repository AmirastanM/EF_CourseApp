using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace EF_CourseApp.Controllers
{
    internal class EducationController
    {
        private readonly EducationService _educationService;

        public EducationController()
        {
            _educationService = new EducationService();
        }

        public async Task  GetAllAsync()
        {
            var datas = await _educationService.GetAllAsync();
            if (datas != null || !datas.Any()) 
            {
                await Console.Out.WriteLineAsync("Data not found");
            }
            foreach (var item in datas) 
            {
                string data = $"Education Name: {item.Name}, Education Color: {item.Color}";
                await Console.Out.WriteLineAsync(data);
            }
        }

    }
}
