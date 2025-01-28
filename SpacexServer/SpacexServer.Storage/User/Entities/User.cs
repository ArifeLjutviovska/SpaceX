namespace SpacexServer.Storage.User.Entities
{
    using SpacexServer.Storage.Common.Entities;

    public class User : Entity
    {
        public string Name { get; protected internal set; }

        public static User Create(
            int id,
            DateTime createdOn,
            DateTime? deletedOn,
            string name)
        {
            return new User
            {
                Id = id,
                CreatedOn = createdOn,
                DeletedOn = deletedOn,
                Name = name
            };
        }
    }
}