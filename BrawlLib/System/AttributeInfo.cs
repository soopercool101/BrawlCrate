using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace System
{
    public class AttributeInfo {
		public string _name;
		public string _description;
		public int _type;

		//0 == float/radians
		//1 == int
		//2 == float radians to degrees
	}

	public class AttributeInterpretation {
		public AttributeInfo[] Array { get; private set; }
		public string Filename { get; private set; }
		public int NumEntries {
			get {
				return Array.Length;
			}
		}

		public AttributeInterpretation(AttributeInfo[] array, string saveToFile) {
			this.Array = array;
			this.Filename = saveToFile;
		}

		public AttributeInterpretation(string filename) {
			this.Filename = filename;

			List<AttributeInfo> list = new List<AttributeInfo>();
			int index = 0x14;
			if (filename != null && File.Exists(filename)) {
				using (var sr = new StreamReader(filename)) {
					for (int i = 0; !sr.EndOfStream /*&& i < NumEntries*/; i++) {
						AttributeInfo attr = new AttributeInfo();
						attr._name = sr.ReadLine();
						attr._description = sr.ReadLine();
						string num = sr.ReadLine();
						try {
							attr._type = int.Parse(num);
						} catch (FormatException ex) {
							throw new FormatException("Invalid type \"" + num + "\" in " + Path.GetFileName(filename) + ".", ex);
						}

						if (attr._description == "") attr._description = "No Description Available.";

						list.Add(attr);
						sr.ReadLine();
						index++;
					}
				}
			}
			this.Array = list.ToArray();
		}

		public override string ToString() {
			return Path.GetFileNameWithoutExtension(this.Filename);
		}

		public void Save() {
			string dir = Path.GetDirectoryName(Filename);
			if (!Directory.Exists(dir)) {
                Directory.CreateDirectory(dir);
			}
			if (File.Exists(Filename)) {
				if (DialogResult.Yes != MessageBox.Show("Overwrite " + Filename + "?", "Overwrite",
					MessageBoxButtons.YesNo)) return;
			}
			using (var sw = new StreamWriter(Filename)) {
				foreach (AttributeInfo attr in Array) {
					sw.WriteLine(attr._name);
					sw.WriteLine(attr._description);
					sw.WriteLine(attr._type);
					sw.WriteLine();
				}
			}
		}
	}
}
