using System.Collections.ObjectModel;
using System.Text;
using Newtonsoft.Json;

namespace WebDataAssember
{

    public class Operations
    {

        public void GetAllFileInDirectory(string path, ICollection<DirectoryDetails> container)
        {

            if (Directory.Exists(path))
            {
                DirectoryDetails directoryDetails = new DirectoryDetails();
                directoryDetails.DirectoryName = path;
                directoryDetails.Guid = Guid.NewGuid().ToString().Split('-')[0].ToString();

                var files = Directory.GetFiles(path);
                var directory = Directory.GetDirectories(path);

                if (directory.Length > 0)
                {
                    ICollection<DirectoryDetails> dirDetails = new Collection<DirectoryDetails>();
                    foreach (var drPath in directory)
                    {
                        GetAllFileInDirectory(drPath, dirDetails);

                        if (files.Length > 0)
                        {
                            dirDetails.Add(new DirectoryDetails
                            {
                                Guid = Guid.NewGuid().ToString().Split('-')[0].ToString(),
                                DirectoryName = drPath
                            });
                        }
                    }

                    directoryDetails.Directories = dirDetails;
                }


                if (files.Length > 0)
                {
                    ICollection<FileDetails> fileDetails = new Collection<FileDetails>();
                    foreach (var item in files)
                    {
                        fileDetails.Add(new FileDetails
                        {
                            Guid = Guid.NewGuid().ToString().Split('-')[0].ToString(),
                            Name = Path.GetFileName(item),
                            Size = new FileInfo(item).Length / 1000000
                        });
                    }
                    directoryDetails.DirectoryName = path;
                    directoryDetails.Files = fileDetails;
                }
                container.Add(directoryDetails);
            }
        }



        public async Task GetAllFilesAsync(string path)
        {
            var allDirectory = Directory.GetDirectories(path);
            ICollection<DirectoryDetails> directories = new Collection<DirectoryDetails>();

            foreach (var item in allDirectory)
            {
                GetAllFileInDirectory(item, directories);
            }
            FileManager fileManager = new FileManager();
            await fileManager.SaveToJsonAsync(directories, "./output/result.json");
        }




    }

}