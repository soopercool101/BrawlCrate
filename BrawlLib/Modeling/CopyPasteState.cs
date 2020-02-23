using System;
using System.Collections.Generic;
using System.Linq;

using BrawlLib.SSBB.ResourceNodes;
using BrawlLib.SSBBTypes;

namespace BrawlLib.Modeling
{
	class CopyPasteState
	{
	}


	[Serializable]
	public struct CollisionLink_S
	{
		public CollisionObject Parent;
		public int EncodeIndex;

		public Vector2 Value;
		public Vector2 RawValue;

		public SortedList<int, PlaneLinkRelationship> CollisionPlaneMembers;

		public bool LinkAlreadyCreated;

		public CollisionLink Reference;

		// A way to know which index this object is. For example, because there are a bunch of 
		// links that might be equal in terms of RawValue, Members, and its parent, this index
		// serves to make sure that only a link serves as a parent
		public int LinkIndex;

		public CollisionLink_S(CollisionLink orig)
		{
			Parent = orig._parent;
			Value = orig.Value;
			RawValue = orig._rawValue;
			EncodeIndex = orig._encodeIndex;
			Reference = null;

			LinkIndex = -1;
			CollisionPlaneMembers = null;
			LinkAlreadyCreated = false;
		}

		public void ApplyToOriginalLink(ref CollisionLink original, bool ApplyParent = false)
		{
			original._encodeIndex = EncodeIndex;
			//original.Value = RawValue;

			if (ApplyParent)
				original._parent = Parent;
		}

		public void RemoveLinkReference()
		{
			Reference = null;
		}
	}

	[Serializable]
	public struct CollisionPlane_S
	{
		public bool Created;

		public CollisionObject Parent;
		public int EncodeIndex;

		public int LinkLeft;
		public int LinkRight;

		public CollisionPlaneFlags Flags;
		public CollisionPlaneFlags2 Flags2;
		public CollisionPlaneType Type;
		public byte Material;

		public int ID;
		public CollisionPlane Reference;

		public object ExtraData;

		public CollisionPlane_S(CollisionPlane orig, int identifier)
		{
			Created = true;

			Parent = orig._parent;
			EncodeIndex = orig._encodeIndex;

			Flags = orig._flags;
			Flags2 = orig._flags2;
			Type = orig._type;
			Material = orig._material;

			ID = identifier;
			Reference = null;
			ExtraData = null;

			LinkLeft = -1;
			LinkRight = -1;
		}

		public void ApplyToOriginal(ref CollisionPlane plane)
		{
			plane._flags = Flags;
			plane._flags2 = Flags2;
			plane._type = Type;
			plane._material = Material;
		}

		public void SetPlaneReference(ref CollisionPlane source)
		{
			Reference = source;
		}
		public void RemovePlaneReference()
		{
			Reference = null;
		}

		// Copied directly from CollisionNode.cs since CollisionPlane_S is just a class
		// that is meant to be copied and prevent references from the original CollisionPlane. 
		// Feel free to change into boolean values if such values such as IsRotating, 
		// UnknownSSE, etc. will be used in the future. The only class using the following 
		// GetFlag and SetFlag (and 2 variants) is CollisionEditor_PasteOptions.
		public void SetFlag(CollisionPlaneFlags Value, bool IsTrue)
		{
			Flags = (Flags & ~Value) | (IsTrue ? Value : 0);
		}
		public void SetFlag2(CollisionPlaneFlags2 Value, bool IsTrue)
		{
			Flags2 = (Flags2 & ~Value) | (IsTrue ? Value : 0);
		}
		public bool GetFlag(CollisionPlaneFlags Value)
		{
			return (Flags & Value) != 0;
		}
		public bool GetFlag2(CollisionPlaneFlags2 Value)
		{
			return (Flags2 & Value) != 0;
		}
	}

	public struct PlaneLinkRelationship
	{
		public PlaneLinkPositioning Positioning;
	}

	public enum PlaneLinkPositioning
	{
		Left,
		Right,
		None
	}

	// Keeps copied links and planes to a single state.
	// Plus this is the class where it handles both points (links) and planes's copy state and paste
	// state.
	public class CopiedLinkPlaneState
	{
		public CollisionLink_S[] CopiedLinks;
		public CollisionPlane_S[] CopiedPlanes;

