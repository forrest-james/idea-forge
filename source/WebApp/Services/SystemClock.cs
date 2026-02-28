using Application.Common.Interfaces;

namespace WebApp.Services
{
    public sealed class SystemClock : IClock
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}