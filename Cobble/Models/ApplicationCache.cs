namespace Cobble.Models
{
    public class ApplicationCache
    {
        public DateTime LastTime { get; set; }
        public static ApplicationCache Default { get; } = new()
        {
            LastTime = DateTime.Now
        };
    }
}
