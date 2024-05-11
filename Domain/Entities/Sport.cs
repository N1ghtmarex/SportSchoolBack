namespace Domain.Entities
{
    public class Sport: BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public List<Section>? Section { get; set; }
    }
}
