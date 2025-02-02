namespace SpacexServer.Storage.Users.Entities
{
    /// <summary>
    /// Represents a user entity in the system.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Gets the unique identifier for the user.
        /// </summary>
        public int Id { get; protected internal set; }

        /// <summary>
        /// Gets the timestamp when the user was created.
        /// </summary>
        public DateTime CreatedOn { get; protected internal set; }

        /// <summary>
        /// Gets the timestamp when the user was deleted (if applicable).
        /// </summary>
        public DateTime? DeletedOn { get; protected internal set; }

        /// <summary>
        /// Gets the user's email address.
        /// </summary>
        public string Email { get; protected internal set; } = string.Empty;

        /// <summary>
        /// Gets the user's first name.
        /// </summary>
        public string FirstName { get; protected internal set; } = string.Empty;

        /// <summary>
        /// Gets the user's last name.
        /// </summary>
        public string LastName { get; protected internal set; } = string.Empty;

        /// <summary>
        /// Gets the hashed password of the user.
        /// </summary>
        public string Password { get; protected internal set; } = string.Empty;

        /// <summary>
        /// Creates a new user instance.
        /// </summary>
        /// <param name="email">The user's email address.</param>
        /// <param name="firstName">The user's first name.</param>
        /// <param name="lastName">The user's last name.</param>
        /// <param name="password">The hashed password.</param>
        /// <returns>A new <see cref="User"/> instance.</returns>
        public static User Create(string email, string firstName, string lastName, string password)
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

        /// <summary>
        /// Updates the user's password.
        /// </summary>
        /// <param name="password">The new hashed password.</param>
        public void UpdatePassword(string password)
        {
            Password = password;
        }
    }
}