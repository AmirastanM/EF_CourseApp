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

                var result = await _educationService.GetAllAsync();

                bool educationExists = false;
                foreach (var item in result)
                {
                    if (item.Name == name)
                    {
                        educationExists = true;
                        break;
                    }
                }

                if (educationExists)
                {
                    ConsoleColor.Red.WriteConsole("This education name exists. Please add a new name.");
                    goto Name;
                }

                if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    ConsoleColor.Red.WriteConsole("Format is wrong, Please add again");
                    goto Name;
                }

                if (name.Length < 3)
                {
                    ConsoleColor.Red.WriteConsole(" Name of education must not be less than 3 letters ");
                    goto Name;
                }


                Console.WriteLine("Add education color:");
            Color: string color = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(color))
                {
                    Console.WriteLine("Color can't be empty. Please try again.");
                    goto Color;
                }
                if (!Regex.IsMatch(color, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    ConsoleColor.Red.WriteConsole("Format is wrong, Please add again");
                    goto Color;
                }

                DateTime time = DateTime.Now;

                await _educationService.CreateAsync(new Education { Name = name.Trim().ToLower(), Color = color.Trim().ToLower(), Date = time });

                Console.WriteLine("Education successfully added.");
            }
            catch (Exception ex)
            {                
                Console.WriteLine(ex.Message);
            }
        
    }

        public async Task UpdateAsync()
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

                    DateTime time = DateTime.Now;
                    var existingEducation = await _educationService.GetByIdAsync(id);
                    if (existingEducation == null)
                    {
                        Console.WriteLine("Education not found.");
                        goto uId;
                    }


                    Console.WriteLine("Please write new education name:");
                    checkName: string name = Console.ReadLine();
                    if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
                    {
                        Console.WriteLine("Not correct input, please try again");
                        goto checkName;
                    }
                    existingEducation.Name = name;

                    Console.WriteLine("Please write new education color:");
                    string color = Console.ReadLine();
                    if (!Regex.IsMatch(color, @"^[\p{L}\p{M}' \.\-]+$"))
                    {
                        Console.WriteLine("Not correct input, please try again");
                        goto checkName;
                    }
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
                    DateTime time = DateTime.Now;
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

        public async Task GetAllAsync()
        {
            var datas = await _educationService.GetAllAsync();
            if (datas != null || !datas.Any())
            {
                await Console.Out.WriteLineAsync("Data not found");
            }
            foreach (var item in datas)
            {
                string data = $"Education Name: {item.Name}, Education Color: {item.Color}, Created day: {item.Date}";
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
            if (datas != null || !datas.Any())
            {
                await Console.Out.WriteLineAsync("Data not found");
            }
            foreach (var item in datas)
            {
                string data = $"Education Name: {item.Name}, Education Color: {item.Color}, Group name: {string.Join(", ", item.Groups)}";
                await Console.Out.WriteLineAsync(data);
            }
        }
    }
}

       