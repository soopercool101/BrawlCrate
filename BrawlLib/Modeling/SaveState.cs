using BrawlLib.Internal;
using BrawlLib.SSBB.ResourceNodes;

using System.Collections.Generic;

namespace BrawlLib.Modeling
{
    public abstract class SaveState
    {
        public bool _isUndo = true;

		// This is an object that stores extra data information
		// for just in case if something needs to be stored and 
		// later retrieve it.
		public object _tag = null;
	}

    public class CollisionState : SaveState
    {
        public List<CollisionLink> _collisionLinks;
        public List<Vector2> _linkVectors;

        public CollisionNode _collisionNode;
        public CollisionObject _collisionObject;
        public CollisionPlane _collisionPlane;
        
		public bool _split;
        public bool _merge;
        public bool _create;
        public bool _delete;
    }

    public class VertexState : SaveState
    {
        public int _animFrame;
        public CHR0Node _chr0;
        public List<Vertex3> _vertices = null;
        public List<Vector3> _weightedPositions = null;
        public IModel _targetModel;
    }

    public class BoneState : SaveState
    {
        public bool _updateBindState; //This will update the actual mesh when the bone is moved
        public bool _updateBoneOnly;  //This means the bones won't affect the mesh when moved
        public int _frameIndex = 0;
        public CHR0Node _animation;
        public IBoneNode[] _bones;
        public FrameState[] _frameStates;
        public IModel _targetModel;
    }
}