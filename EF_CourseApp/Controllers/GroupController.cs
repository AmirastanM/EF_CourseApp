﻿using Domain.Models;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Service.Extensions;
using Service.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System.Xml.Linq;

namespace EF_CourseApp.Controllers
{
    internal class GroupController
    {
        private readonly GroupService _groupService;
        private readonly EducationService _educationService;


        public GroupController()
        {
            _groupService = new GroupService();
            _educationService = new EducationService();
        }


        public async Task CreateAsync()
        {

            ConsoleColor.Cyan.WriteConsole("Add group name:");
        GroupName: string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                ConsoleColor.Red.WriteConsole("Name can't be empty, please try again.");
                goto GroupName;
            }
          

            var result = await _groupService.GetAllAsync();

            foreach (var item in result)
            {
                if (item.Name.ToLower() == name.ToLower().Trim())
                {
                    ConsoleColor.Red.WriteConsole("This name already exists, please add a different name");
                    goto GroupName;
                }
            }

            if (name.Length < 3)
            {
                ConsoleColor.Red.WriteConsole("Group name can't be less than 3 letters");
                goto GroupName;
            }

            if (!Regex.IsMatch(name, @"^[a-zA-Z]+\d+$"))
            {
                ConsoleColor.Red.WriteConsole("Format is wrong, try again");
                goto GroupName;
            }

            ConsoleColor.Cyan.WriteConsole("Add group capacity:");
        Capacity: string gCapacity = Console.ReadLine();
            int capacity;

            if (string.IsNullOrWhiteSpace(gCapacity) || !int.TryParse(gCapacity, out capacity) || capacity > 20)
            {
                Console.WriteLine("Capacity been integer between 1 and 20. Please try again.");
                goto Capacity;
            }

            ConsoleColor.Cyan.WriteConsole("Add Education ID:");

            var educationList = await _educationService.GetAllAsync();
            foreach (var education in educationList)
            {
                Console.WriteLine($"ID: {education.Id}, Name: {education.Name}");
            }

        EduID: string eduId = Console.ReadLine();
            int educationId;

            if (string.IsNullOrWhiteSpace(eduId) || !int.TryParse(eduId, out educationId))
            {
                ConsoleColor.Red.WriteConsole("Input can't be letter or empty, please try again.");
                goto EduID;
            }

            var existingEducation = educationList.FirstOrDefault(e => e.Id == educationId);

            if (existingEducation == null)
            {
                ConsoleColor.Red.WriteConsole("Education not found, please try again.");
                goto EduID;
            }
            try
            {
                DateTime time = DateTime.Now;

                await _groupService.CreateAsync(new Domain.Models.Group { Name = name.Trim().ToLower(), Capacity = capacity, EducationId = educationId, Date = time });

                ConsoleColor.Green.WriteConsole("Group successfully added");
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
                    ConsoleColor.Cyan.WriteConsole("Please write Group ID:");

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

                    var existingGroup = await _groupService.GetByIdAsync(id);
                    if (existingGroup == null)
                    {
                        Console.WriteLine("Group not found");
                        goto uId;
                    }

                    ConsoleColor.Cyan.WriteConsole("Please write new group name:");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        name = existingGroup.Name;
                    }
                    else if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
                    {
                        ConsoleColor.Red.WriteConsole("Input is not correct, please try again.");
                        continue;
                    }

                    ConsoleColor.Cyan.WriteConsole("Please write new group capacity :");
                    string capacityStr = Console.ReadLine();
                    int capacity;
                    if (string.IsNullOrWhiteSpace(capacityStr))
                    {
                        capacity = existingGroup.Capacity;
                    }
                    else if (!int.TryParse(capacityStr, out capacity))
                    {
                        ConsoleColor.Red.WriteConsole("Input is not correct, please try again.");
                        continue;
                    }

                    ConsoleColor.Cyan.WriteConsole("Please write new education ID:");
                    string eduIdStr = Console.ReadLine();
                    int eduId;
                    if (string.IsNullOrWhiteSpace(eduIdStr))
                    {
                        eduId = existingGroup.EducationId;
                    }
                    else if (!int.TryParse(eduIdStr, out eduId))
                    {
                        ConsoleColor.Red.WriteConsole("Input is not correct, please try again.");
                        continue;
                    }

                    existingGroup.Name = name;
                    existingGroup.Capacity = capacity;
                    existingGroup.EducationId = eduId;

                    await _groupService.UpdateAsync(existingGroup);
                    Console.WriteLine("Group successfully updated.");
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
            ConsoleColor.Cyan.WriteConsole("Please add the Group id: ");
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
                    await _groupService.DeleteAsync(id);


                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {

                ConsoleColor.Red.WriteConsole("Input format is wrong, please enter correct information");
                goto Id;

            }
        }

