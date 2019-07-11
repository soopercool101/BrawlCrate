from BrawlCrate.API import BrawlAPI
from BrawlLib.SSBB.ResourceNodes import *
from BrawlCrate.NodeWrappers import *

def tex_search(): # Function to scan for all image nodes in the file
    wrappers = BrawlAPI.NodeWrapperList # Get the wrapper of every node in the file (export filters are contained in wrappers)
    tex_list = []
    for wrapper in wrappers:
        if ".png" in wrapper.ExportFilter: # If the node is exportable to png, add it to the list
            tex_list += [wrapper.Resource]
    return tex_list

# Main function
if BrawlAPI.RootNode != None: # If there is a valid open file
    root = BrawlAPI.RootNode
    folder = BrawlAPI.OpenFolderDialog()
    if folder:
        count = 0 # Set the count
        for item in tex_search(): # Gather the texture list to export
            item.Export(folder + "/" + item.Name + ".png") # We already know everything in this list is exportable to a .png
            count += 1 # Increment the count
        if count: # If any textures are found, show the user a success message
            BrawlAPI.ShowMessage(str(count) + " textures were successfully exported to " + folder, "Success")
        else: # If no textures are found, show an error message
            BrawlAPI.ShowError('No textures were found in the open file','Error')
else: # Show an error message if there is no valid file open
    BrawlAPI.ShowError('Cannot find Root Node (is a file open?)','Error')