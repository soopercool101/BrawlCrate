using BrawlLib.SSBB.ResourceNodes;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.MDL0Color)]
    [NodeWrapper(ResourceType.MDL0UV)]
    [NodeWrapper(ResourceType.MDL0Normal)]
    [NodeWrapper(ResourceType.MDL0Vertex)]
    [NodeWrapper(ResourceType.MDL0Texture)]
    public class MDL0EntryWrapper : GenericWrapper
    {
        public override ResourceNode Duplicate(bool changeName)
        {
            if (_resource.Parent == null)
            {
                return null;
            }

            string tempPath = Path.GetTempFileName();
            _resource.Export(tempPath);
            // Initialize node in a way that will not cause crashes
            ResourceNode rNode2;
            switch (Resource.ResourceFileType)
            {
                case ResourceType.MDL0Color:
                    rNode2 = new MDL0ColorNode();
                    break;
                case ResourceType.MDL0UV:
                    rNode2 = new MDL0UVNode();
                    break;
                case ResourceType.MDL0Material:
                    rNode2 = new MDL0MaterialNode();
                    break;
                case ResourceType.MDL0Shader:
                    rNode2 = new MDL0ShaderNode();
                    break;
                case ResourceType.MDL0Texture:
                    rNode2 = new MDL0TextureNode();
                    break;
                case ResourceType.MDL0Normal:
                    rNode2 = new MDL0NormalNode();
                    break;
                case ResourceType.MDL0Bone:
                    rNode2 = new MDL0BoneNode();
                    break;
                case ResourceType.MDL0Vertex:
                    rNode2 = new MDL0VertexNode();
                    break;
                default:
                    throw new NotSupportedException("Unsupported type for MDL0 Duplication");
            }
            
            rNode2._name = _resource.Name;
            rNode2.Replace(tempPath);

            if (rNode2 == null)
            {
                MessageBox.Show("The node could not be duplicated correctly.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }

            // Remove the node from the parent temporarily
            rNode2.Remove();

            // Set the name programatically (based on Windows' implementation)
            int index = _resource.Index;
            int n = 0;
            if (changeName)
            {
                while (_resource.Parent.FindChildrenByName(rNode2.Name).Length >= 1)
                {
                    // Get the last index of the last duplicated node in order to place it after that one
                    index = Math.Max(index, _resource.Parent.FindChildrenByName(rNode2.Name).Last().Index);
                    // Set the name based on the number of duplicate nodes found
                    rNode2.Name = $"{_resource.Name} ({++n})";
                }
            }

            // Place the node in the same containing parent, after the last duplicated node.
            _resource.Parent.InsertChild(rNode2, true, index + 1);

            // Update name again in order to refresh things that need refreshing when name is updated
            rNode2.OnRenamed();

            return rNode2;
        }
    }
}
