using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Internal.Windows.Controls.Hex_Editor
{
    /// <summary>
    /// Defines a build-in ContextMenuStrip manager for HexBox control to show Copy, Cut, Paste menu in contextmenu of the control.
    /// </summary>
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public sealed class BuiltInContextMenu : Component
    {
        /// <summary>
        /// Contains the HexBox control.
        /// </summary>
        private readonly HexBox _hexBox;

        /// <summary>
        /// Contains the ContextMenuStrip control.
        /// </summary>
        private ContextMenuStrip _contextMenuStrip;

        /// <summary>
        /// Contains the "Cut"-ToolStripMenuItem object.
        /// </summary>
        //ToolStripMenuItem _cutToolStripMenuItem;
        /// <summary>
        /// Contains the "Copy"-ToolStripMenuItem object.
        /// </summary>
        private ToolStripMenuItem _copyToolStripMenuItem;

        /// <summary>
        /// Contains the "Paste"-ToolStripMenuItem object.
        /// </summary>
        private ToolStripMenuItem _pasteToolStripMenuItem;

        /// <summary>
        /// Contains the "Select All"-ToolStripMenuItem object.
        /// </summary>
        private ToolStripMenuItem _selectAllToolStripMenuItem;

        /// <summary>
        /// Initializes a new instance of BuildInContextMenu class.
        /// </summary>
        /// <param name="hexBox">the HexBox control</param>
        internal BuiltInContextMenu(HexBox hexBox)
        {
            _hexBox = hexBox;
            _hexBox.ByteProviderChanged += new EventHandler(HexBox_ByteProviderChanged);
        }

        /// <summary>
        /// If ByteProvider
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        private void HexBox_ByteProviderChanged(object sender, EventArgs e)
        {
            CheckBuiltInContextMenu();
        }

        /// <summary>
        /// Assigns the ContextMenuStrip control to the HexBox control.
        /// </summary>
        private void CheckBuiltInContextMenu()
        {
            if (Util.DesignMode)
            {
                return;
            }

            if (_contextMenuStrip == null)
            {
                ContextMenuStrip cms = new ContextMenuStrip();
                //_cutToolStripMenuItem = new ToolStripMenuItem(CutMenuItemTextInternal, CutMenuItemImage, new EventHandler(CutMenuItem_Click));
                //cms.Items.Add(_cutToolStripMenuItem);
                _copyToolStripMenuItem = new ToolStripMenuItem(CopyMenuItemTextInternal, CopyMenuItemImage,
                    new EventHandler(CopyMenuItem_Click));
                cms.Items.Add(_copyToolStripMenuItem);
                _pasteToolStripMenuItem = new ToolStripMenuItem(PasteMenuItemTextInternal, PasteMenuItemImage,
                    new EventHandler(PasteMenuItem_Click));
                cms.Items.Add(_pasteToolStripMenuItem);

                cms.Items.Add(new ToolStripSeparator());

                _selectAllToolStripMenuItem = new ToolStripMenuItem(SelectAllMenuItemTextInternal,
                    SelectAllMenuItemImage, new EventHandler(SelectAllMenuItem_Click));
                cms.Items.Add(_selectAllToolStripMenuItem);
                cms.Opening += new CancelEventHandler(BuildInContextMenuStrip_Opening);

                _contextMenuStrip = cms;
            }

            if (_hexBox.ByteProvider == null && _hexBox.ContextMenuStrip == _contextMenuStrip)
            {
                _hexBox.ContextMenuStrip = null;
            }
            else if (_hexBox.ByteProvider != null && _hexBox.ContextMenuStrip == null)
            {
                _hexBox.ContextMenuStrip = _contextMenuStrip;
            }
        }

        /// <summary>
        /// Before opening the ContextMenuStrip, we manage the availability of the items.
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        private void BuildInContextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            //_cutToolStripMenuItem.Enabled = this._hexBox.CanCut();
            _copyToolStripMenuItem.Enabled = _hexBox.CanCopy();
            _pasteToolStripMenuItem.Enabled = _hexBox.CanPaste();
            _selectAllToolStripMenuItem.Enabled = _hexBox.CanSelectAll();
        }

        /// <summary>
        /// The handler for the "Cut"-Click event
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        private void CutMenuItem_Click(object sender, EventArgs e)
        {
            _hexBox.Cut();
        }

        /// <summary>
        /// The handler for the "Copy"-Click event
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        private void CopyMenuItem_Click(object sender, EventArgs e)
        {
            _hexBox.Copy();
        }

        /// <summary>
        /// The handler for the "Paste"-Click event
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        private void PasteMenuItem_Click(object sender, EventArgs e)
        {
            _hexBox.Paste();
        }

        /// <summary>
        /// The handler for the "Select All"-Click event
        /// </summary>
        /// <param name="sender">the sender object</param>
        /// <param name="e">the event data</param>
        private void SelectAllMenuItem_Click(object sender, EventArgs e)
        {
            _hexBox.SelectAll();
        }

        /// <summary>
        /// Gets or sets the custom text of the "Copy" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu")]
        [DefaultValue(null)]
        [Localizable(true)]
        public string CopyMenuItemText
        {
            get => _copyMenuItemText;
            set => _copyMenuItemText = value;
        }

        private string _copyMenuItemText;

        /// <summary>
        /// Gets or sets the custom text of the "Cut" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu")]
        [DefaultValue(null)]
        [Localizable(true)]
        public string CutMenuItemText
        {
            get => _cutMenuItemText;
            set => _cutMenuItemText = value;
        }

        private string _cutMenuItemText;

        /// <summary>
        /// Gets or sets the custom text of the "Paste" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu")]
        [DefaultValue(null)]
        [Localizable(true)]
        public string PasteMenuItemText
        {
            get => _pasteMenuItemText;
            set => _pasteMenuItemText = value;
        }

        private string _pasteMenuItemText;

        /// <summary>
        /// Gets or sets the custom text of the "Select All" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu")]
        [DefaultValue(null)]
        [Localizable(true)]
        public string SelectAllMenuItemText
        {
            get => _selectAllMenuItemText;
            set => _selectAllMenuItemText = value;
        }

        private string _selectAllMenuItemText;

        /// <summary>
        /// Gets the text of the "Cut" ContextMenuStrip item.
        /// </summary>
        internal string CutMenuItemTextInternal => !string.IsNullOrEmpty(CutMenuItemText) ? CutMenuItemText : "Cut";

        /// <summary>
        /// Gets the text of the "Copy" ContextMenuStrip item.
        /// </summary>
        internal string CopyMenuItemTextInternal => !string.IsNullOrEmpty(CopyMenuItemText) ? CopyMenuItemText : "Copy";

        /// <summary>
        /// Gets the text of the "Paste" ContextMenuStrip item.
        /// </summary>
        internal string PasteMenuItemTextInternal =>
            !string.IsNullOrEmpty(PasteMenuItemText) ? PasteMenuItemText : "Paste";

        /// <summary>
        /// Gets the text of the "Select All" ContextMenuStrip item.
        /// </summary>
        internal string SelectAllMenuItemTextInternal =>
            !string.IsNullOrEmpty(SelectAllMenuItemText) ? SelectAllMenuItemText : "SelectAll";

        /// <summary>
        /// Gets or sets the image of the "Cut" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu")]
        [DefaultValue(null)]
        public Image CutMenuItemImage
        {
            get => _cutMenuItemImage;
            set => _cutMenuItemImage = value;
        }

        private Image _cutMenuItemImage;

        /// <summary>
        /// Gets or sets the image of the "Copy" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu")]
        [DefaultValue(null)]
        public Image CopyMenuItemImage
        {
            get => _copyMenuItemImage;
            set => _copyMenuItemImage = value;
        }

        private Image _copyMenuItemImage;

        /// <summary>
        /// Gets or sets the image of the "Paste" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu")]
        [DefaultValue(null)]
        public Image PasteMenuItemImage
        {
            get => _pasteMenuItemImage;
            set => _pasteMenuItemImage = value;
        }

        private Image _pasteMenuItemImage;

        /// <summary>
        /// Gets or sets the image of the "Select All" ContextMenuStrip item.
        /// </summary>
        [Category("BuiltIn-ContextMenu")]
        [DefaultValue(null)]
        public Image SelectAllMenuItemImage
        {
            get => _selectAllMenuItemImage;
            set => _selectAllMenuItemImage = value;
        }

        private Image _selectAllMenuItemImage;
    }
}