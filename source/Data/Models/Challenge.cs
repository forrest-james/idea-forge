using Data.Exceptions;

namespace Data.Models
{
    public class Challenge
    {
        public Guid Id { get; private set; }       
        public string Name { get; private set; }
        public AppIdea AppIdea { get; private set; }
        public Palette Palette { get; private set; }

        private Challenge() { }

        public Challenge(string name, AppIdea appIdea, Palette palette)
        {
            Id = Guid.NewGuid();
            Name = name;
            AppIdea = appIdea;
            Palette = palette;
        }

        public void Rename(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new DomainException("Name is required.");
            Name = name.Trim();
        }
    }
}