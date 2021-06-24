using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;

namespace SetAccessRights
{
    public class AccessRightsProvider
    {
        public void SetRightsToAll()
        {
            //string folder = GetExecutionFolder();
            //if (Directory.Exists(folder))
            //    AddUsersAndPermissions(folder, "Everyone", FileSystemRights.FullControl, AccessControlType.Allow);
            //List<string> files = GetAllFiles(folder);

            //foreach (string file in files)
            //{
            //    try
            //    {
            //        SetAccessRights(file);
            //    }
            //    catch
            //    {
            //        // Discard
            //    }
            //}

            string commonAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Scan";
            if (Directory.Exists(commonAppDataFolder))
                SetAccessRightsDirectory(commonAppDataFolder);
            //files = GetAllFiles(commonAppDataFolder);

            //foreach (string file in files)
            //{
            //    try
            //    {
            //        SetAccessRights(file);
            //    }
            //    catch
            //    {
            //        // Discard
            //    }
            //}

            //string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            //if (Directory.Exists(appDataFolder))
            //    AddUsersAndPermissions(appDataFolder, "Everyone", FileSystemRights.FullControl, AccessControlType.Allow);
            //files = GetAllFiles(appDataFolder);

            //foreach (string file in files)
            //{
            //    try
            //    {
            //        SetAccessRights(file);
            //    }
            //    catch
            //    {
            //        // Discard
            //    }
            //}
        }

        private string GetExecutionFolder()
        {
            return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        /// <summary>
        /// Returns all files in folder recursively
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<string> GetAllFiles(string path)
        {
            List<string> files = new List<string>();
            Stack<string> directoryStack = new Stack<string>();

            directoryStack.Push(path);
            while (directoryStack.Count > 0)
            {
                string dir = directoryStack.Pop();

                try
                {
                    files.AddRange(Directory.GetFiles(dir, "*.*"));

                    foreach (string directory in Directory.GetDirectories(dir))
                    {
                        directoryStack.Push(directory);
                    }
                }
                catch
                {
                    // Discard
                }
            }

            return files;
        }

        /// <summary>
        /// Set full access rights for specified file
        /// </summary>
        /// <param name="file"></param>
        private void SetAccessRights(string file)
        {
            FileSecurity fileSecurity = File.GetAccessControl(file);
            AuthorizationRuleCollection rules = fileSecurity.GetAccessRules(true, true, typeof(NTAccount));

            foreach (FileSystemAccessRule rule in rules)
            {
                string name = rule.IdentityReference.Value;

                if (rule.FileSystemRights != FileSystemRights.FullControl)
                {
                    FileSecurity newFileSecurity = File.GetAccessControl(file);
                    FileSystemAccessRule newRule = new FileSystemAccessRule(name, FileSystemRights.FullControl, AccessControlType.Allow);
                    newFileSecurity.AddAccessRule(newRule);
                    File.SetAccessControl(file, newFileSecurity);
                }
            }
        }

        private void SetAccessRightsDirectory(string directory)
        {
            DirectorySecurity directorySecurity = Directory.GetAccessControl(directory);
            AuthorizationRuleCollection rules = directorySecurity.GetAccessRules(true, true, typeof(NTAccount));

            foreach (FileSystemAccessRule rule in rules)
            {
                string name = rule.IdentityReference.Value;

                if (rule.FileSystemRights != FileSystemRights.FullControl)
                {
                    //FileSecurity newFileSecurity = File.GetAccessControl(file);
                    //FileSystemAccessRule newRule = new FileSystemAccessRule(name, FileSystemRights.FullControl, AccessControlType.Allow);
                    //newFileSecurity.AddAccessRule(newRule);
                    //File.SetAccessControl(file, newFileSecurity);

                    AddUsersAndPermissions(directory, name, FileSystemRights.FullControl, AccessControlType.Allow);
                }
            }
        }

        public void AddUsersAndPermissions(string DirectoryName, string UserAccount, FileSystemRights UserRights, AccessControlType AccessType)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(DirectoryName);
            DirectorySecurity dirSecurity = directoryInfo.GetAccessControl();

            dirSecurity.AddAccessRule(new FileSystemAccessRule(UserAccount, UserRights, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.NoPropagateInherit, AccessType));

            directoryInfo.SetAccessControl(dirSecurity);
        }
    }
}
