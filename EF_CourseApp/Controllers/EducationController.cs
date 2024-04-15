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
using System.Text.RegularExpressions;
using Service.Extensions;
using System.Xml.Linq;

namespace EF_CourseApp.Controllers
{
    internal class EducationController
    {
        private readonly EducationService _educationService;

        public EducationController()
        {
            _educationService = new EducationService();
        }

        public async Task CreateEducationAsync()
        {
            string name = string.Empty;
            string color = string.Empty;

            ConsoleColor.Cyan.WriteConsole("Add education name:");
        Name: name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                ConsoleColor.Red.WriteConsole("Name can't be empty. Please try again:");
                name = Console.ReadLine();
                goto Name;
            }
            if (name.Length < 3)
            {
                ConsoleColor.Red.WriteConsole("Name can't be shorter than 3 characters");
                goto Name;
            }

            var result = await _educationService.GetAllAsync();

            bool nameExists = result.Any(item => item.Name.ToLower() == name.ToLower().Trim());
            if (nameExists)
            {
                ConsoleColor.Red.WriteConsole("This name already exists. Please try again:");
                goto Name;
            }

            if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
            {
                ConsoleColor.Red.WriteConsole("Incorrect format. Please try again:");

                goto Name;
            }

            ConsoleColor.Cyan.WriteConsole("Add education color:");

        Color: color = Console.ReadLine()?.Trim().ToLower();

            if (string.IsNullOrEmpty(color))
            {
                ConsoleColor.Red.WriteConsole("Color can't be empty. Please try again:");
                color = Console.ReadLine();
                goto Color;
            }

            if (!Regex.IsMatch(color, @"^[\p{L}\p{M}' \.\-]+$"))
            {
                ConsoleColor.Red.WriteConsole("Incorrect format. Please try again:");
                goto Color;
            }

