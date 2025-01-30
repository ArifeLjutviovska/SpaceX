namespace SpacexServer.Storage.Common.Entities
{
    public abstract class Entity
    {
        public int Id { get; protected internal set; }

        public DateTime CreatedOn { get; protected internal set; }

        public DateTime? DeletedOn { get; protected internal set; }

        protected Entity()
        {
        }
    }
}