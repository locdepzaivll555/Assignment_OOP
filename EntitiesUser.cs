namespace CinemaManagement.Entities
{
    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
      
        // TTP = 3 verification codes to see if you are an admin
        public User(string username, string password, string phone, string email)
        {
            Username = username;
            Password = password;
            Email = email;
            Phone = phone;
            
        }
    }
}
