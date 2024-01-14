using System.Security.Claims;
using SMSClient.Constants;
using SMSClient.Model;

namespace SMSClient.Model
{
    public partial class ClaimsList
    {
         public static List<CustomRoleClaims> CustomRoleClaims = new List<CustomRoleClaims>
            {
             new CustomRoleClaims
            {
                DisplayClaimName = "Create Role",
                AccessLevel = AccessLevels.Create,
                Module = Modules.Role
            },
            new CustomRoleClaims
            {
                DisplayClaimName = "View Role",
                AccessLevel = AccessLevels.View,
                Module = Modules.Role
            },
             new CustomRoleClaims
            {
                DisplayClaimName = "Edit Role",
                AccessLevel = AccessLevels.Edit,
                Module = Modules.Role
            },
              new CustomRoleClaims
            {
                DisplayClaimName = "Delete Role",
                AccessLevel = AccessLevels.Delete,
                Module = Modules.Role
            },
              new CustomRoleClaims
            {
                DisplayClaimName = "Create User",
                AccessLevel = AccessLevels.Create,
                Module = Modules.User
            },
            new CustomRoleClaims
            {
                DisplayClaimName = "View User",
                AccessLevel = AccessLevels.View,
                Module = Modules.User
            },
             new CustomRoleClaims
            {
                DisplayClaimName = "Edit User",
                AccessLevel = AccessLevels.Edit,
                Module = Modules.User
            },
              new CustomRoleClaims
            {
                DisplayClaimName = "Delete User",
                AccessLevel = AccessLevels.Delete,
                Module = Modules.User
            },
              new CustomRoleClaims
            {
                DisplayClaimName = "Create Department",
                AccessLevel = AccessLevels.Create,
                Module = Modules.Department
            },
            new CustomRoleClaims
            {
                DisplayClaimName = "View Department",
                AccessLevel = AccessLevels.View,
                Module = Modules.Department
            },
             new CustomRoleClaims
            {
                DisplayClaimName = "Edit Department",
                AccessLevel = AccessLevels.Edit,
                Module = Modules.Department
            },
              new CustomRoleClaims
            {
                DisplayClaimName = "Delete Department",
                AccessLevel = AccessLevels.Delete,
                Module = Modules.Department
            },
              new CustomRoleClaims
            {
                DisplayClaimName = "Create Class",
                AccessLevel = AccessLevels.Create,
                Module = Modules.Class
            },
            new CustomRoleClaims
            {
                DisplayClaimName = "View Class",
                AccessLevel = AccessLevels.View,
                Module = Modules.Class
            },
             new CustomRoleClaims
            {
                DisplayClaimName = "Edit Class",
                AccessLevel = AccessLevels.Edit,
                Module = Modules.Class
            },
              new CustomRoleClaims
            {
                DisplayClaimName = "Delete Class",
                AccessLevel = AccessLevels.Delete,
                Module = Modules.Class
            },
            new CustomRoleClaims
            {
                DisplayClaimName = "Create Course",
                AccessLevel = AccessLevels.Create,
                Module = Modules.Course,
            },
            new CustomRoleClaims
            {
                DisplayClaimName = "View Course",
                AccessLevel = AccessLevels.View,
                Module = Modules.Course
            },
             new CustomRoleClaims
            {
                DisplayClaimName = "Edit Course",
                AccessLevel = AccessLevels.Edit,
                Module = Modules.Course
            },
              new CustomRoleClaims
            {
                DisplayClaimName = "Delete Course",
                AccessLevel = AccessLevels.Delete,
                Module = Modules.Course
            },

                   new CustomRoleClaims
            {
                DisplayClaimName = "Create Student",
                AccessLevel = AccessLevels.Create,
                Module = Modules.Student
            },
            new CustomRoleClaims
            {
                DisplayClaimName = "View Student",
                AccessLevel = AccessLevels.View,
                Module = Modules.Student
            },
             new CustomRoleClaims
            {
                DisplayClaimName = "Edit Student",
                AccessLevel = AccessLevels.Edit,
                Module = Modules.Student
            },
              new CustomRoleClaims
            {
                DisplayClaimName = "Delete Student",
                AccessLevel = AccessLevels.Delete,
                Module = Modules.Student
            },
            new CustomRoleClaims
            {
                DisplayClaimName = "Create Teacher",
                AccessLevel = AccessLevels.Create,
                Module = Modules.Teacher
            },
            new CustomRoleClaims
            {
                DisplayClaimName = "View Teacher",
                AccessLevel = AccessLevels.View,
                Module = Modules.Teacher
            },
             new CustomRoleClaims
            {
                DisplayClaimName = "Edit Teacher",
                AccessLevel = AccessLevels.Edit,
                Module = Modules.Teacher
            },
              new CustomRoleClaims
            {
                DisplayClaimName = "Delete Teacher",
                AccessLevel = AccessLevels.Delete,
                Module = Modules.Teacher
            },



        };
    }
}
