namespace JwtAuth
{
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }

    public class UserRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public static class UserList
    {
        public static List<User> AllUsers => new()
        {
            new User { UserName = "srinivas", Password = "myTestPassword", Role = "Admin" },
            new User { UserName = "prasad", Password = "myTestPassword", Role = "user" }
        };
    }
}
