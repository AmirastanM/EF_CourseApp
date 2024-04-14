using Domain.Models;
using EF_CourseApp.Controllers;
using Service.Enams;
using Service.Extensions;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection.Emit;

EducationController educationController = new EducationController();
GroupController groupController = new GroupController();


while (true)
{
GetMenues();

Operation: string operationStr = Console.ReadLine();

    int operation;

    bool isCorrectOperationFormat = int.TryParse(operationStr, out operation);

    if (isCorrectOperationFormat)
    {
        switch (operation)
        {
            case (int)OperationType.EducationCreate:
               await educationController.CreateEducationAsync();
                break;
            case (int)OperationType.EducationDelete:
                await educationController.DeleteEducationAsync();
                break;
            case (int)OperationType.EducationUpdate:
                await educationController.UpdateEducationAsync();
                break;
            case (int)OperationType.GetEducationById:
                await educationController.GetByIdAsync();
                break;
            case (int)OperationType.GetAllEducations:
                await educationController.GetAllAsync();
                break;
            case (int)OperationType.SearchEducations:
                await educationController.SearchAsync();
                break;
            case (int)OperationType.GetAllEducationsWhithGroups:
                await educationController.GetAllWithGroupsAsync();
                break;
            case (int)OperationType.SortEducationsWhithCreateDate:
                await educationController.SortWithCreateDateAsync();
                break;
            case (int)OperationType.GroupCreate:
                await groupController.CreateAsync();
                break;
            case (int)OperationType.GroupDelete:
                await groupController.DeleteAsync();
                break;
            case (int)OperationType.GroupUpdate:
                await groupController.UpdateAsync();
                break;
            case (int)OperationType.GetGroupById:
                await groupController.GetByIdAsync();
                break;
            case (int)OperationType.GetAllGroups:
                await groupController.GetAllAsync();
                break;
            case (int)OperationType.SearchGroups:
                await groupController.SearchAsync();
                break;
            case (int)OperationType.GetAllGroupsWhithEducationId:
                await groupController.GetAllWithEducationIdAsync();
                break;
            case (int)OperationType.FilteGroupsWhithEducationName:
                await groupController.FilterByEducationNameAsync();
                break;
            case (int)OperationType.SortGroupsWhithCapacity:
                await groupController.SortWithCapacityAsync();
                break;



            default:
                ConsoleColor.Red.WriteConsole("This operation is not avialable, please choose correct operation again");
                goto Operation;
        }
    }
    else
    {
        ConsoleColor.Red.WriteConsole("Operation format is wrong, please add operation again");
        goto Operation;
    }
}







void GetMenues()
{
    Console.WriteLine("Please choose operation :" +  
        "\n    1 - EducationCreate                                    9 - GroupCreate\n" +
        "    2 - EducationDelete                                    10 - GroupDelete\n" +
        "    3 - EducationUpdate                                    11 - GroupUpdate\n" +
        "    4 - GetEducationById                                   12 - GetGroupById\n" +
        "    5 - GetAllEducations                                   13 - GetAllGroups\n" +
        "    6 - SearchEducations                                   14 - SearchGroups\n" +
        "    7 - GetAllEducationsWhithGroups                        15 - GetAllGroupsWhithEducationId\n" +
        "    8 - SortEducationsWhithCreateDate                      16 - FilteGroupsWhithEducationName\n" +
        "                                                           17 - SortGroupsWhithCapacity");

}


       
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        