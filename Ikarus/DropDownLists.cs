using System.Linq;
using System.ComponentModel;
using BrawlLib.SSBB.ResourceNodes;
using Ikarus.MovesetFile;
using BrawlLib.SSBBTypes;

namespace Ikarus
{
    public class DropDownListBonesMDef : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            MDL0Node model = (context.Instance as MovesetEntryNode).Model;
            if (model != null)
                return new StandardValuesCollection(model._linker.BoneCache.Select(n => n.ToString()).ToList());
            return null;
        }
    }

    public class DropDownListBonesArticleMDef : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            ArticleNode article = context.Instance as ArticleNode;
            if (article != null && article._info != null && article._info._model != null)
                return new StandardValuesCollection(article._info._model.BoneCache.Select(n => n.ToString()).ToList());
            return null;
        }
    }

    public class DropDownListRequirementsMDef : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            string[] values = Manager.iRequirements;
            if (values != null)
                return new StandardValuesCollection(values);
            return null;
        }
    }

    public class DropDownListGFXFilesMDef : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            string[] values = Manager.iGFXFiles;
            if (values != null)
                return new StandardValuesCollection(values);
            return null;
        }
    }

    public class DropDownListExtNodesMDef : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            TableEntryNode[] values = (context.Instance as MovesetEntryNode)._root.ReferenceList.ToArray();
            if (values != null)
                return new StandardValuesCollection(values.Select(n => n.ToString()).ToList());
            return null;
        }
    }

    public class DropDownListEnumMDef : StringConverter
    {
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            string[] values = (context.Instance as EventEnumValue).Enums;
            if (values != null)
                return new StandardValuesCollection(values);
            return null;
        }
    }
}
