using SetAccessRights;
using System.ComponentModel;

namespace SetAccessRightsClassLib
{
    [RunInstaller(true)]
    public partial class Installer : System.Configuration.Install.Installer
    {
        public Installer()
        {
            InitializeComponent();
        }

        public override void Install(System.Collections.IDictionary stateSaver)
        {
            base.Install(stateSaver);
            AccessRightsProvider provider = new AccessRightsProvider();
            provider.SetRightsToAll();
        }


        public override void Commit(System.Collections.IDictionary savedState)
        {
            base.Commit(savedState);
        }
    }
}
