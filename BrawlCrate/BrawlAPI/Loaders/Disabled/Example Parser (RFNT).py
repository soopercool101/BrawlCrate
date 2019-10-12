from BrawlCrate.API import *
from BrawlLib.SSBB.ResourceNodes import *
from BrawlCrate.NodeWrappers import PluginWrapper
from System.Windows.Forms import ToolStripMenuItem, ToolStripSeparator
import struct

# Font node parser
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

    # Called for each instance of a new RFNT
    def OnInitialize(self):
        if self._name is None:
            self._name = "NW4R Font"
        return False

# Wrapper for font files
class RFNTWrapper(PluginWrapper):
    # This function returns a new instance of the class.
    # Necessary in order to properly call necessary functions
    def GetInstance(self):
        return RFNTWrapper()

# Basic test function that is called by the context menu
def doSomething_handler(sender, event_args):
    BrawlAPI.ShowMessage("This is a test function","Title")

# Create an instance of our node class and add it to the API loader cache
node = RFNTNode()
BrawlAPI.AddResourceParser(node)
# Create an instance of our wrapper class and add it to the API wrapper cache
wrapper = RFNTWrapper()
BrawlAPI.AddWrapper[RFNTNode](wrapper)
# Add a context menu item to our new wrapper
BrawlAPI.AddContextMenuItem(RFNTWrapper, ToolStripMenuItem("Do Something", None, doSomething_handler))