            try
            {
                DateTime time = DateTime.Now;

                await _educationService.CreateEducationAsync(new Education { Name = name.Trim().ToLower(), Color = color.Trim().ToLower(), Date = time });

                ConsoleColor.Green.WriteConsole("Education successfully added.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public async Task UpdateEducationAsync()
        {
            bool isInputValid = false;

            while (!isInputValid)
            {
                try
                {
                    List<Education> educations = await _educationService.GetAllAsync();

                    Console.WriteLine("Exist Educations:");
                    foreach (Education education in educations)
                    {
                        Console.WriteLine($"EducationId: {education.Id}, Name: {education.Name}");
                    }

                    Console.WriteLine("Please write education ID:");

                uId: string idStr = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(idStr))
                    {
                        ConsoleColor.Red.WriteConsole("ID can't be empty ,please write again.");
                        goto uId;
                    }

                    if (!int.TryParse(idStr, out int id))
                    {
                        ConsoleColor.Red.WriteConsole("Format is wrong, please write correctly.");
                        goto uId;
                    }

                    var existingEducation = await _educationService.GetByIdAsync(id);
                    if (existingEducation == null)
                    {
                        ConsoleColor.Red.WriteConsole("Education not found.");
                        goto uId;
                    }

                    ConsoleColor.Cyan.WriteConsole("Please write new education name:");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        name = existingEducation.Name;
                    }
                    else if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
                    {
                        ConsoleColor.Red.WriteConsole("Input isn't correct, please try again.");
                        continue;
                    }

                    ConsoleColor.Cyan.WriteConsole("Please write new education color:");
                    string color = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(color))
                    {
                        color = existingEducation.Color;
                    }
                    else if (!Regex.IsMatch(color, @"^[\p{L}\p{M}' \.\-]+$"))
                    {
                        ConsoleColor.Red.WriteConsole("Input isn't correct, please try again.");
                        continue;
                    }

                    existingEducation.Name = name;
                    existingEducation.Color = color;

                    DateTime time = DateTime.Now;
                    await _educationService.UpdateEducationAsync(existingEducation);
                    ConsoleColor.Green.WriteConsole("Education successfully updated.");
                    isInputValid = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task DeleteEducationAsync()
        {
            
                
                List<Education> educations = await _educationService.GetAllAsync();

                
                Console.WriteLine("List of Educations:");
                foreach (Education education in educations)
                {
                    Console.WriteLine($"EducationId: {education.Id}, Name: {education.Name}");
                }

            ConsoleColor.Cyan.WriteConsole("Please write the Education id: ");
            Id: string idStr = Console.ReadLine();
                int id;
                bool isCorrectIdFormat = int.TryParse(idStr, out id);

                if (string.IsNullOrEmpty(idStr))
                {
                ConsoleColor.Red.WriteConsole("Input can't be empty, please try again.");
                    goto Id;
                }

                if (isCorrectIdFormat)
                {
                    try
                    {
                        DateTime time = DateTime.Now;
                        await _educationService.DeleteEducationAsync(id);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                else
                {
                ConsoleColor.Red.WriteConsole("Format is wrong, please enter correct format");
                    goto Id;
                }
           
        }

        public async Task GetAllAsync()
        {
            var datas = await _educationService.GetAllAsync();
            if (datas == null || !datas.Any())
            {
                ConsoleColor.Red.WriteConsole("Data not found");
            }
            else
            {
                foreach (var item in datas)
                {
                    string data = $"Education Id: {item.Id}, Education Name: {item.Name}, Education Color: {item.Color}, Created day: {item.Date}";
                    Console.WriteLine(data);
                }
            }
        }

        public async Task GetByIdAsync()
        {
            ConsoleColor.Cyan.WriteConsole("Enter the Education ID: ");
        gId: string idStr = Console.ReadLine();

            if (string.IsNullOrEmpty(idStr))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty, please try again.");
                goto gId;
            }

            if (!int.TryParse(idStr, out int id))
            {
                ConsoleColor.Red.WriteConsole("Format is wrong, Please add correct format");
                goto gId;
            }

            try
            {
                var data = await _educationService.GetByIdAsync(id);

                if (data == null)
                {
                    ConsoleColor.Red.WriteConsole("Education not found.");
                    return;
                }

                string result = $"Education Name: {data.Name}, Education Color: {data.Color}, CreateDate: {data.Date}";
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SearchAsync()
        {
            ConsoleColor.Cyan.WriteConsole("Please enter the education name:");

            string textStr;
            do
            {
                textStr = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(textStr))
                {
                    ConsoleColor.Red.WriteConsole("Input can't be empty. Please try again.");
                    continue;
                }

                if (!Regex.IsMatch(textStr, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    ConsoleColor.Red.WriteConsole("Format is wrong");
                    continue;
                }

                try
                {
                    var result = await _educationService.SearchAsync(textStr);

                    if (result.Count == 0)
                    {
                        ConsoleColor.Red.WriteConsole("Data not found, please try again.");
                        continue;
                    }

                    foreach (var item in result)
                    {
                        string data = $"Education name: {item.Name}, Education color: {item.Color}, CreateDate: {item.Date}";
                        Console.WriteLine(data);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                break;
            } while (true);
        }

        public async Task GetAllWithGroupsAsync()
        {
            var datas = await _educationService.GetAllAsync();
            if (datas == null || !datas.Any())
            {
                ConsoleColor.Red.WriteConsole("Data not found");
            }
            else
            {
                foreach (var item in datas)
                {
                    string groupNames = (item.Groups != null && item.Groups.Any()) ? string.Join(", ", item.Groups.Select(g => g.Name)) : "No groups";
                    Console.WriteLine($"Education Name: {item.Name}, Education Color: {item.Color}, Group Name: {item.Groups}");
                }
            }
        }

        public async Task<List<Education>> SortWithCreateDateAsync()
        {
            try
            {
                string sortType;
                do
                {
                    ConsoleColor.Cyan.WriteConsole("Choose sort type: Asc or Desc");
                    sortType = Console.ReadLine();

                    if (sortType.ToLower() != "asc" && sortType.ToLower() != "desc")
                    {
                        ConsoleColor.Cyan.WriteConsole("Please choose sorting type 'Asc' or 'Desc'.");
                    }
                }
                while (sortType.ToLower() != "asc" && sortType.ToLower() != "desc");

                var educations = await _educationService.SortWhithCreateDateAsync(sortType);

                foreach (var education in educations)
                {
                    Console.WriteLine($"Name: {education.Name}, CreateDate: {education.Date}");
                }

                return educations;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new List<Education>();
            }
        }
    }
}

       