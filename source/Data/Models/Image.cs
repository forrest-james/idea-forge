using Data.ValueObjects;

namespace Data.Models
{
    public class Image
    {
        public Guid Id { get; private set; }
        public Guid AppDesignId { get; private set; }
        public ImageUrl Url { get; private set; }
        public int Order { get; private set; }

        private Image() { }

        internal Image(Guid appDesignId, ImageUrl url, int order)
        {
            Id = Guid.NewGuid();
            AppDesignId = appDesignId;
            Url = url;
            Order = order;
        }

        internal void SetOrder(int order)
        {
            Order = order;
        }
    }
}