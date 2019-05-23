from BrawlBox.API import bboxapi
from BrawlLib.SSBB.ResourceNodes import *

def tex_search(node): #Recursive function to scan for all TEX0 nodes in the file
    if isinstance(node, TEX0Node):
        return [node] #If it's a TEX0 node, include it in the list
    list = []
    for child in node.Children:
        list += tex_search(child) #Otherwise, keep looking for children
    return list

if bboxapi.RootNode != None:
    root = bboxapi.RootNode
    for item in tex_search(root):
        print item.Name
        item.Export(item.Name + ".png") #We already know everything in this list is a TEX0, so we can export it to a #PNG
    print("    Done!")
else:
    bboxapi.ShowMessage('Cannot find Root Node (is a file open?)','Error')