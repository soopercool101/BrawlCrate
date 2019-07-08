from BrawlCrate.API import BrawlAPI
from BrawlLib.SSBB.ResourceNodes import *
from BrawlLib.Imaging import IImageSource

# DEPRECIATED due to the new preview system. Working on a rewrite.

def tex_search(node): # Recursive function to scan for all image nodes in the file
    if isinstance(node, IImageSource):
        return [node] # If it's an image node, include it in the list
    tex_list = []
    for child in node.Children:
        tex_list += tex_search(child) # Otherwise, keep looking for children
    return tex_list

if BrawlAPI.RootNode != None:
    root = BrawlAPI.RootNode
    folder = BrawlAPI.OpenFolderDialog()
    if folder:
        count = 0;
        for item in tex_search(root):
            item.Export(folder + "/" + item.Name + ".png") # We already know everything in this list should be exportable to a .png
            count += 1
        if count:
            BrawlAPI.ShowMessage(str(count) + " textures were successfully exported to " + folder, "Success")
        else:
            BrawlAPI.ShowMessage('No textures were found in the open file','Error')
else:
    BrawlAPI.ShowMessage('Cannot find Root Node (is a file open?)','Error')