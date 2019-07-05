__author__ = "soopercool101"
__version__ = "1.0.0"

from BrawlCrate.API import *
from BrawlCrate.NodeWrappers import MDL0Wrapper
from BrawlLib.SSBB.ResourceNodes import *
from System.Windows.Forms import ToolStripMenuItem
from System import String

# Function to ensure the context menu item is only active if it's the CSP ARC
def enable_check(sender, event_args):
    sender.Enabled = (BrawlAPI.SelectedNode.IsStagePosition)

def stgpos_search(node): # Recursive function to scan for all image nodes in the file
    if isinstance(node, MDL0Node) and node.IsStagePosition and BrawlAPI.SelectedNode is not node:
        return [node] # If it's a stage position node, include it in the list
    stgpos_list = []
    for child in node.Children:
        stgpos_list += stgpos_search(child) # Otherwise, keep looking for children
    return stgpos_list

# Function to export CSP BRRESs in the selected ARC
def sync_stg_pos(sender, event_args):
    root = BrawlAPI.RootNode
    sel = BrawlAPI.SelectedNode
    count = 0;
    final_string = "The following node(s) have been synced with " + sel.Name + ":"
    for item in stgpos_search(root):
        name = item.Name
        item.Replace(sel) # We already know everything in this list should be exportable to a .png
        item.Name = name
        count += 1
        final_string += "\n\t" + item.Name
    if count:
        BrawlAPI.ShowMessage(final_string, "Success")
    else:
        BrawlAPI.ShowMessage('No textures were found in the open file','Error')

# Add a button to our right click menu.
#
# Arguments are (in order) as follows:
# Wrapper: Denotes which wrapper the context menu items will be added to
# Submenu: If not blank, adds to a submenu with this name
# Description: Creates a mouseover description for the item
# Conditional: When the wrapper's context menu is opened, this function is called. Allows enabling/disabling of plugin members based on specific conditions
# Items: One or more toolstripmenuitems that will be added
BrawlAPI.AddContextMenuItem(MDL0Wrapper, "", "Syncs all Stage Positions in this file to match this node", enable_check, ToolStripMenuItem("Sync Stage Positions", None, sync_stg_pos))
