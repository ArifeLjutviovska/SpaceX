namespace SpacexServer.Storage.Users.Entities
{
    public class User
    {
        public int Id { get; protected internal set; }

        public DateTime CreatedOn { get; protected internal set; }

        public DateTime? DeletedOn { get; protected internal set; }

        public string Email { get; protected internal set; } = string.Empty;

        public string FirstName { get; protected internal set; } = string.Empty;

        public string LastName { get; protected internal set; } = string.Empty;

        public string Password { get; protected internal set; } = string.Empty;

        public static User Create(string email,
                                  string firstName,
                                  string lastName,
                                  string password)
        {
            return new User
            {
                CreatedOn = DateTime.UtcNow,
                DeletedOn = null,
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                Password = password
            };
        }
    }
}