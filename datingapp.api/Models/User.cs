namespace datingapp.api.Models
{
    public class User
    {
        public int id { get; set; }

        public string UserName { get; set; }

        public byte[] PasswordHash { get; set; }

        
        public byte[] PasswordSalt { get; set; }
    }
}