		// These should not be used unless if creating it from CreateLinksAndPlanes.
		public CollisionObject CreatedObject;
		public List<CollisionLink> CreatedLinks;
		public List<CollisionPlane> CreatedPlanes;

		public CopiedLinkPlaneState() { }

		public CopiedLinkPlaneState(CollisionLink_S[] CopiedLinks, CollisionPlane_S[] CopiedPlanes)
		{
			this.CopiedLinks = (CollisionLink_S[])CopiedLinks.Clone();
			this.CopiedPlanes = (CollisionPlane_S[])CopiedPlanes.Clone();
		}

		// Clears copied links and planes from CopiedLinkPlaneState.
		public void CreateCopyLinksAndPlanes(ref List<CollisionLink> TargetLinks, ref List<CollisionPlane> TargetPlanes,
		bool IgnoreOtherObjects, ref CollisionObject SelectedObject)
		{
			if (TargetLinks == null || TargetLinks.Count == 0)
				return;

			CopiedLinks = new CollisionLink_S[TargetLinks.Count];
			CopiedPlanes = new CollisionPlane_S[TargetPlanes.Count];

			// Helps in assigning a plane an ID so that way the copy operation does not go wrong.
			int PlaneCountID = 0;

			// Iterate through every target link available.
			for (int li = TargetLinks.Count - 1; li >= 0; --li)
			{
				CollisionLink link = TargetLinks[li];

				// First check if IgnoreOtherObjects is set.
				if (IgnoreOtherObjects)
				{
					// If true, then check if the parent is not the same. If it is not, then
					// skip this link.
					if (!ReferenceEquals(link._parent, SelectedObject))
						continue;
				}

				// Create an structure variant of CollisionLink.
				CollisionLink_S link_S = new CollisionLink_S(link);

				// Retrieve our plane members.
				List<CollisionPlane> planes = link._members;

				// Iterate through every associated plane available. This helps in knowing if there are any
				// planes that already exists.
				for (int lip = planes.Count - 1; lip >= 0; --lip)
				{
					CollisionPlane plane = planes[lip];

					// Create a structured plane that do not have any links ID yet.
					CollisionPlane_S plane_S = new CollisionPlane_S();

					// This is how it is known if the plane was referenced and copied from PlanesID.
					bool PlaneReferenced = false;

					int PlaneIDToApply = PlaneCountID;

					// If PlaneCountID is more than 0, it means that there are more planes to check.
					if (PlaneCountID > 0)
					{
						int CPLength = CopiedPlanes.Length;

						// Look up for a plane that has an equal reference since really, when
						// creating lists, we don't just deep clone it, we reference them. (Not
						// unless it were to be really deep-cloned in which ReferenceEquals would
						// always return false)
						for (int lips = PlaneCountID - 1; lips >= 0; --lips)
						{
							if (lips >= CPLength)
								break;

							// Retrieve the copied variant of the plane.
							CollisionPlane_S plane_ST = CopiedPlanes[lips];

							// Check if this plane has an equal reference to the plane stored in
							// the list. Helps with connecting links back into a plane.
							if (CollisionPlane.PlaneEquals(plane_ST.Reference, plane))
							{
								plane_S = plane_ST;
								PlaneReferenced = true;
								PlaneIDToApply = plane_ST.ID;

								break;
							}
						}
					}

					bool linkLeftNotNothing = plane._linkLeft != null;
					bool linkRightNotNothing = plane._linkRight != null;
					bool leftLinkEquals = linkLeftNotNothing && ReferenceEquals(link, plane._linkLeft);
					bool rightLinkEquals = linkRightNotNothing && ReferenceEquals(link, plane._linkRight);

					// First check if the links themselves are not equal to the main link being read.
					// It can of course, be an option if the user wants to still select planes that have
					// the other link not being selected.
					if (linkLeftNotNothing && !leftLinkEquals && !plane._linkLeft._highlight)
						continue;
					if (linkRightNotNothing && !rightLinkEquals && !plane._linkRight._highlight)
						continue;

					if (!plane_S.Created && !PlaneReferenced)
						plane_S = new CollisionPlane_S(plane, PlaneCountID);
					else if (plane_S.Created)
					{
						// This would not even make any sense, but why would at least 2 links
						// have their ID created when all are already taken over?
						if (plane_S.LinkLeft != -1 && plane_S.LinkRight != -1)
							continue;
					}

					// Create a collision plane member, in which it creates a relationship.
					if (link_S.CollisionPlaneMembers == null)
						link_S.CollisionPlaneMembers = new SortedList<int, PlaneLinkRelationship>();

					PlaneLinkRelationship relationship = new PlaneLinkRelationship();

					// Do the thing where it checks if the link is left, right, or nonexistent.
					if (leftLinkEquals)
					{
						relationship.Positioning = PlaneLinkPositioning.Left;
						plane_S.LinkLeft = li;
					}
					else if (rightLinkEquals)
					{
						relationship.Positioning = PlaneLinkPositioning.Right;
						plane_S.LinkRight = li;
					}
					else
					{
						relationship.Positioning = PlaneLinkPositioning.None;
					}

					// Add the relationship to a list of plane members to store.
					// As long as there is no key/value pair from PlaneIDToApply.
					if (!link_S.CollisionPlaneMembers.ContainsKey(PlaneIDToApply))
						link_S.CollisionPlaneMembers.Add(PlaneIDToApply, relationship);

					// Overwrite the copied plane's index to the new Plane_S.
					CopiedPlanes[PlaneIDToApply] = plane_S;

					// If the plane was not referenced then a plane reference is set and the
					// plane's count ID is increased.
					if (!PlaneReferenced)
					{
						CopiedPlanes[PlaneCountID].SetPlaneReference(ref plane);
						PlaneCountID = PlaneCountID + 1;
					}
				}

				CopiedLinks[li] = link_S;
			}

			// Check if there are available planes. If so, then remove any plane reference.
			if (CopiedPlanes.Length > 0)
			{
				for (int i = CopiedPlanes.Length - 1; i >= 0; --i)
				{
					CopiedPlanes[i].RemovePlaneReference();
				}
			}
		}

