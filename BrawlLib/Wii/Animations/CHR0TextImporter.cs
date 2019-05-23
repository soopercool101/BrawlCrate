using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using BrawlLib.SSBB.ResourceNodes;
using System.Windows.Forms;

namespace BrawlLib.Wii.Animations
{
    //Credit goes to AgoAJ for the original converter. I have made some changes, though.
    class CHR0TextImporter
    {
        class Coords
        {
            public List<float> x, y, z;
            public Coords()
            {
                x = new List<float>();
                y = new List<float>();
                z = new List<float>();
            }
            public int highestCount { get { return Math.Max(Math.Max(x.Count, y.Count), z.Count); } }
        }
        static Coords getCoords(TextReader reader, out string line)
        {
            Coords output = new Coords();
            for (line = reader.ReadLine(); line != null && line.CompareTo("	") != 0 && (line.CompareTo("X") == 0 || line.CompareTo("Y") == 0 || line.CompareTo("Z") == 0); line = reader.ReadLine())
                if (line.CompareTo("X") == 0)
                    for (line = reader.ReadLine(); line != null && line.CompareTo("	") != 0; line = reader.ReadLine())
                        output.x.Add(float.Parse(line));
                else if (line.CompareTo("Y") == 0)
                    for (line = reader.ReadLine(); line != null && line.CompareTo("	") != 0; line = reader.ReadLine())
                        output.y.Add(float.Parse(line));
                else if (line.CompareTo("Z") == 0)
                    for (line = reader.ReadLine(); line != null && line.CompareTo("	") != 0; line = reader.ReadLine())
                        output.z.Add(float.Parse(line));
            
            return output;
        }
        public static CHR0Node Convert(string input)
        {
            try
            {
                CHR0Node chr0 = new CHR0Node();
                chr0.Name = Path.GetFileNameWithoutExtension(input);
                chr0.Loop = false;
                chr0.Version = 4;

                for (TextReader reader = new StreamReader(input); reader.Peek() != -1; )
                {
                    string line = reader.ReadLine();
                    if (line.CompareTo("Animation Keyframe Data Frames") == 0)
                    {
                        line = reader.ReadLine();

                        //Remove any non-digit characters
                        string justNumbers = new String(line.Where(Char.IsDigit).ToArray());
                        int keyFrameCount = int.Parse(justNumbers);
                        chr0.FrameCount = keyFrameCount;

                        for (line = reader.ReadLine(); line != null && line.CompareTo("End Of Animation Data") != 0; line = reader.ReadLine())
                        {
                            if (line.CompareTo("Keyframe Data") == 0)
                            {
                                CHR0EntryNode node = new CHR0EntryNode();
                                chr0.AddChild(node);
                                node.Name = reader.ReadLine();
                                node.SetSize(keyFrameCount, chr0.Loop);

                                Coords translation = new Coords();
                                Coords rotation = new Coords();
                                Coords scale = new Coords();

                                for (line = reader.ReadLine(); line != null && line.CompareTo("End of Keyframe Data") != 0; line = reader.ReadLine())
                                {
                                //Use goto to retry the loop without advancing the reader, when it returns from getCoords it will have the next line we need from it.
                                retry:
                                    if (line.CompareTo("Translation") == 0)
                                    {
                                        reader.ReadLine(); //Skip first blank line
                                        translation = getCoords(reader, out line);
                                        goto retry;
                                    }
                                    else if (line.CompareTo("Rotation") == 0)
                                    {
                                        reader.ReadLine(); //Skip first blank line
                                        rotation = getCoords(reader, out line);
                                        goto retry;
                                    }
                                    else if (line.CompareTo("Scale") == 0)
                                    {
                                        reader.ReadLine(); //Skip first blank line
                                        scale = getCoords(reader, out line);
                                        goto retry;
                                    }
                                    else if (line.CompareTo("End of Keyframe Data") == 0) //If line equals this now it's time to break
                                        break;
                                }
                                //Add frames to node
                                for (int frameCount = 0; translation.highestCount > frameCount || rotation.highestCount > frameCount || scale.highestCount > frameCount; frameCount++)
                                {
                                    if (translation.highestCount > frameCount)
                                    {
                                        if (translation.x.Count != 0 && frameCount < translation.x.Count)
                                            node.SetKeyframe(6, frameCount, translation.x[frameCount]);
                                        if (translation.y.Count != 0 && frameCount < translation.y.Count)
                                            node.SetKeyframe(7, frameCount, translation.y[frameCount]);
                                        if (translation.z.Count != 0 && frameCount < translation.z.Count)
                                            node.SetKeyframe(8, frameCount, translation.z[frameCount]);
                                    }
                                    if (rotation.highestCount > frameCount)
                                    {
                                        if (rotation.x.Count != 0 && frameCount < rotation.x.Count)
                                            node.SetKeyframe(3, frameCount, rotation.x[frameCount]);
                                        if (rotation.y.Count != 0 && frameCount < rotation.y.Count)
                                            node.SetKeyframe(4, frameCount, rotation.y[frameCount]);
                                        if (rotation.z.Count != 0 && frameCount < rotation.z.Count)
                                            node.SetKeyframe(5, frameCount, rotation.z[frameCount]);
                                    }
                                    if (scale.highestCount > frameCount)
                                    {
                                        if (scale.x.Count != 0 && frameCount < scale.x.Count)
                                            node.SetKeyframe(0, frameCount, scale.x[frameCount]);
                                        if (scale.y.Count != 0 && frameCount < scale.y.Count)
                                            node.SetKeyframe(1, frameCount, scale.y[frameCount]);
                                        if (scale.z.Count != 0 && frameCount < scale.z.Count)
                                            node.SetKeyframe(2, frameCount, scale.z[frameCount]);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Not a valid CHR0 text file.");
                        return null;
                    }
                }
                return chr0;
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem importing keyframes.\n\nError:\n" + e.ToString());
                return null;
            }
        }
    }
}