        public async Task GetAllAsync()
        {
           
                var datas = await _groupService.GetAllAsync();
                if (datas == null || !datas.Any())
                {
                    ConsoleColor.Red.WriteConsole("Data not found");
                }
                else
                {
                    foreach (var item in datas)
                    {
                        string data = $"Group Id: {item.Id}, Group Name: {item.Name}, Group Capacity: {item.Capacity}, Education Id: {item.EducationId}";
                        await Console.Out.WriteLineAsync(data);
                    }
                }
            
        }

        public async Task GetByIdAsync()
        {
            ConsoleColor.Cyan.WriteConsole("Enter Group ID: ");
        gId: string idStr = Console.ReadLine();

            if (string.IsNullOrEmpty(idStr))
            {
                ConsoleColor.Red.WriteConsole("ID can't be empty. Please try again.");
                goto gId;
            }

            if (!int.TryParse(idStr, out int id))
            {
                ConsoleColor.Red.WriteConsole("Format is wrong. Please add correct format");
                goto gId;
            }

            try
            {
                var data = await _groupService.GetByIdAsync(id);

                if (data == null)
                {
                    ConsoleColor.Red.WriteConsole("Education not found.");
                    return;
                }

                string result = $"Group Name: {data.Name}, Group Capacity: {data.Capacity}, Education Id: {data.EducationId}";
                Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task SearchAsync()
        {
            ConsoleColor.Cyan.WriteConsole("Please enter the group name:");

            while (true)
            {
                string textStr = Console.ReadLine();

            Text: if (string.IsNullOrWhiteSpace(textStr))
                {
                    ConsoleColor.Red.WriteConsole("Input can't be empty. Please try again.");
                    goto Text;
                }
                if (!Regex.IsMatch(textStr, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    ConsoleColor.Red.WriteConsole("Format is wrong");
                    goto Text;
                }

                try
                {
                    var result = await _groupService.SearchAsync(textStr);

                    if (result.Count == 0)
                    {
                        ConsoleColor.Red.WriteConsole("Data not found, please try again.");
                        goto Text;
                    }

                    foreach (var item in result)
                    {
                        string data = $"Group name: {item.Name}, Group capacity: {item.Capacity}, CreateDate: {item.Date}";
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

        public async Task FilterByEducationNameAsync()
        {

            ConsoleColor.Cyan.WriteConsole("Add the Education Name: ");
        EName: string educationName = Console.ReadLine();


            if (string.IsNullOrWhiteSpace(educationName))
            {
                ConsoleColor.Red.WriteConsole("Input can't be empty");
                goto EName;
            }
            else
            {

                try
                {
                    var response = await _groupService.FilterByEducationNameAsync(educationName);
                    if (response.Count != 0)
                    {
                        foreach (var item in response)
                        {
                            string data = $"Group Name: {item.Name}, Education: {item.Education.Name}";
                            Console.WriteLine(data);
                        }
                    }
                    else
                    {
                        ConsoleColor.Red.WriteConsole("Data not found");
                        goto EName;
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    goto EName;
                }
            }
            
        }

        public async Task GetAllWithEducationIdAsync()
            {
                ConsoleColor.Cyan.WriteConsole("Enter the Education ID: ");
            gId: string idStr = Console.ReadLine();

                if (string.IsNullOrEmpty(idStr))
                {
                    ConsoleColor.Red.WriteConsole("Input can't be empty, please try again.");
                    goto gId;
                }

                if (!int.TryParse(idStr, out int educationId))
                {
                    ConsoleColor.Red.WriteConsole("Format is wrong, please try again.");
                    goto gId;
                }

                var datas = await _groupService.GetAllWithEducationIdAsync(educationId);

                if (datas == null || !datas.Any())
                {
                    ConsoleColor.Red.WriteConsole("Data not found");
                }
                else
                {
                    foreach (var item in datas)
                    {
                        string data = $"Group Name: {item.Name}, Group capacity: {item.Capacity}, Education Id: {string.Join(", ", item.EducationId)}";
                        await Console.Out.WriteLineAsync(data);
                    }
                }
            }

        public async Task<List<Domain.Models.Group>> SortWithCapacityAsync()
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
                            ConsoleColor.Red.WriteConsole("Please choose correct type 'Asc' or 'Desc'.");
                        }
                    }
                    while (sortType.ToLower() != "asc" && sortType.ToLower() != "desc");

                    var groups = await _groupService.SortWithCapacityAsync(sortType);

                    foreach (var group in groups)
                    {
                        Console.WriteLine($"Name: {group.Name}, Capacity: {group.Capacity}");
                    }

                    return groups;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<Domain.Models.Group>();
                }
            }



        }
    }
    
    

    

