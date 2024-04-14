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

                var result = await _educationService.GetAllAsync();

                bool nameExists = result.Any(item => item.Name.ToLower() == name.ToLower().Trim());
                if (nameExists)
                {
                    ConsoleColor.Red.WriteConsole("This name already exists. Please try again:");                    
                    goto Name;
                 }

                if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    ConsoleColor.Red.WriteConsole("Format is wrong. Please try again:");
                    name = Console.ReadLine();
                    goto Name;
                 }

                if (name.Length < 3)
                {
                    ConsoleColor.Red.WriteConsole("Name of education can't be less than 3 letters. Please try again:");
                    name = Console.ReadLine();
                goto Name;
                 }


            ConsoleColor.Cyan.WriteConsole("Add education color:");
            Color:string colore = Console.ReadLine()?.Trim().ToLower();
         
                if (string.IsNullOrWhiteSpace(colore))
                {
                ConsoleColor.Red.WriteConsole("Color can't be empty. Please try again:");
                    color = Console.ReadLine();
                    goto Color;
                }

                if (!Regex.IsMatch(colore, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    ConsoleColor.Red.WriteConsole("Format is wrong. Please try again:");
                    goto Color;
                } 
            try
            {
                DateTime time = DateTime.Now;

                await _educationService.CreateEducationAsync(new Education { Name = name.Trim().ToLower(), Color = color.Trim().ToLower(), Date = time });

                Console.WriteLine("Education successfully added.");
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

                    Console.WriteLine("List of Educations:");
                    foreach (Education education in educations)
                    {
                        Console.WriteLine($"EducationId: {education.Id}, Name: {education.Name}");
                    }

                    Console.WriteLine("Please write education ID:");

                uId: string idStr = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(idStr))
                    {
                        Console.WriteLine("ID can't be empty. Please write again.");
                        goto uId;
                    }

                    if (!int.TryParse(idStr, out int id))
                    {
                        Console.WriteLine("Format is wrong. Please write correctly.");
                        goto uId;
                    }

                    var existingEducation = await _educationService.GetByIdAsync(id);
                    if (existingEducation == null)
                    {
                        Console.WriteLine("Education not found.");
                        goto uId;
                    }

                    Console.WriteLine("Please write new education name (leave empty to keep the old value):");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        name = existingEducation.Name;
                    }
                    else if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
                    {
                        Console.WriteLine("Not correct input. Please try again.");
                        continue;
                    }

                    Console.WriteLine("Please write new education color (leave empty to keep the old value):");
                    string color = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(color))
                    {
                        color = existingEducation.Color;
                    }
                    else if (!Regex.IsMatch(color, @"^[\p{L}\p{M}' \.\-]+$"))
                    {
                        Console.WriteLine("Not correct input. Please try again.");
                        continue;
                    }

                    existingEducation.Name = name;
                    existingEducation.Color = color;

                    DateTime time = DateTime.Now;
                    await _educationService.UpdateEducationAsync(existingEducation);
                    Console.WriteLine("Education successfully updated.");
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
            try
            {
                
                List<Education> educations = await _educationService.GetAllAsync();

                
                Console.WriteLine("List of Educations:");
                foreach (Education education in educations)
                {
                    Console.WriteLine($"EducationId: {education.Id}, Name: {education.Name}");
                }

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
                    Console.WriteLine("Input format is wrong. Please enter correct information");
                    goto Id;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task GetAllAsync()
        {
            var datas = await _educationService.GetAllAsync();
            if (datas != null || !datas.Any())
            {
                await Console.Out.WriteLineAsync("Data not found");
            }
            foreach (var item in datas)
            {
                string data = $"Education Id: {item.Id}, Education Name: {item.Name}, Education Color: {item.Color}, Created day: {item.Date}";
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
                Console.WriteLine("Format is wrong. Please add correct format");
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
            Console.WriteLine("Please enter the education name:");

            while (true)
            {
                string textStr = Console.ReadLine();

                Text: if (string.IsNullOrWhiteSpace(textStr))
                {
                    Console.WriteLine("Input can't be empty. Please try again.");
                    goto Text;
                }
                if (!Regex.IsMatch(textStr, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    ConsoleColor.Red.WriteConsole("Format is wrong");
                    goto Text;
                }

                try
                {
                    var result = await _educationService.SearchAsync(textStr);

                    if (result.Count == 0)
                    {
                        Console.WriteLine("Data not found. Please try again.");
                        goto Text;
                    }

                    foreach (var item in result)
                    {
                        string data = $"Education name: {item.Name}, Education color: {item.Color}, CreateDate: {item.Date}";
                        Console.WriteLine(data);
                    }

                    break;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task GetAllWithGroupsAsync()
        {
            var datas = await _educationService.GetAllAsync();
            if (datas == null || !datas.Any())
            {
                await Console.Out.WriteLineAsync("Data not found");
            }
            else
            {
                foreach (var item in datas)
                {
                    string groupNames = item.Groups != null? string.Join(", ", item.Groups) : string.Empty;
                    string data = $"Education Name: {item.Name}, Education Color: {item.Color}, Group name: {groupNames}";
                    await Console.Out.WriteLineAsync(data);
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
                    Console.WriteLine("Choose sort type: Asc or Desc");
                    sortType = Console.ReadLine();

                    if (sortType.ToLower() != "asc" && sortType.ToLower() != "desc")
                    {
                        Console.WriteLine("Invalid sort type. Please choose one type 'Asc' or 'Desc'.");
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

       