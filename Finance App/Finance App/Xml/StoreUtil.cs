using System.Linq;
using System.Xml.Linq;

namespace Finance_App.Xml
{
    internal class StoreUtil
    {
        public void CheckPendingSyncFlagEnabled()
        {
            var xmlDoc = XElement.Load("Store.xml");
            if (xmlDoc.Descendants("Flags").Count() > 0)
            {
                Variables.SetPendingSync(true);
            } else
            {
                Variables.SetPendingSync(false);
            }
        }

        public void StorePendingSyncFlag()
        {
            var xmlDoc = XElement.Load("Store.xml");
            xmlDoc.Add(new XElement("Flags", new XAttribute("SyncPending", true)));
            xmlDoc.Save("Store.xml");
            Variables.SetPendingSync(true);
        }

        public void RemovePendingSyncFlag()
        {
            var xmlDoc = XElement.Load("Store.xml");
            xmlDoc.Descendants("Flags").Remove();
            xmlDoc.Save("Store.xml");
            Variables.SetPendingSync(false);
        }
    }
}
