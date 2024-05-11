namespace Domain.Entities
{
    public class Room : BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public List<Section>? Section { get; set; }
    }
}
