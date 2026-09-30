using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace BrawlLib.Wii.Animations
{
    public class AnimFormat
    {
        private static readonly string[] types = {"scale", "rotate", "translate"};
        private static readonly string[] axes = {"X", "Y", "Z"};

        public static void Serialize(CHR0Node node, string output)
        {
            Dictionary<CHR0Node, string> dict = new Dictionary<CHR0Node, string>();
            dict.Add(node, output);
            Serialize(dict);
        }

        public static void Serialize(Dictionary<CHR0Node, string> exports)
        {
            MDL0Node model;

            OpenFileDialog dlgOpen = new OpenFileDialog
            {
                Filter = "MDL0 Model (*.mdl0)|*.mdl0",
                Title = "Select the model this animation is for..."
            };

            if (dlgOpen.ShowDialog() != DialogResult.OK ||
                (model = (MDL0Node) NodeFactory.FromFile(null, dlgOpen.FileName)) == null)
            {
                return;
            }

            foreach (KeyValuePair<CHR0Node, string> node in exports)
            {
                Serialize(node.Key, node.Value, model);
            }
        }

        public static void Serialize(CHR0Node node, string output, MDL0Node model)
        {
            model.Populate();
            using (StreamWriter file = new StreamWriter(output))
            {
                file.WriteLine("animVersion 1.1;");
                file.WriteLine("mayaVersion 2015;");
                file.WriteLine("timeUnit ntscf;");
                file.WriteLine("linearUnit cm;");
                file.WriteLine("angularUnit deg;");
                file.WriteLine("startTime 1;");
                file.WriteLine($"endTime {node.FrameCount};");
                foreach (MDL0BoneNode b in model.AllBones)
                {
                    if (!(node.FindChild(b.Name, true) is CHR0EntryNode e))
                    {
                        file.WriteLine($"anim {b.Name} 0 {b.Children.Count} 0;");
                        continue;
                    }

                    KeyframeCollection c = e.Keyframes;
                    int counter = 0;
                    for (int index = 0; index < 9; index++)
                    {
                        KeyframeArray array = c._keyArrays[index];

                        if (array._keyCount <= 0)
                        {
                            continue;
                        }

                        file.WriteLine("anim {0}.{0}{1} {0}{1} {2} {3} {4} {5};", types[index / 3], axes[index % 3],
                            e.Name, 0, b.Children.Count, counter);
                        file.WriteLine("animData {");
                        file.WriteLine("  input time;");
                        file.WriteLine($"  output {(index > 2 && index < 6 ? "angular" : "linear")};");
                        file.WriteLine("  weighted 0;");
                        file.WriteLine("  preInfinity constant;");
                        file.WriteLine("  postInfinity constant;");
                        file.WriteLine("  keys {");
                        for (KeyframeEntry entry = array._keyRoot._next; entry != array._keyRoot; entry = entry._next)
                        {
                            float angle = (float) Math.Atan(entry._tangent) * Maths._rad2degf;
                            float angle2 = angle;
                            if (entry._next != array._keyRoot && //Done to allow in/out key handle splits to store properly.
                                entry._next._value == entry._value &&
                                entry._next._index == entry._index)
                            {
                                entry = entry._next;
                                angle2 = (float)Math.Atan(entry._tangent) * Maths._rad2degf;
                            }
                            file.WriteLine("    {0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10};",
                                entry._index+1,
                                entry._value.ToString(CultureInfo.InvariantCulture.NumberFormat),
                                "fixed",
                                "fixed",
                                "1",
                                "1",
                                "0",
                                angle.ToString(CultureInfo.InvariantCulture.NumberFormat),
                                "1",
                                angle2.ToString(CultureInfo.InvariantCulture.NumberFormat),
                                "1");
                        }

                        file.WriteLine("  }");
                        file.WriteLine("}");
                        counter++;
                    }

                    if (counter == 0)
                    {
                        file.WriteLine($"anim {b.Name} 0 {b.Children.Count} 0;");
                    }
                }
            }
        }

        public static CHR0Node Read(string input)
        {
            CHR0Node node = new CHR0Node {_name = Path.GetFileNameWithoutExtension(input)};
            using (StreamReader file = new StreamReader(input))
            {
                float start = 0.0f;
                float end = 0.0f;
                float frameAlign = 0.0f;
                double angMode = Maths._deg2rad, tanMode;
                bool isStartZero = true, isHSDimport = false, isHSDmayaVer = false, isMeleeModel = false;
                string line;
                while (true)
                {
                    line = file.ReadLine();

                    if (line == null)
                    {
                        break;
                    }

                    int i = line.IndexOf(' ');
                    string tag = line.Substring(0, i);

                    if (tag == "anim")
                    {
                        break;
                    }

                    string val = line.Substring(i + 1, line.IndexOf(';') - i - 1);

                    switch (tag)
                    {
                        case "startTime":
                        case "startUnitless":
                            float.TryParse(val, out start);
                            if (start == 0)
                                isStartZero = true;
                            break;
                        case "endTime":
                        case "endUnitless":
                            float.TryParse(val, out end);
                            break;
                        case "angularUnit":
                            string[] angLine = line.Split(' ');
                            if (angLine[1].Contains("deg"))
                                angMode = Maths._deg2rad;
                            else if (angLine[1].Contains("rad"))
                                angMode = 1.0d;
                            break;
                        case "mayaVersion":
                            string[] verLine = line.Split(' ');
                            if (verLine[1].Contains("2015"))
                                isHSDmayaVer = true;
                            break;
                        case "animVersion":
                        case "timeUnit":
                        case "linearUnit":
                        default:
                            break;
                    }
                }

                int frameCount = (int) (end - start + 1.5f);
                node.FrameCount = frameCount;
                if (start == 0.0f)
                {
                    end++;
                } 


                while (true)
                {
                    if (line == null)
                    {
                        break;
                    }

                    string[] anim = line.Split(' ');
                    if (anim.Length != 7)
                    {
                        while ((line = file.ReadLine()) != null && !line.StartsWith("anim "))
                        {
                            ;
                        }

                        continue;
                    }

                    string t = anim[2];
                    string bone = anim[3];
                    if (bone.Contains("JOBJ_"))
                        isMeleeModel = true;
                    int mode = -1;
                    if (t.StartsWith("scale"))
                    {
                        if (t.EndsWith("X"))
                        {
                            mode = 0;
                        }
                        else if (t.EndsWith("Y"))
                        {
                            mode = 1;
                        }
                        else if (t.EndsWith("Z"))
                        {
                            mode = 2;
                        }
                    }
                    else if (t.StartsWith("rotate"))
                    {
                        if (t.EndsWith("X"))
                        {
                            mode = 3;
                        }
                        else if (t.EndsWith("Y"))
                        {
                            mode = 4;
                        }
                        else if (t.EndsWith("Z"))
                        {
                            mode = 5;
                        }
                    }
                    else if (t.StartsWith("translate"))
                    {
                        if (t.EndsWith("X"))
                        {
                            mode = 6;
                        }
                        else if (t.EndsWith("Y"))
                        {
                            mode = 7;
                        }
                        else if (t.EndsWith("Z"))
                        {
                            mode = 8;
                        }
                    }

                    if (mode == -1)
                    {
                        while ((line = file.ReadLine()) != null && !line.StartsWith("anim "))
                        {
                            ;
                        }

                        continue;
                    }

                    line = file.ReadLine();

                    if (line.StartsWith("animData"))
                    {
                        CHR0EntryNode e;
                        if (angMode == Maths._deg2rad)
                            tanMode = 1.0;
                        else
                            tanMode = Maths._rad2deg;

                        if ((e = node.FindChild(bone, false) as CHR0EntryNode) == null)
                        {
                            e = new CHR0EntryNode { _name = bone };
                            node.AddChild(e);
                        }

                        while (true)
                        {
                            line = file.ReadLine().TrimStart();
                            int i = line.IndexOf(' ');

                            if (i < 0)
                            {
                                break;
                            }

                            string tag = line.Substring(0, i);

                            if (tag == "keys")
                            {
                                List<KeyframeEntry> l = new List<KeyframeEntry>();
                                List<string> lines = new List<string>();
                                while (true)
                                {
                                    string newline = file.ReadLine().TrimStart();
                                    if (newline == "}")
                                    {
                                        break;
                                    }
                                    lines.Add(newline);
                                }
                                for (int k = 0; k < lines.Count; k++)
                                {
                                    line = lines[k];

                                    string[] s = line.Split(' ');

                                    for (int si = 0; si < s.Length; si++)
                                    {
                                        s[si] = s[si].Trim('\n', ';', ' ');
                                    }


                                    float.TryParse(s[0], NumberStyles.Number, CultureInfo.InvariantCulture,
                                        out float inVal);
                                    float.TryParse(s[1], NumberStyles.Number, CultureInfo.InvariantCulture,
                                        out float outVal);
                                    if (start == 0.0f)
                                        inVal++;

                                    if (angMode == 1.0d && (mode >= 3 && mode <= 5)) //only do so for rotation if accepting radians!
                                    {
                                        outVal = (float)(Maths._rad2deg * (double)outVal);
                                    }
                                    float weight1 = 0;
                                    float weight2 = 0;

                                    float angle1 = 0;
                                    float angle2 = 0;

                                    bool firstFixed = false, secondFixed = false;
                                    bool isAuto = false, hasStep = false;
                                    switch (s[2])
                                    {
                                        case "linear":
                                        case "auto":
                                            isAuto = true;
                                            break;
                                        case "spline":
                                        case "fixed":
                                            firstFixed = true;
                                            float.TryParse(s[7], NumberStyles.Number, CultureInfo.InvariantCulture,
                                                out angle1);
                                            float.TryParse(s[8], NumberStyles.Number, CultureInfo.InvariantCulture,
                                                out weight1);
                                            break;
                                    }

                                    switch (s[3])
                                    {
                                        case "linear":
                                        case "auto":
                                            break;
                                        case "spline":
                                        case "fixed":
                                            secondFixed = true;
                                            if (firstFixed)
                                            {
                                                float.TryParse(s[9], NumberStyles.Number, CultureInfo.InvariantCulture,
                                                    out angle2);
                                                float.TryParse(s[10], NumberStyles.Number, CultureInfo.InvariantCulture,
                                                    out weight2);
                                            }
                                            else
                                            {
                                                float.TryParse(s[7], NumberStyles.Number, CultureInfo.InvariantCulture,
                                                    out angle2);
                                                float.TryParse(s[8], NumberStyles.Number, CultureInfo.InvariantCulture,
                                                    out weight2);
                                            }

                                            break;
                                        case "step":
                                            secondFixed = true;
                                            hasStep = true;
                                            angle2 = 0;
                                            weight2 = 1;
                                            break;
                                    }

                                    bool anyFixed = secondFixed || firstFixed;
                                    bool bothFixed = secondFixed && firstFixed;
                                    isHSDimport = isHSDmayaVer && isStartZero && isMeleeModel;
                                    
                                    KeyframeEntry x = e.SetKeyframe(mode, (int) (inVal - 0.5f), outVal, true);
                                    if (isAuto && hasStep)
                                    {
                                        x._tangent = 0;
                                        if (x._prev != null)
                                        {
                                            x._tangent = x._prev._tangent;
                                            x.InsertAfter(new KeyframeEntry(x)
                                            { _tangent = 0, _isStep = true });
                                        }
                                        else //don't waste data on the first key! Only have the step handle.
                                            x._isStep = true;
                                    }
                                    else if (!anyFixed)
                                    {
                                        l.Add(x);
                                    }
                                    else
                                    {
                                        if (bothFixed)
                                        {
                                            float outTangent = 0.0f;
                                            if (!isHSDimport)
                                            {
                                                if (mode >= 3 && mode <= 5)
                                                {
                                                    x._tangent = (float)(tanMode * Math.Tan((double)angle1 * angMode));
                                                    outTangent = (float)(tanMode * Math.Tan((double)angle2 * angMode));
                                                }
                                                else
                                                {
                                                    x._tangent = (float)(tanMode * Math.Tan((double)angle1 * Maths._deg2rad));
                                                    outTangent = (float)(tanMode * Math.Tan((double)angle2 * Maths._deg2rad));
                                                }
                                            }
                                            else //Set up this way to not be fancy. 
                                            {
                                                //When HSD's Melee exports load fine, this entire section can be removed.
                                                if (mode >= 3 && mode <= 5)
                                                {
                                                    x._tangent = (float)((double)angle1 * angMode);
                                                    outTangent = (float)((double)angle2 * angMode);
                                                }
                                                else
                                                {
                                                    x._tangent = (float)((double)angle1 * Maths._deg2rad);
                                                    outTangent = (float)((double)angle2 * Maths._deg2rad);
                                                }
                                            }
                                            

                                                
                                            if (Math.Round(x._tangent,5) != Math.Round(outTangent,5))
                                            {
                                                x.InsertAfter(new KeyframeEntry(x)
                                                { _tangent = outTangent });
                                            }
                                            //anticpate step
                                            if (x._prev != null && 
                                                x._prev._isStep)
                                            {
                                                x._prev.InsertAfter(new KeyframeEntry(x._index - 1, x._prev._value, x._parent)
                                                {   _tangent = 0    });
                                            }
                                        }
                                        else if (firstFixed)
                                        {
                                            if (!isHSDimport)
                                                x._tangent = (float)(tanMode * Math.Tan(((double)angle1 * angMode) * weight1));
                                            else
                                                x._tangent = (float)((double)angle1 * angMode) * weight1;
                                        }
                                        else
                                        {
                                            if (!isHSDimport)
                                                x._tangent = (float)(tanMode * Math.Tan(((double)angle2 * angMode) * weight2));
                                            else
                                                x._tangent = (float)((double)angle2 * angMode) * weight2;
                                        }
                                    }
                                }

                                foreach (KeyframeEntry w in l)
                                {
                                    w.GenerateTangent();
                                }
                            }
                            else
                            {
                                int z = line.IndexOf(';') - i - 1;
                                if (z < 0)
                                {
                                    continue;
                                }

                                string val = line.Substring(i + 1, z);

                                switch (tag)
                                {
                                    case "input":

                                        break;
                                    case "output":

                                        break;
                                    case "weighted":

                                        break;
                                    case "inputUnit":

                                        break;
                                    case "outputUnit":

                                        break;
                                    case "tangentAngleUnit":
                                        string[] angLine = line.Split(' ');
                                        if (angLine[1].Contains("deg"))
                                            tanMode = 1.0;
                                        else if (angLine[1].Contains("rad"))
                                            tanMode = Maths._rad2deg;
                                        break;
                                    case "preInfinity":
                                    case "postInfinity":
                                    default:
                                        break;
                                }
                            }
                        }
                    }

                    line = file.ReadLine();
                }
            }

            return node;
        }
    }
}