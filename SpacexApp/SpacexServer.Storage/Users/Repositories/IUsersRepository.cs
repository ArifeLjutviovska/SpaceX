namespace SpacexServer.Storage.Users.Repositories
{
    using SpacexServer.Storage.Users.Entities;

    /// <summary>
    /// Defines methods for user data access operations.
    /// </summary>
    public interface IUsersRepository
    {
        /// <summary>
        /// Retrieves a user by their email address.
        /// </summary>
        /// <param name="email">The email address of the user.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<User?> GetByEmailAsync(string email);

        /// <summary>
        /// Retrieves a user by their unique identifier.
        /// </summary>
        /// <param name="id">The user's unique ID.</param>
        /// <returns>The user if found; otherwise, null.</returns>
        Task<User?> GetByIdAsync(int id);

        /// <summary>
        /// Updates an existing user's information.
        /// </summary>
        /// <param name="user">The user entity with updated data.</param>
        Task UpdateAsync(User user);

        /// <summary>
        /// Inserts a new user into the database.
        /// </summary>
        /// <param name="user">The user entity to insert.</param>
        Task InsertAsync(User user);
    }
}