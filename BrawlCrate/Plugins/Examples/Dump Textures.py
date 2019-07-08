from BrawlCrate.API import BrawlAPI
from BrawlLib.SSBB.ResourceNodes import *
from BrawlLib.Imaging import IImageSource
from System.Collections.Generic import List
from BrawlCrate.NodeWrappers import *

def tex_search(): # Recursive function to scan for all image nodes in the file
    wrappers = BrawlAPI.NodeWrapperList
    tex_list = []
    for wrapper in wrappers:
        if ".png" in wrapper.ExportFilter: # if the node is exportable to png, add it
            tex_list += [wrapper.Resource]
    return tex_list

if BrawlAPI.RootNode != None:
    root = BrawlAPI.RootNode
    folder = BrawlAPI.OpenFolderDialog()
    if folder:
        count = 0;
        for item in tex_search():
            item.Export(folder + "/" + item.Name + ".png") # We already know everything in this list is exportable to a .png
            count += 1
        if count:
            BrawlAPI.ShowMessage(str(count) + " textures were successfully exported to " + folder, "Success")
        else:
            BrawlAPI.ShowMessage('No textures were found in the open file','Error')
else:
    BrawlAPI.ShowMessage('Cannot find Root Node (is a file open?)','Error')