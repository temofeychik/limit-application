using LimiterApplication.Data.Models.Base;

namespace LimiterApplication.Data.Models
{
    public class User : Entity
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
