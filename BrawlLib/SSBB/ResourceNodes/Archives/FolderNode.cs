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
        public override ResourceType ResourceFileType => ResourceType.Folder;

        public List<ResourceNode> _list;

        public string FolderPath => Parent != null && Parent is FolderNode f ? $"{f.FolderPath}\\{Name}" : _origPath;

        private string[] _directories;
        private string[] _files;

        public override bool IsDirty
        {
            get
            {
                if (!AllowSaving)
                {
                    return false;
                }

                if (HasChanged)
                {
                    return true;
                }

                if (_children != null)
                {
                    foreach (ResourceNode n in _children)
                    {
                        if (n.HasChanged || n.IsDirty)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
            set
            {
                _changed = value;
                if (_children != null)
                {
                    foreach (ResourceNode r in Children)
                    {
                        if (r._children != null)
                        {
                            r.IsDirty = value;
                        }
                        else
                        {
                            r._changed = value;
                        }
                    }
                }
            }
        }

        public override unsafe void Export(string outPath)
        {
            Directory.CreateDirectory(outPath);
            foreach (ResourceNode c in Children)
            {
                if (c.IsDirty || !outPath.Equals(FolderPath))
                {
                    c.Export($"{outPath}\\{c.OrigFileName}");
                }
            }

            IsDirty = false;
        }

        public override void OnPopulate()
        {
            foreach (string s in _directories)
            {
                NodeFactory.FromFolder(this, s);
            }

            foreach (string s in _files)
            {
                ResourceNode node = NodeFactory.FromFile(this, s);
                node._origPath = s;
            }

            base.OnPopulate();
        }

        public override bool OnInitialize()
        {
            _name = new DirectoryInfo(_origPath).Name;
            _directories = Directory.GetDirectories(_origPath);
            _files = Directory.GetFiles(_origPath);
            IsDirty = false;
            base.OnInitialize();

            return true;
        }
    }
}