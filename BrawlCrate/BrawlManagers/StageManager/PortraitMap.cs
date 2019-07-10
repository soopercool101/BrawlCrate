using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace BrawlStageManager {
	public class PortraitMap {
		public static string[] StageOrder = {
			"",
			"battlefield",
			"final",
			"dolpic",
			"mansion",
			"mariopast",// 05
			"kart",
			"donkey",
			"jungle",
			"pirates",
			"oldin",	// 10
			"norfair",
			"orpheon",
			"crayon",
			"halberd",
			"starfox",	// 15
			"stadium",
			"tengan",
			"fzero",
			"ice",
			"gw",		// 20
			"emblem",
			"madein",
			"earth",
			"palutena",
			"famicom",	// 25
			"newpork",
			"village",
			"metalgear",
			"greenhill",
			"pictchat",	// 30
			"plankton",
			"custom01",
			"custom02",
			"custom03",
			"custom04",	// 35
			"custom05",
			"custom06",
			"custom07",
			"custom08",
			"custom09",	// 40
			"custom0a",
			"custom0b",
			"custom0c",
			"custom0d",
			"custom0e",	// 45
			"custom0f",
			"custom10",
			"custom11",
			"custom12",
			"dxshrine",	// 50
			"dxyorster",
			"dxgarden",
			"dxonett",
			"dxgreens",
			"dxrcruise",// 55
			"dxcorneria",
			"dxbigblue",
			"dxzebes",
			"dxpstadium",
			"",		// 50
			"custom13",
			"custom14",
			"custom15",
			"custom16",
			"custom17",	// 55
			"custom18",
			"custom19",
			"custom1a",
			"custom1b",
			"custom1c",	// 60
			"custom1d",
			"custom1e",
			"custom1f",
			"custom20",
			"custom21",	// 65
			"custom22",
			"custom23",
			"custom24",
			"custom25",
		};

		public static int ForPac(string filename) {
			string basename = filename.ToLower().Substring(3);
			for (int i = 0; i < StageOrder.Length; i++) {
				if (StageOrder[i].Length > 0 && basename.StartsWith(StageOrder[i])) {
					return i;
				}
			}
			return 0;
		}
	}
}
