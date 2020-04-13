using BrawlLib.SSBB.ResourceNodes;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace BrawlCrate.NodeWrappers.MDL0
{
    [NodeWrapper(ResourceType.MDL0Color)]
    public class MDL0ColorWrapper : GenericWrapper
    {
        public override ResourceNode Duplicate(bool changeName)
        {
            if (_resource.Parent == null)
            {
                return null;
            }

            string tempPath = Path.GetTempFileName();
            _resource.Export(tempPath);
            // Initialize node as a child of the parent
            ResourceNode rNode2 = new MDL0ColorNode { _name = _resource.Name };
            rNode2.Replace(tempPath);

            if (rNode2 == null)
            {
                MessageBox.Show("The node could not be duplicated correctly.", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return null;
            }

            // Remove the node from the parent temporarily
            rNode2.Remove();

            // Copy ARCEntryNode data, which is contained in the containing ARC, not the node itself
            if (rNode2 is ARCEntryNode node)
            {
                node.FileIndex = ((ARCEntryNode)_resource).FileIndex;
                node.FileType = ((ARCEntryNode)_resource).FileType;
                node.GroupID = ((ARCEntryNode)_resource).GroupID;
            }

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

            // Copy redirect info as necessary
            if (rNode2 is ARCEntryNode entryNode)
            {
                entryNode.RedirectIndex = ((ARCEntryNode)_resource).RedirectIndex;
            }

            // Update name again in order to refresh things that need refreshing when name is updated
            rNode2.OnRenamed();

            return rNode2;
        }
    }
}
