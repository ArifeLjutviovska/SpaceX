namespace SpacexServer.Storage.User.Factories
{
    using SpacexServer.Entities.User.Domain;

    public static class UserFactory
    {
        #region From Domain to DB

        public static Entities.User ToUserDb(this User user)
        {


            return Entities.User.Create(id: user.Id,
                                        createdOn: user.CreatedOn,
                                        deletedOn: user.DeletedOn,
                                        name: user.Name);
        }

        #endregion
    }
}