from BrawlCrate.API import BrawlAPI
from BrawlLib.SSBB.ResourceNodes import *

def tex_search(node): #Recursive function to scan for all TEX0/REFTEntry nodes in the file
    if isinstance(node, TEX0Node) or isinstance(node, REFTEntryNode):
        return [node] #If it's a TEX0/REFTEntry node, include it in the list
    list = []
    for child in node.Children:
        list += tex_search(child) #Otherwise, keep looking for children
    return list

if BrawlAPI.RootNode != None:
    root = BrawlAPI.RootNode
    folder = BrawlAPI.OpenFolderDialog()
    if folder:
        count = 0;
        for item in tex_search(root):
            item.Export(folder + "/" + item.Name + ".png") #We already know everything in this list is a TEX0 or REFTEntry, so we can export it to a .png
            count += 1
        if count:
            BrawlAPI.ShowMessage(str(count) + " textures were successfully exported to " + folder, "Success")
        else:
            BrawlAPI.ShowMessage('No textures were found in the open file','Error')
else:
    BrawlAPI.ShowMessage('Cannot find Root Node (is a file open?)','Error')