namespace SpacexServer.Entities.User.Domain
{
    using SpacexServer.Entities.Common.Entities;

    public class User : Entity
    {
        public string Name { get; private set; } //add valueObject

        public static User Create(string name,
                                  DateTime createdOn)
        {
            return new User
            {
                Name = name,
                CreatedOn = createdOn,
                DeletedOn = null
            };
        }
    }
}