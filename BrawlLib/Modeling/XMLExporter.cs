using BrawlLib.SSBB.ResourceNodes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace BrawlLib.Modeling
{
    public class XMLExporter
    {
        internal static void ExportRMDL(MDL0Node model, string outPath)
        {
            DateTime now = DateTime.Now;
            string user = Environment.UserName;
            string generatorName = "BrawlLib";
            string generatorVer = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion;

            rmdlType rmdl = new rmdlType();
            nw4r_3difType data = new nw4r_3difType()
            {
                version = "1.5.0",
                head = new headType()
                {
                    create = new createType()
                    {
                        user = user,
                        host = user,
                        date = now,
                        source = "",
                    },
                    title = "Model Data",
                    generator = new generatorType()
                    {
                        name = generatorName,
                        version = generatorVer,
                    },
                },
                body = new bodyType()
                {
                    rmdl = rmdl
                },
            };

            rmdl.file_log_array = new file_log_arrayType()
            {
                size = 1,
                file_log = new List<file_logType>()
                {
                    new file_logType()
                    {
                        index = 0,
                        user = user,
                        host = user,
                        date = now,
                        name = generatorName,
                        version = generatorVer,
                        Text = new List<file_logValue>()
                        {
                            file_logValue.export,
                            file_logValue.triangle_strip,
                        },
                    },
                },
            };

            rmdl.model_info = new model_infoType()
            {
                tool_start_frame = 1,
                magnify = 1,
                scaling_rule = (scaling_ruleType)model.ScalingRule,
                tex_matrix_mode = (tex_matrix_modeType)model.TextureMatrixMode,

            };

            Export(data, outPath);
        }

        private static void Export(object o, string outPath)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                using (StreamWriter stream = new StreamWriter(outPath))
                    serializer.Serialize(stream, o);
            }
            catch (Exception ex)
            {
                string s = "";
                while (ex.InnerException != null)
                {
                    s += ex.Message + "\n\n";
                    ex = ex.InnerException;
                }
                MessageBox.Show(s + "\n\n" + ex.Message);
            }
        }
    }
}
