using System.Collections.ObjectModel;

namespace WebDataAssember
{
    public class ImageFile : FileDetails
    {
        public string DominantColor { get; set; }
        public bool IsPortrait { get; set; }
        public string Dimensions { get; set; }
        public ICollection<ImageFile> Thumbnails { get; set; }
        public ICollection<Tag> Tags { get; set; }

        public ImageFile()
        {
            DominantColor = "";
            IsPortrait = false;
            Dimensions = "";
            Thumbnails = new Collection<ImageFile>();
            Tags = new Collection<Tag>();
        }
    }
}