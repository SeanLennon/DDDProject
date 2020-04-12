using Microsoft.AspNetCore.Identity;

namespace Domain.Entities
{
    public class Role : IdentityRole
    {
        public Role(string name) 
        {
            Name = name;
            NormalizedName = Name.ToUpper();
        }

        public override string ToString() => Name;
    }
}