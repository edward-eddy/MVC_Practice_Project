namespace MVC_Practice_Project.PL.Helpers
{
    public static class DocumentSettings
    {
        // 1. Uplode
        // ImageName
        public static string Upload(IFormFile file, string folderName)
        {
            // 1. Get Folder Location

            //string folderPath = "X:\Route\07 MVC\MVC_Practice_Project\MVC_Practice_Project.PL\wwwroot\" + folderName;

            //var folderPath = Directory.GetCurrentDirectory() + "\\wwwroot\\files\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName);

            // 2. Get File Name And Make It Unique

            var fileName = $"{Guid.NewGuid()}-{file.FileName}";

            // File Path

            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);

            file.CopyTo(fileStream);

            return fileName;

        }

        // 2. Delete
        public static void Delete(string fileName, string folderName)
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\files", folderName, fileName);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }
    }
}
