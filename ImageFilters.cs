using ImageMagick;

namespace WebDataAssember
{
    public class ImageFilter
    {
        private readonly MagickImage image;

        public ImageFilter(MagickImage image)
        {
            this.image = image;
        }

        public void AutoGamma()
        {
            image.AutoGamma();
        }

        public void AdaptiveSharpen()
        {
            image.AdaptiveSharpen();
        }
        public void BrightnessContrast(Percentage brightness, Percentage contrast)
        {
            image.BrightnessContrast(brightness, contrast);
        }
        public void AutoLevel()
        {
            image.AutoLevel();
        }

        public void Resize(string path)
        {
            var size = new FileInfo(path).Length / 1024;
            if (size > 500)
            {
                image.Resize(new Percentage(30));
            }
            image.WriteAsync(FileManager.CreateThumbnailDirectory(path));
        }
    }
}