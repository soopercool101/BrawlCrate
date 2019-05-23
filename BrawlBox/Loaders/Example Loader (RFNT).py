from BrawlBox.NodeWrappers import ARCWrapper
from BrawlBox.API import *
from BrawlLib.SSBB.ResourceNodes import *
import struct
from System.Windows.Forms import ToolStripMenuItem

class RFNTNode(PluginLoader):
    # Set our resource type (Dictates nodewrapper and icon)
    def get_ResourceType(self):
        return ResourceType.NoEditFolder

    # Called by super class to check if this loader matches the data
    def TryParse(self, stream):
        src = file(stream)
        src.seek(5,0)
        i = struct.unpack('>I', src.read(4))[0]

        #if yes, return an instance of our class.
        #if i == 0x52464E54: #RFNT
        #    return RFNTNode()
        #else:
        #    return None
        return None

    def OnInitialize(self):
        if self._name is None:
            self._name = "NW4R Font"
        return False

def doSomething_handler(sender, event_args):
    bboxapi.ShowMessage("doing something","title")

# Create an instance of our node class and add it to the API loader cache
node = RFNTNode()
bboxapi.AddLoader(node)

# Add a button to our right click menu
bboxapi.AddContextMenuItem(ARCWrapper, ToolStripMenuItem("Do Something", None, doSomething_handler))
