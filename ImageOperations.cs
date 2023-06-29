using System.Drawing;
using Emgu.CV;
using Emgu.CV.Structure;
using ImageMagick;

namespace WebDataAssember
{
    public class ImageOperations
    {

        private CascadeClassifier haarCascade;
        private Image<Bgr, Byte>? detectedFace = null;

        public delegate void ThreadResultCallback(bool result);
        public int profileLimit = 5;

        public ImageOperations()
        {
            LoadImageDetectionModel();
        }

        public void LoadImageDetectionModel(string? HaarCascadePath = null)
        {
            if (HaarCascadePath == null)
            {
                HaarCascadePath = @"F:\Angular\Projects\WebDataAssember\assets\haarcascade_frontalface_default.xml";
            }
            haarCascade = new CascadeClassifier(HaarCascadePath);
        }

        public async void GetProfileImage(string path, string destinationPath, bool passportSize, ThreadResultCallback callback)
        {
            Thread thread = Thread.CurrentThread;
            string message = $"==> Profile - Background: {thread.IsBackground}, Thread Pool: {thread.IsThreadPoolThread}, Thread ID: {thread.ManagedThreadId}";
            Console.WriteLine(message);

            Image<Bgr, Byte> image = new Image<Bgr, byte>(path);
            var bgrFrame = image;

            var portraitSize = new Size(200, 400);
            var squareSize = new Size(150, 200);
            var size = new Size();

            if (passportSize)
                size = portraitSize;
            else
                size = squareSize;


            if (bgrFrame != null)
            {
                try
                {
                    Image<Gray, byte> grayframe = bgrFrame.Convert<Gray, byte>();
                    // Rectangle[] faces = haarCascade.DetectMultiScale(grayframe, 1.2, 10);

                    Rectangle[] faces = haarCascade.DetectMultiScale(grayframe, 1.1, 5); ;
                    if (faces.Count() > 0)
                    {
                        // Console.WriteLine($"{faces.Count()} Faces Dectected.");
                        for (int i = 0; i < faces.Count(); i++)
                        {
                            // Console.WriteLine(current + ": -> " + faces[i].Width);
                            // Console.WriteLine(current + ": -> " + faces[i].Height);
                            // Console.WriteLine(current + ": -> " + faces[i].X);
                            // Console.WriteLine(current + ": -> " + faces[i].Y);


                            faces[i].Width += size.Width;
                            faces[i].Height += size.Height;

                            faces[i].X -= size.Width / 2;//180;
                            faces[i].Y -= size.Height / 2;//180;

                            // faces[i].Y -= 0;

                            faces[i].Size = new Size(faces[i].Width, faces[i].Height);

                            detectedFace = bgrFrame.Copy(faces[i]).Convert<Bgr, byte>();
                            detectedFace.Save(destinationPath);
                            break;
                        }
                        callback(true);
                    }
                    else
                    {
                        Console.WriteLine("No Face Detected.");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Some Error Occured." + ex.ToString());
                }
            }
            callback(false);
        }


        public async void ResizeImage(object? objPath)
        {
            string path = objPath as string;

            Thread thread = Thread.CurrentThread;
            string message = $"==> Resize - Background: {thread.IsBackground}, Thread Pool: {thread.IsThreadPoolThread}, Thread ID: {thread.ManagedThreadId}";
            Console.WriteLine(message);

            if (path != null)
            {

                var img = new MagickImage(path);
                var size = new FileInfo(path).Length / 1024;

                // img.AdaptiveSharpen();
                // img.AutoGamma();
                // img.BrightnessContrast(new Percentage(-10), new Percentage(30));
                // img.AutoLevel();

                if (size > 500)
                {
                    img.Resize(new Percentage(30));
                }
                img.Write(FileManager.CreateThumbnailDirectory(path));
            }
        }

        // This function calls ProcessImage for each directory
        public async void ProcessDirectoryAsync(object? jsonPathObj)
        {
            string jsonPath = jsonPathObj.ToString();
            if (jsonPath != null)
            {
                var json = await FileManager.ReadJsonAsync(jsonPath);
                Console.WriteLine("Called");
                foreach (var directory in json)
                {
                    Console.WriteLine($"Processing: {directory.DirectoryName}");
                    await ProcessImageAsync(directory);
                }
            }
        }

        public async Task ProcessImageAsync(DirectoryDetails directoryDetails)
        {
            ThreadResultCallback threadResult = new ThreadResultCallback(HandleThreadResult);

            // Check directories for files
            if (directoryDetails.Directories.Count > 0)
            {
                foreach (var directory in directoryDetails.Directories)
                {
                    await ProcessImageAsync(directory);
                }
            }

            // Check files in the current directory.
            if (directoryDetails.Files.Count > 0)
            {
                foreach (var file in directoryDetails.Files)
                {
                    Console.WriteLine($"\nProcessing: {file.Name}");
                    var path = Path.Combine(directoryDetails.DirectoryName, file.Name);
                    var fileType = Path.GetExtension(path);
                    if (fileType == ".jpg" || fileType == ".jpeg" || fileType == ".png")
                    {
                        Console.WriteLine("Sub Thread: " + Thread.CurrentThread.ManagedThreadId);

                        string profileDirPath = FileManager.CreateProfileDirectory(directoryDetails.DirectoryName);
                        var profilePath = Path.Combine(profileDirPath, file.Name);
                        if (profileLimit > 0)
                        {
                            // object param = new { path, profilePath, passportSize = true };

                            Thread profileThread = new Thread(() => GetProfileImage(path, profilePath, true, threadResult));

                            if (!profileThread.IsAlive)
                            {
                                profileThread.Start();
                                profileThread.IsBackground = true;
                            }
                            // profileThread.Join();

                            // if (await GetProfileImage(path, profilePath, true))
                            //     profileLimit--;
                        }


                        Thread thread = new Thread(new ParameterizedThreadStart(ResizeImage));
                        thread.Start(path);
                        thread.Join();

                        // await ResizeImage(path);


                        Console.WriteLine("Released Thread: " + Thread.CurrentThread.ManagedThreadId);


                    }
                }
            }
        }

        private void HandleThreadResult(bool result)
        {
            Console.WriteLine("Result of from the thread");
            if (result)
                profileLimit--;
        }
    }
}