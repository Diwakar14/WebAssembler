namespace WebDataAssember
{
    public class FileDetails
    {
        public string Guid { get; set; }
        public string Name { get; set; }
        public float Size { get; set; }
        public FileDetails()
        {
            Guid = "";
            Name = "";
        }
    }
}