from BrawlCrate.NodeWrappers import ARCWrapper
from BrawlCrate.API import *
from BrawlLib.SSBB.ResourceNodes import *
from System.Windows.Forms import ToolStripMenuItem

def export_to_results(sender, event_args):
    folder = BrawlAPI.OpenFolderDialog()
    count = 0;
    if folder:
        for child in BrawlAPI.SelectedNode.Children:
            if isinstance(child, BRRESNode):
                child.ExportUncompressed(folder + "/MenSelchrFaceB" + ("%02d" % (child.FileIndex,)) + "0.brres");
                count += 1;
        if count:
            BrawlAPI.ShowMessage(str(count) + " BRRESs were successfully exported to " + folder, "Success")
        else:
            BrawlAPI.ShowMessage('No BRRESs were found in the open file','Error')

def EnableCheck(sender, event_args):
    sender.Enabled = (BrawlAPI.SelectedNode.Name == "char_bust_tex_lz77")

# Add a button to our right click menu
BrawlAPI.AddContextMenuItem(ARCWrapper, "", "Exports the CSPs in this ARC to Results Screen formatted BRRESs", EnableCheck, ToolStripMenuItem("Export as RSPs", None, export_to_results))
