using Domain.Models;
using EF_CourseApp.Controllers;
using Service.Enams;
using System.Runtime.InteropServices;

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
                educationController.CreateAsync();
                break;
            case (int)OperationType.EducationDelete:
                educationController.DeleteAsync();
                break;
            case (int)OperationType.EducationUpdate:
                educationController.UpdateAsync();
                break;
            case (int)OperationType.GetEducationById:
                educationController.GetByIdAsync();
                break;
            case (int)OperationType.GetAllEducations:
                educationController.GetAllAsync();
                break;
            case (int)OperationType.SearchEducations:
                educationController.SearchAsync();
                break;
            case (int)OperationType.GetAllEducationsWhithGroups:
                educationController.GetAllWithGroupsAsync();
                break;
            case (int)OperationType.SortEducationsWhithCreateDate:
                //--
                break;
            case (int)OperationType.GroupCreate:
                groupController.CreateAsync();
                break;
            case (int)OperationType.GroupDelete:
                groupController.DeleteAsync();
                break;
            case (int)OperationType.GroupUpdate:
                groupController.UpdateAsync();
                break;
            case (int)OperationType.GetGroupById:
                groupController.GetByIdAsync();
                break;
            case (int)OperationType.GetAllGroups:
                groupController.GetAllAsync();
                break;
            case (int)OperationType.SearchGroups:
                groupController.SearchAsync();
                break;
            case (int)OperationType.GetAllGroupsWhithEducationId:
                groupController.GetAllWithEducationIdAsync();
                break;
            case (int)OperationType.FilteGroupsWhithEducationName:
                groupController.FilterByEducationNameAsync();
                break;
            case (int)OperationType.SortGroupsWhithCapacity:
                groupController.SortWithCapacityAsync();
                break;



            default:
                Console.WriteLine ("This operation is not avialable, please choose correct operation again");
                goto Operation;

        }

    }
    else
    {
        Console.WriteLine ("Operation format is wrong, please add operation again");
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


       
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        