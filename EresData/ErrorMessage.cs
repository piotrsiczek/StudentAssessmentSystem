using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EresData
{
    public class ErrorMessage
    {
        public static string EDIT_GRADES_NAME_EXISTS = "Provided name already exists in this realization.";
        public static string EDIT_GRADES_COULD_NOT_FIND_GRADES_ID = "Provided grade id couldn't be find.";
        public static string ADD_GRADES_COULD_NOT_FIND_REALIZATIONS_ID = "Provided realization id couldn't be find.";
        public static string ADD_GRADES_NAME_EXISTS = "Provided name already exists in this realization.";
        public static string REMOVE_GRADES_COULD_NOT_FIND_GRADES_ID = "Provided grade id couldn't be find.";
        public static string REMOVE_GRADES_CONCURRENCY = "You are trying to modify outdated data.";
    }
}
