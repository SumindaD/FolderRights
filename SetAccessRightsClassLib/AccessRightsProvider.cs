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
            string commonAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + "\\Scan";
            if (Directory.Exists(commonAppDataFolder))
                SetAccessRightsDirectory(commonAppDataFolder);
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
