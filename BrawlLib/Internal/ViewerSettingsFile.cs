using BrawlLib.Imaging;
using BrawlLib.Internal.Windows.Controls.Model_Panel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace BrawlLib.Internal
{
    public enum ImageType
    {
        png,
        tga,
        tif,
        bmp,
        jpg,
        gif
    }

    [Serializable]
    public class ModelEditorSettings : ISerializable
    {
        public bool RetrieveCorrAnims;
        public bool DisplayExternalAnims;
        public bool DisplayNonBRRESAnims;
        public bool UseBindStateBox;
        public bool UsePixelLighting;
        public bool SyncTexToObj;
        public bool SyncObjToVIS0;
        public bool DisableBonesOnPlay;
        public bool Maximize;
        public bool GenTansCHR;
        public bool GenTansSRT;
        public bool GenTansSHP;
        public bool GenTansLight;
        public bool GenTansFog;
        public bool GenTansCam;
        public bool DisplayBRRESAnims;
        public bool SnapToColl;
        public bool FlatBoneList;
        public bool BoneListContains;

        public bool HideMainWindow;
        public bool SavePosition;
        public int _width, _height, _posX, _posY;

        public uint _rightPanelWidth;
        public uint _undoCount;
        public ImageType _imageCapFmt;

        public ARGBPixel _orbColor;
        public ARGBPixel _lineColor;
        public ARGBPixel _lineDeselectedColor;
        public ARGBPixel _floorColor;
        public ARGBPixel _bgColor;
        public ARGBPixel _stgBgColor;

        public string _screenCapPath;
        public string _liveTexFolderPath;

        public List<ModelPanelViewportInfo> _viewports;

        /// <summary>
        /// These are the settings that the model viewer will default to the first time it is opened.
        /// </summary>
        public static ModelEditorSettings Default()
        {
            ModelEditorSettings s = new ModelEditorSettings
            {
                RetrieveCorrAnims = true,
                SyncTexToObj = false,
                SyncObjToVIS0 = false,
                DisableBonesOnPlay = false,
                Maximize = false,
                GenTansCHR = true,
                GenTansSRT = true,
                GenTansSHP = true,
                GenTansLight = true,
                GenTansFog = true,
                GenTansCam = true,
                DisplayNonBRRESAnims = true,
                DisplayExternalAnims = true,
                DisplayBRRESAnims = true,
                SnapToColl = false,
                FlatBoneList = true,
                BoneListContains = false,
                UseBindStateBox = true,
                SavePosition = false,

                _imageCapFmt = 0,
                _undoCount = 50,
                _orbColor = new ARGBPixel(255, 0, 128, 0),
                _lineColor = new ARGBPixel(255, 0, 0, 128),
                _lineDeselectedColor = new ARGBPixel(255, 128, 0, 0),
                _floorColor = new ARGBPixel(255, 128, 128, 191),
                _bgColor = new ARGBPixel(255, 230, 230, 250),
                _stgBgColor = new ARGBPixel(255, 0, 0, 0),

                _viewports = new List<ModelPanelViewportInfo>
                {
                    ModelPanelViewport.DefaultPerspective.GetInfo()
                    //ModelPanelViewport.DefaultFront.GetInfo(),
                    //ModelPanelViewport.DefaultLeft.GetInfo(),
                    //ModelPanelViewport.DefaultTop.GetInfo(),
                }
            };
            foreach (ModelPanelViewportInfo v in s._viewports)
            {
                v._backColor = s._bgColor;
            }

            //s._viewports[0]._percentages = new Vector4(0.0f, 0.5f, 0.5f, 1.0f);
            //s._viewports[1]._percentages = new Vector4(0.5f, 0.5f, 1.0f, 1.0f);
            //s._viewports[2]._percentages = new Vector4(0.5f, 0.0f, 1.0f, 0.5f);
            //s._viewports[3]._percentages = new Vector4(0.0f, 0.0f, 0.5f, 0.5f);

            return s;
        }

        public ModelEditorSettings()
        {
        }

        public ModelEditorSettings(SerializationInfo info, StreamingContext ctxt)
        {
            FieldInfo[] fields = GetType().GetFields(); //Gets public fields only
            foreach (FieldInfo f in fields)
            {
                Type t = f.FieldType;
                f.SetValue(this, info.GetValue(f.Name, t));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            FieldInfo[] fields = GetType().GetFields(); //Gets public fields only
            foreach (FieldInfo f in fields)
            {
                Type t = f.FieldType;
                info.AddValue(f.Name, f.GetValue(this));
            }
        }
    }

    public static class Serializer
    {
        public static void SerializeObject(string filename, ISerializable obj)
        {
            Stream stream = File.Open(filename, FileMode.Create);
            BinaryFormatter bFormatter = new BinaryFormatter();
            bFormatter.Serialize(stream, obj);
            stream.Close();
        }

        public static ISerializable DeserializeObject(string filename)
        {
            Stream stream = File.Open(filename, FileMode.Open);
            BinaryFormatter bFormatter = new BinaryFormatter();
            ISerializable obj = (ISerializable) bFormatter.Deserialize(stream);
            stream.Close();
            return obj;
        }
    }
}