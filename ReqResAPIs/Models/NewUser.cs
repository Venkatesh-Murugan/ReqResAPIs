
namespace ReqResAPIs.Models
{
    public class NewUser
    {

        public string? name { get; set; }

        public string? job { get; set; }

    }

    public class UserResponse
    {
        public string? id { get; set; }
        public string? name { get; set; }
        public string? job { get; set; }
        public DateTimeOffset? createdAt { get; set; }
        public DateTimeOffset? updatedAt { get; set; }
    }
}
