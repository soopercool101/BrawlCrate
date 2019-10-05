using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrawlLib;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.CollisionObj)]
    public class CollisionObjectWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.CollisionDef;

        // Override Duplicate since by default Collision Objects don't contain their own data
        public override void Duplicate()
        {
            if (_resource.Parent == null)
            {
                return;
            }

            string tempPath = Path.GetTempFileName();
            _resource.Export(tempPath + ".coll");
            CollisionNode cNode =
                NodeFactory.FromFile(null, tempPath + ".coll", typeof(CollisionNode)) as CollisionNode;
            if (cNode == null)
            {
                MessageBox.Show("The node could not be duplicated correctly.");
                return;
            }

            int n = 0;
            int index = _resource.Index;
            // Copy the name directly in cases where name isn't saved
            cNode.Children[0].Name = _resource.Name;
            // Set the name programatically (based on Windows' implementation)
            while (_resource.Parent.FindChildrenByName(cNode.Children[0].Name).Length >= 1)
            {
                // Get the last index of the last duplicated node in order to place it after that one
                index = Math.Max(index, _resource.Parent.FindChildrenByName(cNode.Children[0].Name).Last().Index);
                // Set the name based on the number of duplicate nodes found
                cNode.Children[0].Name = $"{_resource.Name} ({++n})";
            }

            // Place the node in the same containing parent, after the last duplicated node.
            _resource.Parent.InsertChild(cNode.Children[0], true, index + 1);
        }
    }
}