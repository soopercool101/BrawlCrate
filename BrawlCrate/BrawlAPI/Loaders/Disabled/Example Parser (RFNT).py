from BrawlCrate.API import *
from BrawlLib.SSBB.ResourceNodes import *
import struct

class RFNTNode(PluginResourceParser):
    # Set our resource type (Dictates nodewrapper and icon)
    def get_ResourceType(self):
        return ResourceType.NoEditFolder

    # Called by super class to check if this loader matches the data
    def TryParse(self, stream):
        src = file(stream)
        src.seek(0,0)
        tag = ""
        tag = tag.join(struct.unpack('>4s', src.read(4)))

        #if yes, return an instance of our class.
        if tag == "RFNT": #RFNT
            return RFNTNode()
        return None

    def OnInitialize(self):
        if self._name is None:
            self._name = "NW4R Font"
        return False

def doSomething_handler(sender, event_args):
    BrawlAPI.ShowMessage("doing something","title")

# Create an instance of our node class and add it to the API loader cache
node = RFNTNode()
BrawlAPI.AddResourceParser(node)