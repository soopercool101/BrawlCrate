using BrawlLib.Internal;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Subspace.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BrawlLib.SSBB.ResourceNodes.Subspace.Objects
{
    public class GSTGNode : BLOCEntryNode
    {
        public override Type SubEntryType => typeof(GSTGEntryNode);
        protected override string baseName => "Static Model Generators";

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return source.Tag == "GSTG" ? new GSTGNode() : null;
        }
    }

    public unsafe class GSTGEntryNode : ResourceNode, IRenderedLink
    {
        internal GSTGEntry Data;
        public List<ResourceNode> RenderTargets
        {
            get
            {
                List<ResourceNode> _targets = new List<ResourceNode>();
                if (Parent?.Parent?.Parent is ARCNode a)
                {
                    if (ModelDataIndex != byte.MaxValue)
                    {
                        ResourceNode model = a.Children.FirstOrDefault(c => c is ARCEntryNode ae && ae.FileType == ARCFileType.ModelData && ae.FileIndex == ModelDataIndex);
                        if (model != null)
                            _targets.Add(model);
                    }
                }
                return _targets;
            }
        }

        public override bool supportsCompression => false;

        [Category("GSTG")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Position
        {
            get => new Vector3(Data._posX, Data._posY, Data._posZ);
            set
            {
                Data._posX = value._x;
                Data._posY = value._y;
                Data._posZ = value._y;
                SignalPropertyChange();
            }
        }

        [Category("GSTG")]
        [TypeConverter(typeof(Vector3StringConverter))]
        public Vector3 Rotation
        {
            get => new Vector3(Data._rotX, Data._rotY, Data._rotZ);
            set
            {
                Data._rotX = value._x;
                Data._rotY = value._y;
                Data._rotZ = value._y;
                SignalPropertyChange();
            }
        }

        [Category("GSTG")]
        [TypeConverter(typeof(NullableByteConverter))]
        public byte ModelDataIndex
        {
            get => Data._modelDataIndex;
            set
            {
                Data._modelDataIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("GSTG")]
        [TypeConverter(typeof(NullableByteConverter))]
        public byte CollisionDataIndex
        {
            get => Data._collisionDataIndex;
            set
            {
                Data._collisionDataIndex = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1A
        {
            get => Data._unknown0x1A;
            set
            {
                Data._unknown0x1A = value;
                SignalPropertyChange();
            }
        }

        [Category("Unknown")]
        public byte Unknown0x1B
        {
            get => Data._unknown0x1B;
            set
            {
                Data._unknown0x1B = value;
                SignalPropertyChange();
            }
        }

        public override int OnCalculateSize(bool force)
        {
            return GSTGEntry.Size;
        }

        public override bool OnInitialize()
        {
            Data = *(GSTGEntry*)WorkingUncompressed.Address;
            return false;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            GSTGEntry* hdr = (GSTGEntry*) address;
            *hdr = Data;
        }
    }
}
