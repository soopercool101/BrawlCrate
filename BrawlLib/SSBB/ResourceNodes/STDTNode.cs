using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using BrawlLib.SSBBTypes;
using System.Linq;
using System.Windows.Forms;

namespace BrawlLib.SSBB.ResourceNodes
{
    public unsafe class STDTNode : ARCEntryNode, IAttributeList {
        public override ResourceType ResourceType { get { return ResourceType.STDT; } }
        internal STDT* Header { get { return (STDT*)WorkingUncompressed.Address; } }
        internal int version, unk1, unk2;

        // Internal buffer for editing - changes written back to WorkingUncompressed on rebuild
        internal UnsafeBuffer entries;

        [Category("Stage Trap Data Table")]
        public int NumEntries{get{return entries.Length / 4;}}
        [Category("Stage Trap Data Table")]
        public int Unk1 { get { return unk1; } set { unk1 = value; SignalPropertyChange(); } }
        [Category("Stage Trap Data Table")]
        public int Unk2 { get { return unk2; } set { unk2 = value; SignalPropertyChange(); } }

        public override bool OnInitialize() {
            version = Header->_version;
            unk1 = Header->_unk1;
            unk2 = Header->_unk2;

            entries = new UnsafeBuffer(WorkingUncompressed.Length - 0x14);
            Memory.Move(entries.Address, Header->Entries, (uint)entries.Length);
            return false;
        }

        protected override string GetName() {
            return base.GetName("STDT");
        }

        public override void OnRebuild(VoidPtr address, int length, bool force) {
            STDT* header = (STDT*)address;
            header->_tag = STDT.Tag;
            header->_unk1 = unk1;
            header->_unk2 = unk2;
            header->_version = version;
            header->_entryOffset = 0x14;
            Memory.Move(header->Entries, entries.Address, (uint)entries.Length);
        }

        public override int OnCalculateSize(bool force) {
            return WorkingUncompressed.Length;
        }

        internal static ResourceNode TryParse(DataSource source) { return ((STDT*)source.Address)->_tag == STDT.Tag ? new STDTNode() : null; }
        [Browsable(false)]
        public VoidPtr AttributeAddress {
            get {
                return entries.Address;
            }
        }
        public void SetFloat(int index, float value) {
            if (((bfloat*)AttributeAddress)[index] != value) {
                ((bfloat*)AttributeAddress)[index] = value;
                SignalPropertyChange();
            }
        }
        public float GetFloat(int index) {
            return ((bfloat*)AttributeAddress)[index];
        }
        public void SetInt(int index, int value) {
            if (((bint*)AttributeAddress)[index] != value) {
                ((bint*)AttributeAddress)[index] = value;
                SignalPropertyChange();
            }
        }
        public int GetInt(int index) {
            return ((bint*)AttributeAddress)[index];
        }

		public IEnumerable<AttributeInterpretation> GetPossibleInterpretations() {
			ReadConfig();
			ResourceNode root = this;
			while (root.Parent != null) root = root.Parent;
			var q = from f in STDTFormats
					where 0x14 + f.NumEntries * 4 == WorkingUncompressed.Length
					select f;

			bool any_match_name = q.Any(f => String.Equals(
				Path.GetFileNameWithoutExtension(f.Filename),
				root.Name.Replace("STG", ""),
				StringComparison.InvariantCultureIgnoreCase));
			if (!any_match_name) q = q.Concat(new AttributeInterpretation[] { GenerateDefaultInterpretation() });

			q = q.OrderBy(f => !String.Equals(
				Path.GetFileNameWithoutExtension(f.Filename),
				root.Name.Replace("STG", ""),
				StringComparison.InvariantCultureIgnoreCase));

			return q;
		}

		private AttributeInterpretation GenerateDefaultInterpretation() {
			AttributeInfo[] arr = new AttributeInfo[NumEntries];
			buint* pIn = (buint*)AttributeAddress;
			int index = 0x14;
			for (int i = 0; i < arr.Length; i++) {
				arr[i] = new AttributeInfo() {
					_name = "0x" + index.ToString("X3")
				};
				//Guess if the value is a an integer or float
				uint u = (uint)*((buint*)pIn);
				float f = (float)*((bfloat*)pIn);
				if (*pIn == 0) {
					arr[i]._type = 0;
					arr[i]._description = "Default: 0 (could be int or float - be careful)";
				} else if (((u >> 24) & 0xFF) != 0 && *((bint*)pIn) != -1 && !float.IsNaN(f)) {
					float abs = Math.Abs(f);
					if (abs > 0.0000001 && abs < 10000000) {
						arr[i]._type = 0;
						arr[i]._description = "Default (float): " + f + " (" + u.ToString("X8") + ")";
					} else {
						arr[i]._type = 1;
						arr[i]._description = "Default (unknown type): " + u + " (" + u.ToString("X8") + ")";
						arr[i]._name = "~" + arr[i]._name;
					}
				} else {
					arr[i]._type = 1;
					arr[i]._description = "Default (int): " + u + " (" + u.ToString("X8") + ")";
					arr[i]._name = "*" + arr[i]._name;
				}
				index += 4;
				pIn++;
			}

			ResourceNode root = this;
			while (root.Parent != null) root = root.Parent;
			string filename = "STDT/" + root.Name.Replace("STG", "") + ".txt";
			return new AttributeInterpretation(arr, filename);
		}

		private static List<AttributeInterpretation> STDTFormats = new List<AttributeInterpretation>();
		private static HashSet<string> configpaths_read = new HashSet<string>();

		private static void ReadConfig() {
			if (Directory.Exists("STDT")) {
				foreach (string path in Directory.EnumerateFiles("STDT", "*.txt")) {
					if (configpaths_read.Contains(path)) continue;
					configpaths_read.Add(path);
					try {
						STDTFormats.Add(new AttributeInterpretation(path));
					} catch (FormatException ex) {
						if (Properties.Settings.Default.HideMDL0Errors) {
							Console.Error.WriteLine(ex.Message);
						} else {
							MessageBox.Show(ex.Message);
						}
					}
				}
			}
		}
    }
}
