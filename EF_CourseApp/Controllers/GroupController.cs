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

            Console.WriteLine("Add group name:");
        GroupName: string name = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Name can't be empty. Please try again.");
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

            if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
            {
                ConsoleColor.Red.WriteConsole("Format is wrong, try again");
                goto GroupName;
            }

            Console.WriteLine("Add group capacity:");
        Capacity: string gCapacity = Console.ReadLine();
            int capacity;

            if (string.IsNullOrWhiteSpace(gCapacity) || !int.TryParse(gCapacity, out capacity) || capacity > 20)
            {
                Console.WriteLine("Capacity been integer between 1 and 20. Please try again.");
                goto Capacity;
            }

            Console.WriteLine("Add Education ID:");

            var educationList = await _educationService.GetAllAsync();
            foreach (var education in educationList)
            {
                Console.WriteLine($"ID: {education.Id}, Name: {education.Name}");
            }

        EduID: string eduId = Console.ReadLine();
            int educationId;

            if (string.IsNullOrWhiteSpace(eduId) || !int.TryParse(eduId, out educationId))
            {
                Console.WriteLine("Input can't be letter or empty, Please try again.");
                goto EduID;
            }

            var existingEducation = educationList.FirstOrDefault(e => e.Id == educationId);

            if (existingEducation == null)
            {
                Console.WriteLine("Education not found. Please enter a valid education ID.");
                goto EduID;
            }
            try
            {
                DateTime time = DateTime.Now;

                await _groupService.CreateAsync(new Domain.Models.Group { Name = name.Trim().ToLower(), Capacity = capacity, EducationId = educationId, Date = time });

                Console.WriteLine("Group successfully added.");
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
                        Console.WriteLine("Group not found.");
                        goto uId;
                    }

                    Console.WriteLine("Please write new group name (leave empty to keep the old value):");
                    string name = Console.ReadLine();
                    if (string.IsNullOrWhiteSpace(name))
                    {
                        name = existingGroup.Name;
                    }
                    else if (!Regex.IsMatch(name, @"^[\p{L}\p{M}' \.\-]+$"))
                    {
                        Console.WriteLine("Not correct input. Please try again.");
                        continue;
                    }

                    Console.WriteLine("Please write new group capacity (leave empty to keep the old value):");
                    string capacityStr = Console.ReadLine();
                    int capacity;
                    if (string.IsNullOrWhiteSpace(capacityStr))
                    {
                        capacity = existingGroup.Capacity;
                    }
                    else if (!int.TryParse(capacityStr, out capacity))
                    {
                        Console.WriteLine("Not correct input. Please try again.");
                        continue;
                    }

                    Console.WriteLine("Please write new education ID (leave empty to keep the old value):");
                    string eduIdStr = Console.ReadLine();
                    int eduId;
                    if (string.IsNullOrWhiteSpace(eduIdStr))
                    {
                        eduId = existingGroup.EducationId;
                    }
                    else if (!int.TryParse(eduIdStr, out eduId))
                    {
                        Console.WriteLine("Not correct input. Please try again.");
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

        public async Task<List<Domain.Models.Group>> SortWithCapacityAsync()
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
                        Console.WriteLine("Invalid sort type. Please choose correct type 'Asc' or 'Desc'.");
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

    

