using RoomieApp.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper_iOS))]
namespace RoomieApp.iOS
{
    internal class FileHelper_iOS
    {
        public void Copy(string fromFile, string toFile)
        {
            System.IO.File.Copy(fromFile, toFile);
        }
    }
}