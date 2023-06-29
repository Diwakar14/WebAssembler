using WebDataAssember;


internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("---WebData Assembler---");
        string[] testImage = {
            @"F:\Angular\Projects\WebDataAssember\test\_DSC6674.jpg",
            @"F:\Angular\Projects\WebDataAssember\test\(11).jpg",
            @"F:\Angular\Projects\WebDataAssember\test\(24).jpg",
            @"F:\Angular\Projects\WebDataAssember\test\18eighteen.com - Blaire Ivory - Summer Cummin (x35) (September 13th, 2016) (1).jpg",
            @"F:\Angular\Projects\WebDataAssember\test\00059.jpg",
            @"f:\Angular\Projects\WebDataAssember\test\00066.jpg"
        };
        // Operations operation = new Operations();
        // await operation.GetAllFilesAsync(@"F:\ImageServer\source\ImageSet");

        ImageOperations operation = new ImageOperations();
        // operation.ResizeImage(@"F:\Angular\Projects\WebDataAssember\test\1540067414-wUQOGfolkpxN9UPp_filename__MG_2524___12.JPG");


        Console.WriteLine("This is Outer Main Thread: " + Thread.CurrentThread.ManagedThreadId);
        //await operation.ProcessDirectoryAsync("./output/result.json");

        Thread thread = new Thread(new ParameterizedThreadStart(operation.ProcessDirectoryAsync));
        thread.Start(@"F:\Angular\Projects\WebDataAssember\output\result.json");

        Console.WriteLine("Main thread is Free to use");


        // bool exit = false;

        // while (!exit)
        // {
        //     Console.Clear();
        //     Console.WriteLine($"A. Generate the JSON");
        //     Console.WriteLine($"B. Process Directory");
        //     Console.WriteLine($"E. Exit");
        //     var input = Console.ReadKey();

        //     switch (((char)input.Key))
        //     {
        //         case 'A':
        //             {
        //                 System.Console.WriteLine("Processing JSON");
        //                 Console.ReadKey();
        //                 break;
        //             }
        //         case 'B':
        //             {
        //                 System.Console.WriteLine("Processing Image");
        //                 Console.ReadKey();

        //                 break;
        //             }
        //         case 'E':
        //             {
        //                 System.Console.WriteLine("Exiting...");
        //                 Console.ReadKey();

        //                 exit = true;
        //                 break;
        //             }

        //     }
        // }

        // Test
        // int index = 1;
        // foreach (var item in testImage)
        // {

        //     operation.GetProfileImage(
        //         item,
        //         @"F:\Angular\Projects\WebDataAssember\test\thumbnails\" + index + ".jpg",
        //         true
        //     );

        //     index++;
        // }


    }
}