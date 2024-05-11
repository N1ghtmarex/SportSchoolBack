﻿namespace Domain.Entities
{
    public class Coach : BaseEntity<Guid>
    {
        public Guid ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public List<Section>? Section { get; set; } = new List<Section>();
    }
}
