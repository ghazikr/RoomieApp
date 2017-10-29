using RoomieApp.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(FileHelper_Android))]
namespace RoomieApp.Droid
{
    internal class FileHelper_Android : IFileHelper
    {
        public void Copy(string fromFile, string toFile)
        {
            System.IO.File.Copy(fromFile, toFile);
        }
    }
}