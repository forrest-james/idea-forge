using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels.Account
{
    public sealed class SetPasswordVm
    {
        [Required]
        public string UserId { get; set; } = "";

        [Required]
        public string Token { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; } = "";

        [Required]
        [DataType(DataType.Password)]
        [Compare(nameof(Password))]
        public string ConfirmPassword { get; set; } = "";
    }
}