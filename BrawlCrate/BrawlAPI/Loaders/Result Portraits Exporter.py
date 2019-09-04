__author__ = "soopercool101"
__version__ = "1.0.0"

from BrawlCrate.API import *
from BrawlCrate.NodeWrappers import ARCWrapper
from BrawlLib.SSBB.ResourceNodes import *
from System.Windows.Forms import ToolStripMenuItem

# Function to ensure the context menu item is only active if it's the CSP ARC
def EnableCheck(sender, event_args):
    sender.Enabled = (BrawlAPI.SelectedNode is not None and BrawlAPI.SelectedNode.Name == "char_bust_tex_lz77")

# Function to export CSP BRRESs in the selected ARC
def export_to_results(sender, event_args):
    # Allow user to choose the output folder
    folder = BrawlAPI.OpenFolderDialog()
    if folder:
        # Initialize a "count" variable (used solely for messaging purposes, not really necessary)
        count = 0
        # For every child node in the ARC
        for child in BrawlAPI.SelectedNode.Children:
            # If the child is a BRRES and the BRRES isn't empty
            if isinstance(child, BRRESNode) and child.HasChildren:
                # Export the BRRES without compression to a filename that matches the RSP implementation
                child.ExportUncompressed(folder + "/MenSelchrFaceB" + ("%02d" % (child.FileIndex,)) + "0.brres")
                # Increment the count for every successful export
                count += 1
        if count:
            # If successful, print success message
            BrawlAPI.ShowMessage(str(count) + " BRRESs were successfully exported to " + folder, "Success")
        else:
            # Otherwise, print error message
            BrawlAPI.ShowError('No BRRESs were found in the open file','Error')

# Add a button to our right click menu. In this case, adds a button to the right click menu for any ARC named "char_bust_tex_lz77"
#
# Arguments are (in order) as follows:
# Wrapper: Denotes which wrapper the context menu items will be added to
# Submenu: If not blank, adds to a submenu with this name
# Description: Creates a mouseover description for the item
# Conditional: When the wrapper's context menu is opened, this function is called. Allows enabling/disabling of plugin members based on specific conditions
# Items: One or more toolstripmenuitems that will be added
BrawlAPI.AddContextMenuItem(ARCWrapper, "", "Exports the CSPs in this ARC to Results Screen formatted BRRESs", EnableCheck, ToolStripMenuItem("Export as RSPs", None, export_to_results))
