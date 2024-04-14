using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Enams
{
    public enum OperationType
    {
        EducationCreate = 1,
        EducationDelete,
        EducationUpdate,
        GetEducationById,
        GetAllEducations,
        SearchEducations,
        GetAllEducationsWhithGroups,        
        SortEducationsWhithCreateDate,
        GroupCreate,
        GroupDelete,
        GroupUpdate,
        GetGroupById,
        GetAllGroups,
        SearchGroups,
        GetAllGroupsWhithEducationId,
        FilteGroupsWhithEducationName,
        SortGroupsWhithCapacity
    }
}

