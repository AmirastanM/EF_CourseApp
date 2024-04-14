using Domain.Models;
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
using Group = Domain.Models.Group;

namespace EF_CourseApp.Controllers
{
    internal class GroupController
    {
        private readonly GroupService _groupService;

        public GroupController()
        {
            _groupService = new GroupService();
        }


        public async Task CreateAsync()
        {
            try
            {
                Console.WriteLine("Add group name:");
            GroupName: string name = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(name))
                {
                    Console.WriteLine("Name can't be empty. Please try again.");
                    goto GroupName;
                }

                var result = await _groupService.GetAllAsync();

                bool educationExists = false;
                foreach (var item in result)
                {
                    if (item.Name == name)
                    {
                        educationExists = true;
                        break;
                    }
                }
                if (name.Length < 3)
                {
                    ConsoleColor.Red.WriteConsole("Group name can't be less than three letters ");
                    goto GroupName;
                }
                if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
                {
                    ConsoleColor.Red.WriteConsole("Format is wrong");
                    goto GroupName;
                }


                Console.WriteLine("Add group capacity:");
            Capacity: string gCapacity = Console.ReadLine();
                int capacity;

                if (string.IsNullOrWhiteSpace(gCapacity) || !int.TryParse(gCapacity, out capacity))
                {
                    Console.WriteLine("Capacity can't be empty or string. Please try again.");
                    goto Capacity;
                }

                Console.WriteLine("Add Education Id:");
            eId: string eduId = Console.ReadLine();
                int eId;

                if (string.IsNullOrWhiteSpace(eduId))
                {
                    Console.WriteLine("Capacity can't be empty or string. Please try again.");
                    goto eId;
                }
                if (!int.TryParse(eduId, out eId))
                {
                    Console.WriteLine("Invalid education ID. Please try again.");
                    goto eId;
                }
                else
                {
                    int educationId = eId;
                }


                DateTime time = DateTime.Now;

                await _groupService.CreateAsync(new Group { Name = name.Trim().ToLower(), Capacity = capacity, EducationId = eId, Date = time });

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
                    Console.WriteLine("Please write Group ID:");

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
                    var existingGroup = await _groupService.GetByIdAsync(id);
                    if (existingGroup == null)
                    {
                        Console.WriteLine("Education not found.");
                        goto uId;
                    }

                    Console.WriteLine("Please write new group name:");
                    checkName: string name = Console.ReadLine();
                    if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
                    {
                        Console.WriteLine("Not correct input, please try again");
                        goto checkName;
                    }
                    existingGroup.Name = name;

                    Console.WriteLine("Please write new group capacity:");
                    rCapacity: string capacityStr = Console.ReadLine();
                    int capacity;
                    if (!int.TryParse(capacityStr, out capacity))
                    {
                        Console.WriteLine("Capacity can't be string. Please try again.");
                        goto rCapacity;
                    }
                    existingGroup.Capacity = capacity;

                    await _groupService.UpdateAsync(existingGroup);
                    Console.WriteLine("Education successfully updated.");
                    isInputValid = true;

                    Console.WriteLine("Please write new education id:");
                eID: string eIdStr = Console.ReadLine();
                    int eduId;
                    if (!int.TryParse(eIdStr, out eduId))
                    {
                        Console.WriteLine("Education Id can't be letter. Please try again.");
                        goto eID;
                    }
                    existingGroup.EducationId = eduId;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public async Task DeleteAsync()
        {
            Console.WriteLine("Please add the Group id: ");
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
                    await _groupService.DeleteAsync(id);


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
            var datas = await _groupService.GetAllAsync();
            if (datas != null || !datas.Any())
            {
                await Console.Out.WriteLineAsync("Data not found");
            }
            foreach (var item in datas)
            {
                string data = $"Group Name: {item.Name}, Group Capacity: {item.Capacity}, Education Id: {item.EducationId}";
                await Console.Out.WriteLineAsync(data);
            }
        }

        public async Task GetByIdAsync()
        {
            Console.WriteLine("Enter the Group ID: ");
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
                var data = await _groupService.GetByIdAsync(id);

                if (data == null)
                {
                    Console.WriteLine("Education not found.");
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
            Console.WriteLine("Please enter the group name:");

            while (true)
            {
                string textStr = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(textStr))
                {
                    Console.WriteLine("Input can't be empty. Please try again.");
                    continue;
                }

                try
                {
                    var result = await _groupService.SearchAsync(textStr);

                    if (result.Count == 0)
                    {
                        Console.WriteLine("Data not found. Please try again.");
                        continue;
                    }

                    foreach (var item in result)
                    {
                        string data = $"Group name: {item.Name}, Group capacity: {item.Capacity}, Education Id: {item.EducationId}";
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

            var datas = await _groupService.GetAllAsync();
            if (datas != null || !datas.Any())
            {
                await Console.Out.WriteLineAsync("Data not found");
            }
            foreach (var item in datas)
            {
                string data = $"Group Name: {item.Name}, Group capacity: {item.Capacity}, Education name: {string.Join(", ", item.Education)}";
                await Console.Out.WriteLineAsync(data);
            }
        }
               
        public async Task GetAllWithEducationIdAsync()
        {
            Console.WriteLine("Enter the Education ID: ");
        gId: string idStr = Console.ReadLine();

            if (string.IsNullOrEmpty(idStr))
            {
                Console.WriteLine("ID can't be empty. Please try again.");
                goto gId;
            }

            if (!int.TryParse(idStr, out int educationId))
            {
                Console.WriteLine("Format is wrong. Please enter integer format.");
                goto gId;
            }

            try
            {
                var data = await _groupService.GetAllWithEducationIdAsync(educationId);

                if (data == null || data.Count == 0)
                {
                    Console.WriteLine("No groups found for this ID.");
                    return;
                }

                foreach (var group in data)
                {
                    string result = $"Group Name: {group.Name}, Group Capacity: {group.Capacity}, Education Id: {group.EducationId}";
                    Console.WriteLine(result);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine( ex.Message);
            }
        }

        public async Task<List<Group>> SortWithCapacityAsync()
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
                return new List<Group>();
            }
        }
    }
    }

    

