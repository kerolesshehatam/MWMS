namespace MWMS.Services.Maintenance.Doamin.Entities
{
    public class Entity<TId>
    {
        public TId Id { get; private set; }

        public Entity(TId id)
        {
            Id = id;
        }
    }
}
