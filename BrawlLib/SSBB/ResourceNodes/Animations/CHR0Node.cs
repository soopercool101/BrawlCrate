using BrawlLib.Internal;
using BrawlLib.Internal.IO;
using BrawlLib.Internal.Windows.Forms;
using BrawlLib.Modeling.Collada;
using BrawlLib.SSBB.Types;
using BrawlLib.SSBB.Types.Animations;
using BrawlLib.Wii.Animations;
using BrawlLib.Wii.Models;
using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class CHR0Node : NW4RAnimationNode
    {
        internal CHR0v4_3* Header4_3 => (CHR0v4_3*) WorkingUncompressed.Address;
        internal CHR0v5* Header5 => (CHR0v5*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.CHR0;
        public override Type[] AllowedChildTypes => new Type[] {typeof(CHR0EntryNode)};
        public override int[] SupportedVersions => new int[] {4, 5};
        public override string Tag => "CHR0";

        public CHR0Node()
        {
            _version = 4;
        }

        private const string _category = "Bone Animation";

        [Category(_category)]
        public override int FrameCount
        {
            get => base.FrameCount;
            set => base.FrameCount = value;
        }

        [Category(_category)]
        public override bool Loop
        {
            get => base.Loop;
            set
            {
                base.Loop = value;
                UpdateChildFrameLimits();
            }
        }

        protected override void UpdateChildFrameLimits()
        {
            foreach (CHR0EntryNode n in Children)
            {
                n.SetSize(_numFrames, Loop);
            }
        }

        public CHR0EntryNode CreateEntry()
        {
            return CreateEntry(null);
        }

        public CHR0EntryNode CreateEntry(string name)
        {
            CHR0EntryNode n = new CHR0EntryNode
            {
                _name = FindName(name)
            };
            AddChild(n);
            n.SetSize(_numFrames, Loop);
            return n;
        }

        public void InsertKeyframe(int index)
        {
            FrameCount++;
            foreach (CHR0EntryNode c in Children)
            {
                c.Keyframes.Insert(index, 0, 1, 2, 3, 4, 5, 6, 7, 8);
            }
        }

        public void DeleteKeyframe(int index)
        {
            foreach (CHR0EntryNode c in Children)
            {
                c.Keyframes.Delete(index, 0, 1, 2, 3, 4, 5, 6, 7, 8);
            }

            FrameCount--;
        }

        public int num;

        public override bool OnInitialize()
        {
            base.OnInitialize();
            if (_version == 5)
            {
                CHR0v5* header = Header5;
                _numFrames = header->_numFrames;
                _loop = header->_loop != 0;

                if (_name == null)
                {
                    if (header->ResourceString != null)
                    {
                        _name = header->ResourceString;
                    }
                    else
                    {
                        _name = "anim" + Index;
                    }
                }

                if (header->_origPathOffset > 0)
                {
                    _originalPath = header->OrigPath;
                }

                (_userEntries = new UserDataCollection()).Read(header->UserData, WorkingUncompressed);

                return header->Group->_numEntries > 0;
            }
            else
            {
                CHR0v4_3* header = Header4_3;
                _numFrames = header->_numFrames;
                _loop = header->_loop != 0;

                if (_name == null)
                {
                    if (header->ResourceString != null)
                    {
                        _name = header->ResourceString;
                    }
                    else
                    {
                        _name = "anim" + Index;
                    }
                }

                if (header->_origPathOffset > 0)
                {
                    _originalPath = header->OrigPath;
                }

                return header->Group->_numEntries > 0;
            }
        }

        public override void OnPopulate()
        {
            ResourceGroup* group = Header4_3->Group;
            for (int i = 0; i < group->_numEntries; i++)
            {
                new CHR0EntryNode().Initialize(this, new DataSource(group->First[i].DataAddress, 0));
            }
        }

        internal override void GetStrings(StringTable table)
        {
            table.Add(Name);
            foreach (CHR0EntryNode n in Children)
            {
                table.Add(n.Name);
            }

            if (_version == 5)
            {
                _userEntries.GetStrings(table);
            }

            if (!string.IsNullOrEmpty(_originalPath))
            {
                table.Add(_originalPath);
            }
        }

        public override int OnCalculateSize(bool force)
        {
            int size = (_version == 5 ? CHR0v5.Size : CHR0v4_3.Size) + 0x18 + Children.Count * 0x10;
            foreach (CHR0EntryNode n in Children)
            {
                size += n.CalculateSize(true);
            }

            if (_version == 5)
            {
                size += _userEntries.GetSize();
            }

            return size;
        }

        public override void Export(string outPath)
        {
            if (outPath.EndsWith(".dae", StringComparison.OrdinalIgnoreCase))
            {
                Collada.Serialize(new CHR0Node[] {this}, 60.0f, false, outPath);
            }
            else if (outPath.EndsWith(".anim", StringComparison.OrdinalIgnoreCase))
            {
                AnimFormat.Serialize(this, outPath);
            }
            else
            {
                base.Export(outPath);
            }
        }

        public static CHR0Node FromFile(string path)
        {
            try
            {
                if (path.EndsWith(".anim", StringComparison.OrdinalIgnoreCase))
                {
                    return AnimFormat.Read(path);
                }

                if (path.EndsWith(".txt", StringComparison.OrdinalIgnoreCase))
                {
                    return CHR0TextImporter.Convert(path);
                }

                if (path.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
                {
                    return CHR0JsonImporter.Convert(path);
                }

                //if (path.EndsWith(".bvh", StringComparison.OrdinalIgnoreCase))
                //    return BVH.Import(path);
                //if (path.EndsWith(".vmd", StringComparison.OrdinalIgnoreCase))
                //    return PMDModel.ImportVMD(path);

                return NodeFactory.FromFile(null, path, typeof(CHR0Node)) as CHR0Node;
            }
            catch (System.Runtime.Serialization.SerializationException e)
            {
                MessageBox.Show("There was a problem importing the model animation.\n\nError:\n" + e.Message);
            }


            throw new NotSupportedException("The file extension specified is not of a supported animation type.");
        }

        public override void Replace(string fileName)
        {
            if (fileName.EndsWith(".chr0", StringComparison.OrdinalIgnoreCase))
            {
                base.Replace(fileName);
                return;
            }

            CHR0Node n = FromFile(fileName);
            n.Name = Name;
            Parent?.InsertChild(n, true, Index + 1);
            Remove();
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            ResourceGroup* group;
            if (_version == 5)
            {
                CHR0v5* header = (CHR0v5*) address;
                *header = new CHR0v5(_version, length, _numFrames, Children.Count, _loop);
                group = header->Group;
            }
            else
            {
                CHR0v4_3* header = (CHR0v4_3*) address;
                *header = new CHR0v4_3(_version, length, _numFrames, Children.Count, _loop);
                group = header->Group;
            }

            *group = new ResourceGroup(Children.Count);

            VoidPtr entryAddress = group->EndAddress;
            VoidPtr dataAddress = entryAddress;

            foreach (CHR0EntryNode n in Children)
            {
                dataAddress += n._entryLen;
            }

            ResourceEntry* rEntry = group->First;
            foreach (CHR0EntryNode n in Children)
            {
                (rEntry++)->_dataOffset = (int) entryAddress - (int) group;

                n._dataAddr = dataAddress;
                n.Rebuild(entryAddress, n._entryLen, true);
                entryAddress += n._entryLen;
                dataAddress += n._dataLen;
            }

            if (_userEntries.Count > 0 && _version == 5)
            {
                CHR0v5* header = (CHR0v5*) address;
                header->UserData = dataAddress;
                _userEntries.Write(dataAddress);
            }
        }

        protected internal override void PostProcess(VoidPtr bresAddress, VoidPtr dataAddress, int dataLength,
                                                     StringTable stringTable)
        {
            base.PostProcess(bresAddress, dataAddress, dataLength, stringTable);

            ResourceGroup* group;
            if (_version == 5)
            {
                CHR0v5* header = (CHR0v5*) dataAddress;
                header->ResourceStringAddress = (int) stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
                }

                group = header->Group;
            }
            else
            {
                CHR0v4_3* header = (CHR0v4_3*) dataAddress;
                header->ResourceStringAddress = (int) stringTable[Name] + 4;
                if (!string.IsNullOrEmpty(_originalPath))
                {
                    header->OrigPathAddress = stringTable[_originalPath] + 4;
                }

                group = header->Group;
            }

            group->_first = new ResourceEntry(0xFFFF, 0, 0, 0, 0);
            ResourceEntry* rEntry = group->First;

            int index = 1;
            foreach (CHR0EntryNode n in Children)
            {
                dataAddress = (VoidPtr) group + (rEntry++)->_dataOffset;
                ResourceEntry.Build(group, index++, dataAddress, (BRESString*) stringTable[n.Name]);
                n.PostProcess(dataAddress, stringTable);
            }

            if (_version == 5)
            {
                _userEntries.PostProcess(((CHR0v5*) dataAddress)->UserData, stringTable);
            }
        }

        internal static ResourceNode TryParse(DataSource source, ResourceNode parent)
        {
            return ((BRESCommonHeader*) source.Address)->_tag == CHR0v4_3.Tag ? new CHR0Node() : null;
        }

        #region Extra Functions

        /// <summary>
        /// Stretches or compresses all frames of the animation to fit a new frame count specified by the user.
        /// </summary>
        public void Resize()
        {
            FrameCountChanger f = new FrameCountChanger();
            if (f.ShowDialog(FrameCount) == DialogResult.OK)
            {
                Resize(f.NewValue);
            }
        }

        /// <summary>
        /// Stretches or compresses all frames of the animation to fit a new frame count.
        /// </summary>
        public void Resize(int newFrameCount)
        {
            KeyframeEntry kfe = null;
            float ratio = newFrameCount / (float) FrameCount;
            foreach (CHR0EntryNode e in Children)
            {
                KeyframeCollection newCollection = new KeyframeCollection(9, newFrameCount + (Loop ? 1 : 0), 1, 1, 1);
                for (int x = 0; x < FrameCount; x++)
                {
                    int newFrame = (int) (x * ratio + 0.5f);
                    float frameRatio = newFrame == 0 ? 0 : x / (float) newFrame;
                    for (int i = 0; i < 9; i++)
                    {
                        if ((kfe = e.GetKeyframe(i, x)) != null)
                        {
                            newCollection.SetFrameValue(i, newFrame, kfe._value)._tangent =
                                kfe._tangent * (float.IsNaN(frameRatio) ? 1 : frameRatio);
                        }
                    }
                }

                e._keyframes = newCollection;
            }

            FrameCount = newFrameCount;
        }

        public void MergeWith()
        {
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = "CHR0 Animation (*.chr0)|*.chr0",
                Title = "Please select an animation to merge with."
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                CHR0Node external;
                if ((external = FromFile(o.FileName)) != null)
                {
                    MergeWith(external);
                }
            }
        }

        public void MergeWith(CHR0Node external)
        {
            if (external.FrameCount != FrameCount && MessageBox.Show(null,
                "Frame counts are not equal; the shorter animation will end early. Do you still wish to continue?",
                "Merge Animations", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                return;
            }

            if (external.FrameCount > FrameCount)
            {
                FrameCount = external.FrameCount;
            }

            foreach (CHR0EntryNode _extTarget in external.Children)
            {
                CHR0EntryNode node = null;
                KeyframeEntry kfe = null;

                CHR0EntryNode entry = new CHR0EntryNode {Name = _extTarget.Name};
                entry.SetSize(_extTarget.FrameCount, Loop);

                //Apply all external keyframes to current entry.
                for (int x = 0; x < _extTarget.FrameCount; x++)
                {
                    for (int i = 0; i < 9; i++)
                    {
                        if ((kfe = _extTarget.GetKeyframe(i, x)) != null)
                        {
                            entry.Keyframes.SetFrameValue(i, x, kfe._value)._tangent = kfe._tangent;
                        }
                    }
                }

                if ((node = FindChild(_extTarget.Name, false) as CHR0EntryNode) == null)
                {
                    AddChild(entry, true);
                }
                else
                {
                    DialogResult result = MessageBox.Show(null,
                        "A bone entry with the name " + _extTarget.Name +
                        " already exists.\nDo you want to rename this entry?\nOtherwise, you will have the option to merge the keyframes.",
                        "Rename Entry?", MessageBoxButtons.YesNoCancel);
                    if (result == DialogResult.Yes)
                    {
                        Top:
                        RenameDialog d = new RenameDialog();
                        if (d.ShowDialog(null, entry) == DialogResult.OK)
                        {
                            if (entry.Name != _extTarget.Name)
                            {
                                AddChild(entry, true);
                            }
                            else
                            {
                                MessageBox.Show("The name wasn't changed!");
                                goto Top;
                            }
                        }
                    }
                    else if (result == DialogResult.No)
                    {
                        result = MessageBox.Show(null, "Do you want to merge the keyframes of the entries?",
                            "Merge Keyframes?", MessageBoxButtons.YesNoCancel);
                        if (result == DialogResult.Yes)
                        {
                            KeyframeEntry kfe2 = null;

                            if (_extTarget.FrameCount > node.FrameCount)
                            {
                                node.SetSize(_extTarget.FrameCount, Loop);
                            }

                            //Merge all external keyframes with the current entry.
                            for (int x = 0; x < _extTarget.FrameCount; x++)
                            {
                                for (int i = 0; i < 9; i++)
                                {
                                    if ((kfe = _extTarget.GetKeyframe(i, x)) != null)
                                    {
                                        if ((kfe2 = node.GetKeyframe(i, x)) == null)
                                        {
                                            node.SetKeyframe(i, x, kfe._value);
                                        }
                                        else
                                        {
                                            result = MessageBox.Show(null,
                                                "A keyframe at frame " + x + " already exists.\nOld value: " +
                                                kfe2._value + "\nNew value:" + kfe._value +
                                                "\nReplace the old value with the new one?", "Replace Keyframe?",
                                                MessageBoxButtons.YesNoCancel);
                                            if (result == DialogResult.Yes)
                                            {
                                                node.SetKeyframe(i, x, kfe._value);
                                            }
                                            else if (result == DialogResult.Cancel)
                                            {
                                                Restore();
                                                return;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (result == DialogResult.Cancel)
                        {
                            Restore();
                            return;
                        }
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        Restore();
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Adds an animation opened by the user to the end of this one
        /// </summary>
        public void Append()
        {
            CHR0Node external = null;
            OpenFileDialog o = new OpenFileDialog
            {
                Filter = "CHR0 Animation (*.chr0)|*.chr0",
                Title = "Please select an animation to append."
            };
            if (o.ShowDialog() == DialogResult.OK)
            {
                if ((external = (CHR0Node) NodeFactory.FromFile(null, o.FileName)) != null)
                {
                    Append(external);
                }
            }
        }

        /// <summary>
        /// Adds an animation to the end of this one
        /// </summary>
        public void Append(CHR0Node external)
        {
            Append(external, 1);
        }

        public void Append(CHR0Node external, int timesToAppend)
        {
            int extCount = external.FrameCount;

            for (int appendCount = 0; appendCount < timesToAppend; appendCount++)
            {
                int origIntCount = FrameCount;
                FrameCount += extCount;

                foreach (CHR0EntryNode extEntry in external.Children)
                {
                    CHR0EntryNode intEntry = null;
                    KeyframeEntry kfe;
                    if ((intEntry = (CHR0EntryNode) FindChild(extEntry.Name, false)) == null)
                    {
                        CHR0EntryNode newIntEntry = new CHR0EntryNode {Name = extEntry.Name};
                        newIntEntry.SetSize(extEntry.FrameCount + origIntCount, Loop);
                        for (int x = 0; x < extEntry.FrameCount; x++)
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                if ((kfe = extEntry.GetKeyframe(i, x)) != null)
                                {
                                    newIntEntry.Keyframes.SetFrameValue(i, x + origIntCount, kfe._value)._tangent =
                                        kfe._tangent;
                                }
                            }
                        }

                        AddChild(newIntEntry);
                    }
                    else
                    {
                        for (int x = 0; x < extEntry.FrameCount; x++)
                        {
                            for (int i = 0; i < 9; i++)
                            {
                                if ((kfe = extEntry.GetKeyframe(i, x)) != null)
                                {
                                    intEntry.Keyframes.SetFrameValue(i, x + origIntCount, kfe._value)._tangent =
                                        kfe._tangent;
                                }
                            }
                        }
                    }
                }
            }
        }

        public CHR0Node Reverse(bool appendReverse)
        {
            using (CHR0Node tempReversedCHR0 = new CHR0Node
                {Name = Name, Loop = Loop, FrameCount = FrameCount})
            {
                KeyframeEntry kfe;
                foreach (CHR0EntryNode tempEntry in Children)
                {
                    CHR0EntryNode newIntEntry = new CHR0EntryNode {Name = tempEntry.Name};
                    newIntEntry.SetSize(tempEntry.FrameCount, Loop);
                    for (int x = 0; x < tempEntry.FrameCount; x++)
                    {
                        for (int i = 0; i < 9; i++)
                        {
                            if ((kfe = tempEntry.GetKeyframe(i, x)) != null)
                            {
                                newIntEntry.Keyframes.SetFrameValue(i, FrameCount - (x + (Loop ? 0 : 1)), kfe._value)
                                        ._tangent =
                                    kfe._tangent;
                            }
                        }
                    }

                    tempReversedCHR0.AddChild(newIntEntry);
                }

                if (appendReverse)
                {
                    Append(tempReversedCHR0);
                    return this;
                }
                else
                {
                    Replace(tempReversedCHR0);
                    return tempReversedCHR0;
                }
            }
        }

        public void Port(MDL0Node baseModel)
        {
            MDL0Node model;

            OpenFileDialog dlgOpen = new OpenFileDialog
            {
                Filter = "MDL0 Model (*.mdl0)|*.mdl0",
                Title = "Select the model this animation is for..."
            };

            if (dlgOpen.ShowDialog() == DialogResult.OK)
            {
                if ((model = (MDL0Node) NodeFactory.FromFile(null, dlgOpen.FileName)) != null)
                {
                    Port(baseModel, model);
                }
            }
        }

        public void Port(MDL0Node _targetModel, MDL0Node _extModel)
        {
            MDL0BoneNode extBone;
            MDL0BoneNode bone;
            KeyframeEntry kfe;
            foreach (CHR0EntryNode _target in Children)
            {
                extBone = (MDL0BoneNode) _extModel.FindChild(_target.Name, true); //Get external model bone
                bone = (MDL0BoneNode) _targetModel.FindChild(_target.Name, true); //Get target model bone

                for (int x = 0; x < _target.FrameCount; x++)
                {
                    for (int i = 3; i < 9; i++)
                    {
                        if ((kfe = _target.GetKeyframe(i, x)) != null) //Check for a keyframe
                        {
                            if (bone != null && extBone != null)
                            {
                                switch (i)
                                {
                                    //Translations
                                    case 6: //Trans X
                                        if (Math.Round(kfe._value, 4) == Math.Round(extBone._bindState.Translate._x, 4))
                                        {
                                            kfe._value = bone._bindState.Translate._x;
                                        }
                                        else if (bone._bindState.Translate._x < extBone._bindState.Translate._x)
                                        {
                                            kfe._value -=
                                                extBone._bindState.Translate._x - bone._bindState.Translate._x;
                                        }
                                        else if (bone._bindState.Translate._x > extBone._bindState.Translate._x)
                                        {
                                            kfe._value +=
                                                bone._bindState.Translate._x - extBone._bindState.Translate._x;
                                        }

                                        break;
                                    case 7: //Trans Y
                                        if (Math.Round(kfe._value, 4) == Math.Round(extBone._bindState.Translate._y, 4))
                                        {
                                            kfe._value = bone._bindState.Translate._y;
                                        }
                                        else if (bone._bindState.Translate._y < extBone._bindState.Translate._y)
                                        {
                                            kfe._value -=
                                                extBone._bindState.Translate._y - bone._bindState.Translate._y;
                                        }
                                        else if (bone._bindState.Translate._y > extBone._bindState.Translate._y)
                                        {
                                            kfe._value +=
                                                bone._bindState.Translate._y - extBone._bindState.Translate._y;
                                        }

                                        break;
                                    case 8: //Trans Z
                                        if (Math.Round(kfe._value, 4) == Math.Round(extBone._bindState.Translate._z, 4))
                                        {
                                            kfe._value = bone._bindState.Translate._z;
                                        }
                                        else if (bone._bindState.Translate._z < extBone._bindState.Translate._z)
                                        {
                                            kfe._value -=
                                                extBone._bindState.Translate._z - bone._bindState.Translate._z;
                                        }
                                        else if (bone._bindState.Translate._z > extBone._bindState.Translate._z)
                                        {
                                            kfe._value +=
                                                bone._bindState.Translate._z - extBone._bindState.Translate._z;
                                        }

                                        break;

                                    //Rotations
                                    //case 3: //Rot X
                                    //    difference = bone._bindState.Rotate._x - extBone._bindState.Rotate._x;
                                    //    kfe._value += difference;
                                    //    //if (difference != 0)
                                    //    //    FixChildren(bone, 0);
                                    //    break;
                                    //case 4: //Rot Y
                                    //    difference = bone._bindState.Rotate._y - extBone._bindState.Rotate._y;
                                    //    kfe._value += difference;
                                    //    //if (difference != 0)
                                    //    //    FixChildren(bone, 1);
                                    //    break;
                                    //case 5: //Rot Z
                                    //    difference = bone._bindState.Rotate._z - extBone._bindState.Rotate._z;
                                    //    kfe._value += difference;
                                    //    //if (difference != 0)
                                    //    //    FixChildren(bone, 2);
                                    //    break;
                                }
                            }

                            if (kfe._value == float.NaN || kfe._value == float.PositiveInfinity ||
                                kfe._value == float.NegativeInfinity)
                            {
                                kfe.Remove();
                                _target.Keyframes._keyArrays[i]._keyCount--;
                            }
                        }
                    }
                }
            }

            _changed = true;
        }

        private void FixChildren(MDL0BoneNode node, int axis)
        {
            KeyframeEntry kfe;
            foreach (MDL0BoneNode b in node.Children)
            {
                CHR0EntryNode _target = (CHR0EntryNode) FindChild(b.Name, true);
                if (_target != null)
                {
                    switch (axis)
                    {
                        case 0: //X, correct Y and Z
                            for (int l = 0; l < _target.FrameCount; l++)
                            {
                                for (int g = 3; g < 6; g++)
                                {
                                    if (g != 3)
                                    {
                                        if ((kfe = _target.GetKeyframe(g, l)) != null)
                                        {
                                            kfe._value *= -1;
                                        }
                                    }
                                }
                            }

                            break;
                        case 1: //Y, correct X and Z
                            for (int l = 0; l < _target.FrameCount; l++)
                            {
                                for (int g = 3; g < 6; g++)
                                {
                                    if (g != 4)
                                    {
                                        if ((kfe = _target.GetKeyframe(g, l)) != null)
                                        {
                                            kfe._value *= -1;
                                        }
                                    }
                                }
                            }

                            break;
                        case 2: //Z, correct X and Y
                            for (int l = 0; l < _target.FrameCount; l++)
                            {
                                for (int g = 3; g < 6; g++)
                                {
                                    if (g != 5)
                                    {
                                        if ((kfe = _target.GetKeyframe(g, l)) != null)
                                        {
                                            kfe._value *= -1;
                                        }
                                    }
                                }
                            }

                            break;
                    }
                }

                FixChildren(b, axis);
            }
        }

        public void AverageKeys()
        {
            foreach (CHR0EntryNode w in Children)
            {
                for (int i = 0; i < 9; i++)
                {
                    if (w.Keyframes._keyArrays[i]._keyCount > 1)
                    {
                        KeyframeEntry root = w.Keyframes._keyArrays[i]._keyRoot;
                        if (root._next != root && root._prev != root && root._prev != root._next)
                        {
                            float tan = (root._next._tangent + root._prev._tangent) / 2.0f;
                            float val = (root._next._value + root._prev._value) / 2.0f;

                            root._next._tangent = tan;
                            root._prev._tangent = tan;

                            root._next._value = val;
                            root._prev._value = val;
                        }
                    }
                }
            }

            SignalPropertyChange();
        }

        public void AverageKeys(string boneName)
        {
            CHR0EntryNode w = FindChild(boneName, false) as CHR0EntryNode;
            if (w == null)
            {
                return;
            }

            for (int i = 0; i < 9; i++)
            {
                if (w.Keyframes._keyArrays[i]._keyCount > 1)
                {
                    KeyframeEntry root = w.Keyframes._keyArrays[i]._keyRoot;
                    if (root._next != root && root._prev != root && root._prev != root._next)
                    {
                        float tan = (root._next._tangent + root._prev._tangent) / 2.0f;
                        float val = (root._next._value + root._prev._value) / 2.0f;

                        root._next._tangent = tan;
                        root._prev._tangent = tan;

                        root._next._value = val;
                        root._prev._value = val;
                    }
                }
            }

            SignalPropertyChange();
        }

        #endregion
    }

    public unsafe class CHR0EntryNode : ResourceNode, IKeyframeSource
    {
        internal CHR0Entry* Header => (CHR0Entry*) WorkingUncompressed.Address;
        public override ResourceType ResourceFileType => ResourceType.CHR0Entry;

        [Browsable(false)] public int FrameCount => Keyframes.FrameLimit;

        internal KeyframeCollection _keyframes;

        [DisplayName("Uncompressed Size (Bytes)")]
        public override uint UncompressedSize =>
            (uint) (AnimationConverter.CalculateCHR0Size(Keyframes, out _, out _) + _entryLen);

        [Browsable(false)]
        public KeyframeCollection Keyframes
        {
            get
            {
                if (_keyframes == null)
                {
                    _keyframes = AnimationConverter.DecodeKeyframes(Header, Parent as CHR0Node, 9, 1, 1, 1);
                }

                return _keyframes;
            }
        }

        private bool _useModelScale,
            _useModelRotate,
            _useModelTranslate,
            _scaleCompApply,
            _scaleCompParent,
            _classicScaleOff;

        //#if DEBUG

        public bool UseModelScale
        {
            get => _useModelScale;
            set
            {
                _useModelScale = value;
                SignalPropertyChange();
            }
        }

        public bool UseModelRotate
        {
            get => _useModelRotate;
            set
            {
                _useModelRotate = value;
                SignalPropertyChange();
            }
        }

        public bool UseModelTranslate
        {
            get => _useModelTranslate;
            set
            {
                _useModelTranslate = value;
                SignalPropertyChange();
            }
        }

        public bool ScaleCompensateApply
        {
            get => _scaleCompApply;
            set
            {
                _scaleCompApply = value;
                SignalPropertyChange();
            }
        }

        public bool ScaleCompensateParent
        {
            get => _scaleCompParent;
            set
            {
                _scaleCompParent = value;
                SignalPropertyChange();
            }
        }

        public bool ClassicScaleOff
        {
            get => _classicScaleOff;
            set
            {
                _classicScaleOff = value;
                SignalPropertyChange();
            }
        }

#if DEBUG
        public AnimationCode Flags => _code;
#endif

        private AnimationCode _code;

        internal int _dataLen;
        internal int _entryLen;
        internal VoidPtr _dataAddr;

        public bool Equals(CHR0EntryNode obj)
        {
            RegenCode();
            obj.RegenCode();

            if (_code != obj._code || _entryLen != obj._entryLen || _dataLen != obj._dataLen)
            {
                return false;
            }
            
            return Keyframes.Equals(obj.Keyframes);
        }

        public override int OnCalculateSize(bool force)
        {
            _dataLen = AnimationConverter.CalculateCHR0Size(Keyframes, out _entryLen, out _code);
            return _dataLen + _entryLen;
        }

        public override bool OnInitialize()
        {
            _keyframes = null;

            if (_name == null && Header->_stringOffset != 0)
            {
                _name = Header->ResourceString;
            }

            _code = Header->Code;

            _useModelScale = _code.UseModelScale;
            _useModelRotate = _code.UseModelRot;
            _useModelTranslate = _code.UseModelTrans;
            _scaleCompApply = _code.ScaleCompApply;
            _scaleCompParent = _code.ScaleCompParent;
            _classicScaleOff = _code.ClassicScaleOff;

            return false;
        }

        public override void Export(string outPath)
        {
            StringTable table = new StringTable
            {
                _name
            };

            int dataLen = OnCalculateSize(true);
            int totalLen = dataLen + table.GetTotalSize();

            using (FileStream stream = new FileStream(outPath, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                FileShare.None, 8, FileOptions.RandomAccess))
            {
                stream.SetLength(totalLen);
                using (FileMap map = FileMap.FromStream(stream))
                {
                    AnimationConverter.EncodeCHR0Keyframes(Keyframes, map.Address, map.Address + _entryLen, _code);
                    table.WriteTable(map.Address + dataLen);
                    PostProcess(map.Address, table);
                }
            }
        }

        public void RegenCode()
        {
            _code.UseModelScale = _useModelScale;
            _code.UseModelRot = _useModelRotate;
            _code.UseModelTrans = _useModelTranslate;
            _code.ScaleCompApply = _scaleCompApply;
            _code.ScaleCompParent = _scaleCompParent;
            _code.ClassicScaleOff = _classicScaleOff;
        }

        public override void OnRebuild(VoidPtr address, int length, bool force)
        {
            RegenCode();
            AnimationConverter.EncodeCHR0Keyframes(_keyframes, address, _dataAddr, _code);
        }

        protected internal virtual void PostProcess(VoidPtr dataAddress, StringTable stringTable)
        {
            CHR0Entry* header = (CHR0Entry*) dataAddress;
            header->ResourceStringAddress = stringTable[Name] + 4;
        }

        internal void SetSize(int count, bool looped)
        {
            Keyframes.FrameLimit = count + (looped ? 1 : 0);
            Keyframes.Loop = looped;
            SignalPropertyChange();
        }

        #region Keyframe Management

        public static bool _generateTangents = true;
        public static bool _alterAdjTangents = true;
        public static bool _alterAdjTangents_KeyFrame_Set = true;
        public static bool _alterAdjTangents_KeyFrame_Del = true;

        public float GetFrameValue(int arrayIndex, float index, bool returnOutValue = false)
        {
            return Keyframes.GetFrameValue(arrayIndex, index, returnOutValue);
        }

        public KeyframeEntry GetKeyframe(int arrayIndex, int index)
        {
            return Keyframes.GetKeyframe(arrayIndex, index);
        }

        public KeyframeEntry SetKeyframe(int arrayIndex, int index, float value, bool forceNoGenTans = false,
                                         bool parsing = false)
        {
            KeyframeEntry k = Keyframes.SetFrameValue(arrayIndex, index, value, parsing);

            if (!forceNoGenTans)
            {
                bool exists = Keyframes.GetKeyframe(arrayIndex, index) != null;
                if (_generateTangents || !exists)
                {
                    k.GenerateTangent();
                }

                if (_generateTangents && _alterAdjTangents && _alterAdjTangents_KeyFrame_Set)
                {
                    k._prev.GenerateTangent();
                    k._next.GenerateTangent();
                }
            }

            SignalPropertyChange();
            return k;
        }

        public void SetKeyframe(int index, CHRAnimationFrame frame)
        {
            float* v = (float*) &frame;
            for (int i = 0; i < 9; i++)
            {
                SetKeyframe(i, index, *v++);
            }
        }

        public void SetKeyframeOnlyTrans(int index, CHRAnimationFrame frame)
        {
            float* v = (float*) &frame.Translation;
            for (int i = 6; i < 9; i++)
            {
                SetKeyframe(i, index, *v++);
            }
        }

        public void SetKeyframeOnlyRot(int index, CHRAnimationFrame frame)
        {
            float* v = (float*) &frame.Rotation;
            for (int i = 3; i < 6; i++)
            {
                SetKeyframe(i, index, *v++);
            }
        }

        public void SetKeyframeOnlyScale(int index, CHRAnimationFrame frame)
        {
            float* v = (float*) &frame.Scale;
            for (int i = 0; i < 3; i++)
            {
                SetKeyframe(i, index, *v++);
            }
        }

        public void SetKeyframeOnlyTrans(int index, Vector3 trans)
        {
            float* v = (float*) &trans;
            for (int i = 6; i < 9; i++)
            {
                SetKeyframe(i, index, *v++);
            }
        }

        public void SetKeyframeOnlyRot(int index, Vector3 rot)
        {
            float* v = (float*) &rot;
            for (int i = 3; i < 6; i++)
            {
                SetKeyframe(i, index, *v++);
            }
        }

        public void SetKeyframeOnlyScale(int index, Vector3 scale)
        {
            float* v = (float*) &scale;
            for (int i = 0; i < 3; i++)
            {
                SetKeyframe(i, index, *v++);
            }
        }

        public void RemoveKeyframe(int arrayIndex, int index)
        {
            KeyframeEntry k = Keyframes.Remove(arrayIndex, index);
            if (k != null && _generateTangents &&
                _alterAdjTangents && _alterAdjTangents_KeyFrame_Del)
            {
                k._prev.GenerateTangent();
                k._next.GenerateTangent();
                SignalPropertyChange();
            }
        }

        public void RemoveKeyframe(int index)
        {
            for (int i = 0; i < 9; i++)
            {
                RemoveKeyframe(i, index);
            }
        }

        public void RemoveKeyframeOnlyTrans(int index)
        {
            for (int i = 6; i < 9; i++)
            {
                RemoveKeyframe(i, index);
            }
        }

        public void RemoveKeyframeOnlyRot(int index)
        {
            for (int i = 3; i < 6; i++)
            {
                RemoveKeyframe(i, index);
            }
        }

        public void RemoveKeyframeOnlyScale(int index)
        {
            for (int i = 0; i < 3; i++)
            {
                RemoveKeyframe(i, index);
            }
        }

        public CHRAnimationFrame GetAnimFrame(int index, bool returnOutFrame = false)
        {
            CHRAnimationFrame frame = new CHRAnimationFrame {Index = index};
            float* dPtr = (float*) &frame;
            for (int x = 0; x < 9; x++)
            {
                frame.SetBool(x, Keyframes.GetKeyframe(x, index) != null);
                *dPtr++ = GetFrameValue(x, index, returnOutFrame);
            }

            return frame;
        }

        [Browsable(false)] public KeyframeArray[] KeyArrays => Keyframes._keyArrays;

        #endregion
    }
}