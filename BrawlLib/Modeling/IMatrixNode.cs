using System;
using System.Collections.Generic;
using BrawlLib.Wii.Models;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlLib.Modeling
{
    public interface IMatrixNode
    {
        List<IMatrixNodeUser> Users { get; set; }
        int NodeIndex { get; }
        Matrix Matrix { get; }
        Matrix InverseMatrix { get; }
        bool IsPrimaryNode { get; }
        List<BoneWeight> Weights { get; }
    }
}
