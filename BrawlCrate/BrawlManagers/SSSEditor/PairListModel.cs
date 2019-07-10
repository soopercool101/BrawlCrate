using BrawlLib.SSBB;
using BrawlManagerLib;
using Newtonsoft.Json;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SSSEditor
{
    /// <summary>
    /// cshtml base class. Only exists for Intellisense.
    /// </summary>
    public class PairListPageBase : TemplateBase<PairListModel>
    {
        public new PairListModel Model
        {
            get => base.Model;
            set => base.Model = value;
        }

        public new object Raw(string rawString)
        {
            return base.Raw(rawString);
        }
    }

    public class PairListModel
    {
        public Stage[] stages { get; set; }
        public byte[][] icons { get; set; }
        public Song[] songsByStage { get; set; }
        public List<ModelPair> pairs { get; set; }
        public byte[] screen1 { get; set; }
        public byte[] screen2 { get; set; }

        public PairListModel()
        {
            icons = new byte[256][];
            songsByStage = new Song[256];
            pairs = new List<ModelPair>();
            stages = StageIDMap.Stages;
        }

        public string iconsJSON => JsonConvert.SerializeObject(icons);

        public string songsByStageJSON => JsonConvert.SerializeObject(songsByStage);

        public static ReadOnlyCollection<KeyValuePair<byte, string>> StagesByID => StageIDMap.StagesByID;
    }

    public class ModelPair
    {
        public int origId { get; set; }
        public byte stage { get; set; }
        public byte icon { get; set; }
    }
}