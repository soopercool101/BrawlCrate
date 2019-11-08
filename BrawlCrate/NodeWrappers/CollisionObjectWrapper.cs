using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BrawlLib.SSBB;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCrate.NodeWrappers
{
    [NodeWrapper(ResourceType.CollisionObj)]
    public class CollisionObjectWrapper : GenericWrapper
    {
        public override string ExportFilter => FileFilters.CollisionDef;

        public override string ImportFilter => FileFilters.Raw;

        // Override Duplicate since by default Collision Objects don't contain their own data
        public override void Duplicate()
        {
            if (_resource.Parent == null)
            {
                return;
            }

            string tempPath = Path.GetTempFileName();
            // Explicitly set the ".coll" extension to ensure that the node exports all necessary data
            _resource.Export($"{tempPath}.coll");
            if (!(NodeFactory.FromFile(null, $"{tempPath}.coll", typeof(CollisionNode)) is CollisionNode cNode))
            {
                MessageBox.Show("The node could not be duplicated correctly.");
                return;
            }

            int n = 0;
            int index = _resource.Index;
            // Copy the name directly since Collision Objects don't store their own name
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