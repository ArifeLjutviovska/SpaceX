namespace SpacexServer.Entities.Common.Entities
{
    using System;

    public abstract class Entity
    {

        public int Id { get; protected set; }

        public DateTime CreatedOn { get; protected set; }

        public DateTime? DeletedOn { get; protected set; }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }

            if (obj is Entity)
            {
                return Id.Equals((obj as Entity).Id);
            }

            return false;
        }

        public static bool operator true(Entity baseEntity)
        {
            return baseEntity == null;
        }

        public static bool operator false(Entity baseEntity)
        {
            return baseEntity != null;
        }

        public void MarkDeleted(DateTime deletedOn)
        {
            DeletedOn = deletedOn;
        }
    }
}