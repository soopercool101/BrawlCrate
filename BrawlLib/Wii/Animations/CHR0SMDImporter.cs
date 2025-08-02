using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BrawlLib.Wii.Animations
{
    //This file was wrote by RedStoneMatt, because he needed that importer for a little mod.
    //It ended up not even working.
    //Yikes.
    internal class CHR0SMDImporter
    {
        private class SMDFrame
        {
            public int id, boneID;
            public float posx, posy, posz, rotx, roty, rotz;

            public SMDFrame()
            {
                id = new int();
                boneID = new int();
                posx = new float();
                posy = new float();
                posz = new float();
                rotx = new float();
                roty = new float();
                rotz = new float();
            }

            public SMDFrame(int ID, int BoneID, float XPos, float YPos, float Zpos, float XRot, float YRot, float ZRot)
            {
                id = ID;
                boneID = BoneID;
                posx = XPos;
                posy = YPos;
                posz = Zpos;
                rotx = XRot;
                roty = YRot;
                rotz = ZRot;
            }
        }

        public static CHR0Node Convert(string input)
        {
            try
            {
                CHR0Node chr0 = new CHR0Node
                {
                    Name = Path.GetFileNameWithoutExtension(input),
                    Loop = false,
                    Version = 4
                };
                List<string> boneNames = new List<string>();
                List<int> boneIDs = new List<int>();
                int lineNumber = 0;
                bool isReadingBones = false;
                bool isReadingFrames = false;
                int currentFrameID = new int();
                float radianFloat = 57.29579143313326f; //Angles are given in radians. To convert them into degrees, multiply them by this float.
                List<SMDFrame> allFrames = new List<SMDFrame>();
                for (TextReader reader = new StreamReader(input); reader.Peek() != -1;) //Writing data into Lists
                {
                    string line = reader.ReadLine();

                    if (line.CompareTo("end") == 0) //When reaching this line, we stop reading what we were reading
                    {
                        isReadingBones = false;
                        isReadingFrames = false;
                    }

                    if (isReadingBones)
                    {
                        string[] splitLine = line.Split('\"');
                        int boneID = int.Parse(splitLine[0].Split(' ')[0]);
                        string boneName = splitLine[1];
                        boneIDs.Add(boneID);
                        boneNames.Add(boneName);
                    }

                    if(isReadingFrames)
                    {
                        SMDFrame currentFrame = new SMDFrame();
                        if(line.Contains("time "))
                        {
                            currentFrameID = int.Parse(line.Split(' ')[1]);
                            line = reader.ReadLine();
                            lineNumber++;
                        }
                        string[] frameNumbers = line.Split(' ');
                        
                        int boneID = int.Parse(frameNumbers[0], CultureInfo.InvariantCulture.NumberFormat);
                        float posX = float.Parse(frameNumbers[2], CultureInfo.InvariantCulture.NumberFormat);
                        float posY = float.Parse(frameNumbers[3], CultureInfo.InvariantCulture.NumberFormat);
                        float posZ = float.Parse(frameNumbers[4], CultureInfo.InvariantCulture.NumberFormat);
                        float rotX = float.Parse(frameNumbers[6], CultureInfo.InvariantCulture.NumberFormat) * radianFloat;
                        float rotY = float.Parse(frameNumbers[7], CultureInfo.InvariantCulture.NumberFormat) * radianFloat;
                        float rotZ = float.Parse(frameNumbers[8], CultureInfo.InvariantCulture.NumberFormat) * radianFloat;

                        currentFrame = new SMDFrame(currentFrameID, boneID, posX, posY, posZ, rotX, rotY, rotZ);

                        allFrames.Add(currentFrame);
                    }

                    if (lineNumber == 0) //First Line
                    {
                        if (line.CompareTo("version 1") != 0) //The first line contains the version number. It needs to be 1.
                        {
                            MessageBox.Show("Unsupported SMD Version. Please use SMD Version 1."); //I don't even know if there's versions >1 but let's not take risks
                            return null;
                        }
                    }

                    if (line.CompareTo("nodes") == 0) //When reaching this line, we start reading bones names
                    {
                        isReadingBones = true;
                    }

                    if (line.CompareTo("skeleton") == 0) //When reaching this line, we start reading frames
                    {
                        isReadingFrames = true;
                    }

                    lineNumber++;
                }

                foreach (int thisBone in boneIDs)
                {
                    CHR0EntryNode node = new CHR0EntryNode();
                    chr0.AddChild(node);
                    node.Name = boneNames[thisBone];
                    node.SetSize(currentFrameID + 1, true);

                    int keynum = 0;

                    foreach (SMDFrame currentFrame in allFrames)
                    {
                        //Keyframe Array documentation:
                        //  ---------------------
                        //  |       | X | Y | Z |
                        //  | Scale | 0 | 1 | 2 |
                        //  | Rot   | 3 | 4 | 5 |
                        //  | Pos   | 6 | 7 | 8 |
                        //  ---------------------
                        if (currentFrame.boneID == thisBone)
                        {
                            node.SetKeyframe(3, currentFrame.id, currentFrame.rotx);
                            node.SetKeyframe(4, currentFrame.id, currentFrame.roty);
                            node.SetKeyframe(5, currentFrame.id, currentFrame.rotz);
                            node.SetKeyframe(6, currentFrame.id, currentFrame.posx);
                            node.SetKeyframe(7, currentFrame.id, currentFrame.posy);
                            node.SetKeyframe(8, currentFrame.id, currentFrame.posz);

                            keynum++;
                        }
                    }

                    chr0.FrameCount = keynum;
                    chr0.Loop = true;
                }

                return chr0;
            }
            catch (Exception e)
            {
                MessageBox.Show("There was a problem importing keyframes.\n\nError:\n" + e);
                return null;
            }
        } 
    }
}
