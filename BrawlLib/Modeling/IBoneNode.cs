using BrawlLib.Wii.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace BrawlLib.Modeling
{
    public interface IBoneNode : IMatrixNode
    {
        string Name { get; set; }
        bool Locked { get; set; }
        Matrix BindMatrix { get; }
        Matrix InverseBindMatrix { get; }
        int WeightCount { get; set; }
        FrameState BindState { get; set; }
        FrameState FrameState { get; set; }
        Color NodeColor { get; set; }
        Color BoneColor { get; set; }
        int BoneIndex { get; }
        IModel IModel { get; }
        List<Influence> LinkedInfluences { get; }
        bool IsRendering { get; set; }
        void Render(bool targetModel, ModelPanelViewport viewport, Vector3 position = new Vector3());
        void RecalcBindState(bool updateMesh, bool moveMeshWithBone, bool updateAssetLists = true);
        void RecalcFrameState(ModelPanelViewport v = null);
    }
}
