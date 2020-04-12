using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Enums
{
    public enum Roles
    {
        [Display(Name = "None"), Description("None")]
        None = 0,

        [Display(Name = "User"), Description("User")]
        User = 1,

        [Display(Name = "Admin"), Description("Admin")]
        Admin = 2,

        [Display(Name = "Manager"), Description("Manager")]
        Manager = 3
    }
}