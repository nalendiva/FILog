namespace FILog.Models
{
    public class MenuItem
    {
        public string Title { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }
        public List<MenuItem> SubItems { get; set; } = new List<MenuItem>();
    }


    public class SubMenuItem
    {
        public string Label { get; set; }
        public string Link { get; set; }
        public bool Active { get; set; }
    }
}
