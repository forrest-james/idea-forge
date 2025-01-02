public class Palette {
    public int Id { get; set; }

    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Color code must be exactly 6 hexadecimal characters long between 000000 - FFFFFF")]
    public string PrimaryColor { get; set; }
    
    [Required]
    [StringLength(6, MinimumLength = 6, ErrorMessage = "Color code must be exactly 6 hexadecimal characters long between 000000 - FFFFFF")]
    public string SecondaryColor { get; set; }

    [StringLength(6, MinimumLength = 6, ErrorMessage = "Color code must be exactly 6 hexadecimal characters long between 000000 - FFFFFF")]
    public string AccentColor { get; set; }
}