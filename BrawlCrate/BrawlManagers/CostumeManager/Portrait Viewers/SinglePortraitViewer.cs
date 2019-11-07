using BrawlLib.SSBB.ResourceNodes;
using System;

namespace BrawlCrate.BrawlManagers.CostumeManager.Portrait_Viewers
{
    public abstract partial class SinglePortraitViewer : PortraitViewer
    {
        protected ControlCollection AdditionalControls => additionalTexturesPanel.Controls;

        public abstract int PortraitWidth { get; }
        public abstract int PortraitHeight { get; }
        public abstract ResourceNode PortraitRootFor(int charNum, int costumeNum);

        /// <summary>
        /// This method, when overriden in a subclass, returns the TEX0 within a given
        /// ResourceNode for the given character and costume index.
        /// </summary>
        public abstract ResourceNode MainTEX0For(ResourceNode node, int charNum, int costumeNum);

        protected PortraitViewerTextureData mainTexture;

        // In case the image needs to be reloaded after replacing the texture
        protected int _charNum, _costumeNum;

        public SinglePortraitViewer()
        {
            InitializeComponent();
            mainTexture = new PortraitViewerTextureData(PortraitWidth, PortraitHeight, this);
            additionalTexturesPanel.Controls.Add(mainTexture.TexturePanel);
            mainTexture.OnUpdate = delegate(PortraitViewerTextureData sender) { UpdateImage(_charNum, _costumeNum); };

            _charNum = -1;
            _costumeNum = -1;
        }

        public override bool UpdateImage(int charNum, int costumeNum)
        {
            _charNum = charNum;
            _costumeNum = costumeNum;
            ResourceNode bres = PortraitRootFor(charNum, costumeNum);
            if (bres != null)
            {
                label1.Text = bres.RootNode.Name;
            }

            mainTexture.TextureFrom(bres, charNum, costumeNum);
            return mainTexture.Texture != null;
        }

        public void Replace(string filename, bool useTextureConverter)
        {
            mainTexture.Replace(filename, useTextureConverter);
        }

        protected abstract void saveButton_Click(object sender, EventArgs e);
    }
}