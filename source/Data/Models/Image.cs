using Data.ValueObjects;

namespace Data.Models
{
    public class Image
    {
        public Guid Id { get; private set; }
        public ImageUrl Url { get; private set; }
        public int Order { get; private set; }

        private Image() { }

        internal Image(ImageUrl url, int order)
        {
            Id = Guid.NewGuid();
            Url = url;
            Order = order;
        }

        internal void SetOrder(int order)
        {
            Order = order;
        }
    }
}