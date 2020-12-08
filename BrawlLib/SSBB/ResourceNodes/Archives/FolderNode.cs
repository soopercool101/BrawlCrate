using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace BrawlLib.SSBB.ResourceNodes
{
    public class FolderNode : ResourceNode
    {
        public override ResourceType ResourceFileType => ResourceType.Folder;

        public List<ResourceNode> _list;

        [Browsable(false)]
        public string FolderPath => Parent != null && Parent is FolderNode f ? $"{f.FolderPath}\\{Name}" : _origPath;

        private string[] _directories;
        private string[] _files;


        [DisplayName("Uncompressed Size (Bytes)")]
        [Description("For stability, this value is only updated on save.")]
        public override uint UncompressedSize
        {
            get
            {
                uint size = 0;

                if (Children == null)
                {
                    return size;
                }

                foreach (ResourceNode r in Children)
                {
                    size += r.UncompressedSize;
                }

                return size;
            }
        }

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

        public override void Export(string outPath)
        {
            Directory.CreateDirectory(outPath);
            foreach (ResourceNode c in Children)
            {
                if (c.IsDirty || !outPath.Equals(FolderPath))
                {
                    c.Export($"{outPath}\\{c.FileName}");
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
                if (node != null)
                {
                    node._origPath = s;
                }
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