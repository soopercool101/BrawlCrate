using System;
using System.Reflection;
using System.Runtime.Serialization;
using BrawlLib.Imaging;
using BrawlLib.Internal;
using BrawlLib.OpenGL;

namespace BrawlCrate.UI.Collision_Editor
{
    [Serializable]
    public class CollisionEditorSettings_Data : ISerializable
    {
        public bool ScalePointsWithCamera_Display;
        public bool ScalePointsWithCamera_Selection;
        public bool ReplaceSingleButtonCamPerspectiveWithTwo;

        public bool OnlySelectObjectIfCollisionObjectEquals;
        public bool AlwaysShowUndoRedoMenuOnStart;
        public int MaximumUndoRedoCount;

        public bool Copy_OnlySelectObjectIfCollisionObjectEquals;
        public bool Paste_UseWorldLinkValues;
        public bool Paste_RemoveSelectedCollisionsWhenPasting;

        public ARGBPixel BackgroundColor;

        public bool ShowStagePosition_Spawns;
        public bool ShowStagePosition_Items;
        public bool ShowStagePosition_Boundaries;

        public ViewportProjection CurrentViewportProjection;

        public bool VisibilityCheck_ShowBones;
        public byte VisibilityCheck_ShowPolygons;
        public bool VisibilityCheck_ShowAllModels;

        public Vector3 PerspectiveCameraLocation;

        public Vector3 PerspectiveCameraRotation;

        // Z value of orthographic location is used for scale as there's no such thing such as Z.
        // Only X and Y are actual values.
        public Vector3 OrthographicCameraLocation;

        public bool ResetPerspectiveOrthographicCameraToZeroZero;
        public bool ShowRectangleInZeroZero;


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo f in fields)
            {
                Type t = f.FieldType;
                info.AddValue(f.Name, f.GetValue(this));
            }
        }

        /// <summary>
        /// The settings used when Collision Editor is opened for the first time or if something 
        /// invalid happens such as the settings not being recognized.
        /// </summary>
        public static CollisionEditorSettings_Data DefaultValues()
        {
            return new CollisionEditorSettings_Data
            {
                ScalePointsWithCamera_Display = false,
                ScalePointsWithCamera_Selection = false,
                ReplaceSingleButtonCamPerspectiveWithTwo = false,

                OnlySelectObjectIfCollisionObjectEquals = false,
                AlwaysShowUndoRedoMenuOnStart = false,
                MaximumUndoRedoCount = 25,

                Copy_OnlySelectObjectIfCollisionObjectEquals = false,
                Paste_UseWorldLinkValues = true,
                Paste_RemoveSelectedCollisionsWhenPasting = false,

                BackgroundColor = new ARGBPixel(255, 0, 0, 0),

                ShowStagePosition_Spawns = true,
                ShowStagePosition_Items = false,
                ShowStagePosition_Boundaries = true,

                VisibilityCheck_ShowAllModels = true,
                VisibilityCheck_ShowPolygons = 1,
                VisibilityCheck_ShowBones = false,

                CurrentViewportProjection = ViewportProjection.Perspective,

                PerspectiveCameraLocation = new Vector3(0, 0, 0),
                PerspectiveCameraRotation = new Vector3(0, 0, 0),
                OrthographicCameraLocation = new Vector3(0, 0, 0),
                ResetPerspectiveOrthographicCameraToZeroZero = false,

                ShowRectangleInZeroZero = false
            };
        }
    }
}