﻿namespace Domain.Entities
{
    public class Client : BaseEntity<Guid>
    {
        public Guid ExternalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string Phone {  get; set; } = string.Empty;
        public string? ImageFileName { get; set; }

        public List<Section>? Section { get; set; }
    }
}
