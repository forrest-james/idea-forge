using Data.Exceptions;
using Data.ValueObjects;

namespace Data.Models
{
    public class AppDesign
    {
        private readonly List<Image> _images = new();

        public Guid Id { get; private set; }
        public Challenge Challenge { get; private set; }
        public Palette Palette { get; private set; }
        public string Description { get; private set; }
        public IReadOnlyCollection<Image> Images => _images.OrderBy(i => i.Order).ToList();

        private AppDesign() { }

        public AppDesign(Challenge challenge, Palette palette, string description)
        {
            Id = Guid.NewGuid();
            Challenge = challenge;
            Palette = palette;
            Description = description;
        }

        public void AddImage(ImageUrl url)
        {
            var order = _images.Count;
            _images.Add(new Image(url, order));
        }

        public void RemoveImage(Guid imageId)
        {
            var image = _images.SingleOrDefault(i => i.Id == imageId);
            if (image is null)
                throw new DomainException("Image not found");

            _images.Remove(image);
            ReorderImages();
        }

        public void ReorderImages()
        {
            for (int i = 0; i < _images.Count; i++)
            {
                _images[i].SetOrder(i);
            }
        }
    }
}