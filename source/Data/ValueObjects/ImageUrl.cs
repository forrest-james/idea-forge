namespace Data.ValueObjects
{
    public sealed record ImageUrl
    {
        public string Value { get; }

        private ImageUrl(string value)
        {
            Value = value;
        }

        public static ImageUrl Create(string value)
        {
            if (!Uri.TryCreate(value, UriKind.Absolute, out var uri))
                throw new CannotUnloadAppDomainException("Invalid image URL");

            return new ImageUrl(uri.ToString());
        }

        public override string ToString() => Value;
    }
}