using System.Text;
using Newtonsoft.Json;

namespace WebDataAssember
{
    public class FileManager
    {

        public async Task SaveToJsonAsync(IEnumerable<object> data, string path)
        {
            var json = JsonConvert.SerializeObject(data);
            var Utf8Encoder = Encoding.GetEncoding("UTF-8", new EncoderReplacementFallback(string.Empty), new DecoderExceptionFallback());
            var utf8Text = Utf8Encoder.GetString(Utf8Encoder.GetBytes(json));
            var file = File.CreateText("./output/result.json");
            await file.WriteAsync(utf8Text);
            file.Dispose();
        }

        public static async Task<ICollection<DirectoryDetails>> ReadJsonAsync(string path)
        {
            var jsonData = File.ReadAllText(path);
            var json = JsonConvert.DeserializeObject<ICollection<DirectoryDetails>>(jsonData);
            return json;
        }

        public static string CreateThumbnailDirectory(string path)
        {
            var directoryName = Path.GetDirectoryName(path);
            var fileName = Path.GetFileName(path);
            var thumbDir = Path.Combine(directoryName, "thumbnails");
            DirectoryInfo directory = new DirectoryInfo(thumbDir);

            if (!Directory.Exists(thumbDir))
            {
                directory = Directory.CreateDirectory(Path.Combine(directoryName, "thumbnails"));
            }
            return Path.Combine(directory.FullName, fileName);
        }

        public static string CreateProfileDirectory(string path)
        {

            DirectoryInfo dirPath = new DirectoryInfo(path);
            string profileDirPath = Path.Combine(path, "profile");
            if (!Directory.Exists(profileDirPath))
            {
                dirPath = Directory.CreateDirectory(profileDirPath);
            }
            else
            {
                return profileDirPath;
            }

            return dirPath.FullName;
        }
    }
}