using System.Collections.ObjectModel;

namespace WebDataAssember
{
    public class DirectoryDetails
    {
        public string Guid { get; set; }
        public string DirectoryName { get; set; }

        public ICollection<DirectoryDetails> Directories { get; set; }
        public ICollection<FileDetails> Files { get; set; }

        public DirectoryDetails()
        {
            Guid = "";
            DirectoryName = "";
            Directories = new Collection<DirectoryDetails>();
            Files = new Collection<FileDetails>();
        }

    }
}