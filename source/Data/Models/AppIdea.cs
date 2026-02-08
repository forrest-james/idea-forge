using Data.Enums;

namespace Data.Models
{
    public class AppIdea
    {
        public Guid Id { get; private set; }
        public AppType Type { get; private set; }
        public AppCategory Category { get; private set; }
        public string Description { get; private set; }

        private AppIdea() { }

        public AppIdea(AppType type, AppCategory category, string description)
        {
            Id = Guid.NewGuid();
            Type = type;
            Category = category;
            Description = description;
        }

        public void Update(AppType type, AppCategory category, string description)
        {
            Type = type;
            Category = category;
            Description = description;
        }
    }
}