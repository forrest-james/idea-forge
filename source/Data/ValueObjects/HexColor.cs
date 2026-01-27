using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Data.ValueObjects
{
    public readonly struct HexColor : IEquatable<HexColor>
    {
        private static readonly Regex HexRegex = new("^#?[0-9A-Fa-f]{6}$", RegexOptions.Compiled);

        public string Value { get; }

        private HexColor(string value) => Value = value;

        public static HexColor Parse(string input)
        {
            if (!TryParse(input, out var color))
                throw new ArgumentException($"Invalid hex color: {input}");
            return color;
        }

        private static bool TryParse(string? input, out HexColor color)
        {
            color = default;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            if(!HexRegex.IsMatch(input))
                return false;

            color = new HexColor(Normalize(input));
            return true;
        }

        private static string Normalize(string input)
        {
            var hex = input.StartsWith("#") ? input[1..] : input;
            return $"#{hex.ToUpperInvariant()}";
        }

        public override string ToString() => Value;

        public bool Equals(HexColor other) => Value == other.Value;

        public override bool Equals(object? obj) => obj is HexColor other && Equals(other);

        public override int GetHashCode() => Value.GetHashCode();

        public static bool operator ==(HexColor left, HexColor right) => left.Equals(right);
        public static bool operator !=(HexColor left, HexColor right) => !left.Equals(right);

    }
}