		public void CreateLinksAndPlanes(CollisionObject TargetObject, bool HighlightLinks = true, bool TakeActualLinkValue = false)
		{
			int selectedLinksLinkIndex = 0;

			if (CopiedLinks == null || CopiedLinks.Length < 1)
				return;
			if (CopiedPlanes == null || CopiedPlanes.Length < 1)
				return;

			CreatedObject = TargetObject;
			CreatedLinks = new List<CollisionLink>();
			CreatedPlanes = new List<CollisionPlane>();

			// This is where the magic begins for pasting. Let us get our copied links to memory.
			for (int cl = 0; cl < CopiedLinks.Length; cl++)
			{
				CollisionLink_S Link = CopiedLinks[cl];

				// Then check if the plane members are NOT nothing and make sure that there is a plane reference.
				if (Link.CollisionPlaneMembers != null && Link.CollisionPlaneMembers.Count > 0)
				{
					// Then get our plane reference ID.
					int[] planeReferences = Link.CollisionPlaneMembers.Keys.ToArray();

					// Then check our plane members that was just created in the copy function.
					for (int pa = 0; pa < Link.CollisionPlaneMembers.Count; ++pa)
					{
						// This is important -- we get the reference to the list of copied planes to the plane.
						int reference = planeReferences[pa];

						// If the plane's reference index is less than zero, then it is nonexistent. Since indexes only
						// are obtained at 0.
						if (reference == -1)
							continue;

						// Then retrieve our CollisionPlane_S from our reference.
						CollisionPlane_S plane = CopiedPlanes[reference];

						// If one of the links index is -1, then this plane can be officially skipped.
						if ((plane.LinkLeft == -1) || (plane.LinkRight == -1))
							continue;

						// Then get our CollisionLink_S from the plane's LinkLeft/LinkRight index.
						CollisionLink_S LeftLink_S = CopiedLinks[plane.LinkLeft];
						CollisionLink_S RightLink_S = CopiedLinks[plane.LinkRight];

						// Firct check if both links were already created. If it is, then this plane is skipped
						// since it is not necessary. The plane reference is also checked for just in case it exists, fully
						// stating that these points are already created.
						if (LeftLink_S.LinkAlreadyCreated && RightLink_S.LinkAlreadyCreated && (plane.Reference != null))
							continue;

						// The main reason why they are never initialized is due to how references work.
						// If initialized, then these links will never get deleted due to lack of link referencing.
						// Especially if they contain a reference from either LeftLink_S/RightLink_S.
						CollisionLink leftLink = null;
						CollisionLink rightLink = null;

						// Check of the link was created and has a reference. Reference is used to let us take 
						// the Link reference for a new collision plane linking.
						if (LeftLink_S.LinkAlreadyCreated && LeftLink_S.Reference != null)
							leftLink = LeftLink_S.Reference;
						else
						{
							// Maybe consider bringing the original object of this link?
							leftLink = new CollisionLink(TargetObject, TakeActualLinkValue ? LeftLink_S.Value : LeftLink_S.RawValue);

							// Copies properties from CollisionLink_S to CollisionLink.
							LeftLink_S.ApplyToOriginalLink(ref leftLink);
						}

						// Same procedure as previous.
						if (RightLink_S.LinkAlreadyCreated && RightLink_S.Reference != null)
							rightLink = RightLink_S.Reference;
						else
						{
							rightLink = new CollisionLink(TargetObject, TakeActualLinkValue ? RightLink_S.Value : RightLink_S.RawValue);
							RightLink_S.ApplyToOriginalLink(ref rightLink);
						}

						// If one of left/rightLink is nothing, then this plane will have to be skipped.
						// Errors here are not an option.
						if (leftLink == null || rightLink == null)
							continue;

						// Then a plane is created from leftLink and rightLink.
						CollisionPlane branchedPlane = new CollisionPlane(TargetObject, leftLink, rightLink);

						// Apply the original values that this copied plane used to have once it was copied.
						// If curious as to what it copies, please take a look at ApplyToOriginal in CollisionPlane_S.
						plane.ApplyToOriginal(ref branchedPlane);

						// Give the plane the reference to the created plane.
						plane.Reference = branchedPlane;

						// The links are going to be highlited, if set to.
						leftLink._highlight = HighlightLinks;
						rightLink._highlight = HighlightLinks;

						// Add the links to the list of created links. They are not needed if one already
						// exists in the selection.
						if (LeftLink_S.LinkAlreadyCreated)
							CreatedLinks[LeftLink_S.LinkIndex] = leftLink;
						else
						{
							// Create a link reference so that later LeftLink already have a reference.
							LeftLink_S.Reference = leftLink;
							LeftLink_S.LinkIndex = selectedLinksLinkIndex;
							// Add 1 to our value so that they are not marked as duplicate.
							selectedLinksLinkIndex = selectedLinksLinkIndex + 1;

							// A link is then added to the list of CreatedLinks.
							CreatedLinks.Add(leftLink);
						}

						// Same procedure as previous.
						if (RightLink_S.LinkAlreadyCreated)
							CreatedLinks[RightLink_S.LinkIndex] = rightLink;
						else
						{
							RightLink_S.Reference = rightLink;
							RightLink_S.LinkIndex = selectedLinksLinkIndex;
							selectedLinksLinkIndex = selectedLinksLinkIndex + 1;

							CreatedLinks.Add(rightLink);
						}

						// LinkAlreadyCreated makes sure that if there is one created it does not
						// create duplicates when attempting to check for link creation.
						LeftLink_S.LinkAlreadyCreated = true;
						RightLink_S.LinkAlreadyCreated = true;

						// Apply the left/right links of CopiedLinks so that they are overwritten.
						// Structures only pass these as values so it is necessary to overwrite the
						// original.
						CopiedLinks[plane.LinkLeft] = LeftLink_S;
						CopiedLinks[plane.LinkRight] = RightLink_S;

						// Then we go ahead and put our plane. Same as before with this issue.
						CopiedPlanes[reference] = plane;
					}
				}
				else
				{

				}
			}

			// Remove any paste reference that it was used during the operation.
			for (int li = CopiedLinks.Length - 1; li >= 0; --li)
			{
				var l = CopiedLinks[li];

				l.RemoveLinkReference();
				l.LinkAlreadyCreated = false;
				l.LinkIndex = -1;

				CopiedLinks[li] = l;
			}
			// Remove any reference since we don't want to make ties with a collision plane.
			for (int pl = CopiedPlanes.Length - 1; pl >= 0; --pl)
			{
				CopiedPlanes[pl].RemovePlaneReference();
			}
		}

		public void ClearLinksAndPlanes()
		{
			CreatedLinks.Clear();
			CreatedPlanes.Clear();

			CreatedObject = null;
			CreatedLinks = null;
			CreatedPlanes = null;
		}

		// Only copies structure related links and planes. In this case, it is CopiedLinks and CopiedPlanes.
		public CopiedLinkPlaneState CloneValues()
		{
			return new CopiedLinkPlaneState(CopiedLinks, CopiedPlanes);
		}
	}
}
