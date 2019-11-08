using BrawlLib.Internal.Windows.Controls.Model_Panel;
using BrawlLib.OpenGL;
using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.Wii.Models;
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace BrawlLib.Modeling
{
    public interface IModel : IRenderedObject
    {
        InfluenceManager Influences { get; }
        IBoneNode[] BoneCache { get; }
        IBoneNode[] RootBones { get; }
        IObject[] Objects { get; }
        int SelectedObjectIndex { get; set; }
        bool IsTargetModel { get; set; }

        void ResetToBindState();
        void ApplyCHR(CHR0Node node, float index);
        void ApplySRT(SRT0Node node, float index);
        void ApplySHP(SHP0Node node, float index);
        void ApplyPAT(PAT0Node node, float index);
        void ApplyVIS(VIS0Node node, float index);
        void ApplyCLR(CLR0Node node, float index);
        void ApplySCN(SCN0Node node, float index);

        void RenderVertices(bool depthPass, IBoneNode weightTarget, GLCamera camera);
        void RenderNormals();
        void RenderBoxes(bool model, bool obj, bool bone, bool bindState);
        void RenderBones(ModelPanelViewport v);
    }

    [Serializable]
    public class ModelRenderAttributes : ISerializable
    {
        public bool _renderPolygons = true;
        public bool _renderWireframe = false;
        public bool _renderBones = false;
        public bool _renderMetal = false;
        public bool _renderVertices = false;
        public bool _renderNormals = false;
        public bool _dontRenderOffscreen = false;
        public bool _renderModelBox = false;
        public bool _renderObjectBoxes = false;
        public bool _renderBoneBoxes = false;
        public bool _useBindStateBoxes = true;
        public bool _applyBillboardBones = true;
        public bool _renderShaders = true;
        public bool _scaleBones = false;
        public bool _renderBonesAsPoints = false;

        public ModelRenderAttributes()
        {
        }

        public ModelRenderAttributes(SerializationInfo info, StreamingContext ctxt)
        {
            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo f in fields)
            {
                Type t = f.FieldType;
                f.SetValue(this, info.GetValue(f.Name, t));
            }
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo f in fields)
            {
                Type t = f.FieldType;
                info.AddValue(f.Name, f.GetValue(this));
            }
        }
    }
}