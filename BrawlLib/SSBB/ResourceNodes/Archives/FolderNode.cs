using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BrawlLib.SSBB.ResourceNodes.Archives
{
    public class FolderNode : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.Container;

        public List<ResourceNode> _list;

        public string Path;
        private string[] _directories;
        private string[] _files;

        public override void OnPopulate()
        {
            foreach (string s in _directories)
            {
                NodeFactory.FromFolder(this, s);
            }
            foreach (string s in _files)
            {
                NodeFactory.FromFile(this, s).Populate();
            }
            base.OnPopulate();
        }

        public override bool OnInitialize()
        {
            this._name = new DirectoryInfo(Path).Name;
            this._directories = Directory.GetDirectories(this.Path);
            this._files = Directory.GetFiles(this.Path);
            base.OnInitialize();

            return true;
        }
    }
}
