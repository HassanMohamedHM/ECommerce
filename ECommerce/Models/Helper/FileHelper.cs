using System.IO;
using System.Web;

namespace ECommerce.Models.Helper
{
    public static class FileHelper
    {
        /// <summary>
        /// This Method Used to Upload Files to the Server.
        /// </summary>
        /// <param name="SourcePath">The File Source Path.</param>
        /// <param name="Folder">The Folder Name on the Server.</param>
        /// <param name="FileName">The Name of the File on the Server.</param>
        /// <returns>return the File New Name With Extension.</returns> 
        public static string UploadFile(HttpPostedFileBase SourcePath, string Folder,string FileName)
        {
            string FileNameWithExtension = $"{FileName}.{GetFileExtension(SourcePath.FileName)}";
            string DestinationPath = $"~/{Folder}/{FileNameWithExtension}";
            if (CheckFileExistOrNot(DestinationPath))
            {
                DeleteExistFile(DestinationPath);
            }
            SourcePath.SaveAs(HttpContext.Current.Server.MapPath(DestinationPath));
            return FileNameWithExtension;
        }

        /// <summary>
        /// This Method Used to get File Extenion.
        /// </summary>
        /// <param name="FilePath">The File Physical Path.</param>
        /// <returns>return the File Extension.</returns>
        private static string GetFileExtension(string FilePath)
            => FilePath.Split('.')[FilePath.Split('.').Length - 1];
        /// <summary>
        /// This Method used to check if the file exist on the given path or not.
        /// </summary>
        /// <param name="Path">The file physical Path.</param>
        /// <returns>return true if file Exist ,False if not Exist.</returns>
        private static bool CheckFileExistOrNot(string Path) => File.Exists(Path);
        /// <summary>
        /// This Method used to Delete the file on the given path.
        /// </summary>
        /// <param name="Path">The file physical Path.</param>
        private static void DeleteExistFile(string Path) => File.Delete(Path);
    }
}