using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using SendGrid.Helpers.Errors.Model;
using SendGrid.Helpers.Mail;
using System.Security.Cryptography;

namespace EF_CourseApp.Controllers
{
    internal class EducationController
    {
        private readonly EducationService _educationService;
        private readonly GroupService _groupService;
       


        public EducationController()
        {
            _educationService = new EducationService();
            _groupService = new GroupService();
        }

        public async Task CreateAsync()
        {
            try
            {
                Console.WriteLine("Add education name:");
                Name: string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Name can't be empty. Please try again.");
                    goto Name;
                }

                Console.WriteLine("Add education color:");
                Color: string color = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(color))
                {
                    Console.WriteLine("Color can't be empty. Please try again.");
                    goto Color;
                }

                await _educationService.CreateAsync(new Education { Name = name, Color = color });

                Console.WriteLine("Education successfully added.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        
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

        public async Task GetByIdAsync()
        {
            Console.WriteLine("Enter the Education ID: ");
            gId: string idStr = Console.ReadLine();

            if (string.IsNullOrEmpty(idStr))
            {
                Console.WriteLine("ID can't be empty. Please try again.");
                goto gId;
            }

            if (!int.TryParse(idStr, out int id))
            {
                Console.WriteLine("Input format is wrong. Please add correct format ID");
                goto gId;
            }

            try
            {
                var data = await _educationService.GetByIdAsync(id);

                if (data == null)
                {
                    Console.WriteLine("Education not found.");
                    return;
                }

                string result = $"Education Name: {data.Name}, Education Color: {data.Color}";
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task DeleteAsync()
        {
            Console.WriteLine("Please write the Education id: ");
        Id: string idStr = Console.ReadLine();
            int id;
            bool isCorrectIdFormat = int.TryParse(idStr, out id);

            if (string.IsNullOrEmpty(idStr))
            {
                Console.WriteLine("id can't be empty. Please try again.");
                goto Id;
            }

            if (isCorrectIdFormat)
            {
                try
                {
                    await _educationService.DeleteAsync(id);
                             

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                
                    Console.WriteLine("Input format is wrong. Please enter correct information");
                    goto Id;
                
            }
        }

        public async Task Update()
        {
            bool isInputValid = false;

            while (!isInputValid)
            {
                try
                {
                    Console.WriteLine("Please write education ID:");

                uId: string idStr = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(idStr))
                    {
                        Console.WriteLine("ID can't be empty. Please write a gain.");
                       goto uId;
                    }

                    if (!int.TryParse(idStr, out int id))
                    {
                        Console.WriteLine("Format is wrong, please write correctly again");
                        goto uId; 
                    }

                    var existingEducation = await _educationService.GetByIdAsync(id);
                    if (existingEducation == null)
                    {
                        Console.WriteLine("Education not found.");
                        goto uId; 
                    }

                  
                    Console.WriteLine("Please write new education name:");
                    string name = Console.ReadLine();
                    existingEducation.Name = name;
                                        
                    Console.WriteLine("\"Please write new education color:");
                    string color = Console.ReadLine();
                    existingEducation.Color = color;

                    await _educationService.UpdateAsync(existingEducation);
                    Console.WriteLine("Education successfully updated.");
                    isInputValid = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task SearchByEducation()
        {
            Console.WriteLine("Please enter the education name:");

            while (true)
            {
                string textStr = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(textStr))
                {
                    Console.WriteLine("Input cannot be empty. Please try again.");
                    continue;
                }

                try
                {
                    var result = await _educationService.GetAllWhithExpression(m => m.Name.ToLower().Trim().Contains(textStr.ToLower().Trim()));

                    if (result.Count == 0)
                    {
                        Console.WriteLine("Education not found. Please try again.");
                        continue;
                    }

                    foreach (var item in result)
                    {
                        string data = $"Education name: {item.Name}, Education color: {item.Color}";
                        Console.WriteLine(data);
                    }

                    break; // Выход из цикла, если найдены образования
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}. Please try again.");
                }
            }
        }

        public async Task<List<Group>> GetAllWhithGroupsAsync()
        {
            Console.WriteLine("Please write group name:");
        GName: string groupName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(groupName))
            {
                Console.WriteLine("Group name cannot be empty.");
                goto GName;
            }

            try
            {
                Func<Group, bool> predicate = g => g.Name.Equals(groupName.Trim(), StringComparison.OrdinalIgnoreCase);

                var result = await _groupService.GetAllWhithExpression(predicate);

                if (result.Count == 0)
                {
                    Console.WriteLine("Group not found.");
                    return new List<Group>();
                }

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return new List<Group>();
            }
        }








    }
         




}

       