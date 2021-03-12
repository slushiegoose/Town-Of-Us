using UnityEngine;

namespace TownOfUs.RainbowMod
{ 
    public static class PalettePatch
    {
	    public static void Load()
	    {
	    
		    var array = new[]
		    {
			    "RED",
			    "BLUE",
			    "GRN",
			    "PINK",
			    "ORNG",
			    "YLOW",
			    "BLAK",
			    "WHTE",
			    "PURP",
			    "BRWN",
			    "CYAN",
			    "LIME",
			    "MELON",
			    "CHOCO",
			    "LTBLUE",
			    "BEIGE",
			    "LTPINK",
			    "TURQ",
			    "RNBOW",
			    /*"GLXY",
			    "FIRE",*/

		    };
		    var array2 = new[]
		    {
			    new Color32(198, 17, 17, byte.MaxValue),
			    new Color32(19, 46, 210, byte.MaxValue),
			    new Color32(17, 128, 45, byte.MaxValue),
			    new Color32(238, 84, 187, byte.MaxValue),
			    new Color32(240, 125, 13, byte.MaxValue),
			    new Color32(246, 246, 87, byte.MaxValue),
			    new Color32(63, 71, 78, byte.MaxValue),
			    new Color32(215, 225, 241, byte.MaxValue),
			    new Color32(107, 47, 188, byte.MaxValue),
			    new Color32(113, 73, 30, byte.MaxValue),
			    new Color32(56, byte.MaxValue, 221, byte.MaxValue),
			    new Color32(80, 240, 57, byte.MaxValue),
			    new Color32(168, 50, 62, byte.MaxValue),
			    new Color32(60, 48, 44, byte.MaxValue),
			    new Color32(61, 129, 255, byte.MaxValue),
			    new Color32(240, 211, 165, byte.MaxValue),
			    new Color32(236, 61, 255, byte.MaxValue),
			    new Color32(61, 255, 181, byte.MaxValue),
			    new Color32(0, 0, 0, byte.MaxValue),
			    /*new Color32(0, 0, 0, byte.MaxValue),
			    new Color32(0, 0, 0, byte.MaxValue),*/
		    };
		    var array3 = new[]
		    {
			    new Color32(122, 8, 56, byte.MaxValue),
			    new Color32(9, 21, 142, byte.MaxValue),
			    new Color32(10, 77, 46, byte.MaxValue),
			    new Color32(172, 43, 174, byte.MaxValue),
			    new Color32(180, 62, 21, byte.MaxValue),
			    new Color32(195, 136, 34, byte.MaxValue),
			    new Color32(30, 31, 38, byte.MaxValue),
			    new Color32(132, 149, 192, byte.MaxValue),
			    new Color32(59, 23, 124, byte.MaxValue),
			    new Color32(94, 38, 21, byte.MaxValue),
			    new Color32(36, 169, 191, byte.MaxValue),
			    new Color32(21, 168, 66, byte.MaxValue),
			    new Color32(168, 50, 62, byte.MaxValue),
			    new Color32(60, 48, 44, byte.MaxValue),
			    new Color32(61, 129, 255, byte.MaxValue),
			    new Color32(240, 211, 165, byte.MaxValue),
			    new Color32(236, 61, 255, byte.MaxValue),
			    new Color32(61, 255, 181, byte.MaxValue),
			    new Color32(0, 0, 0, byte.MaxValue),
			    /*new Color32(0, 0, 0, byte.MaxValue),
			    new Color32(0, 0, 0, byte.MaxValue),*/
		    };
		    var array4 = new string[]
		    {
			    "Red",
			    "Blue",
			    "Green",
			    "Pink",
			    "Orange",
			    "Yellow",
			    "Black",
			    "White",
			    "Purple",
			    "Brown",
			    "Cyan",
			    "Lime",
			    "Watermelon",
			    "Chocolate",
			    "Sky Blue",
			    "Beige",
			    "Hot Pink",
			    "Turquoise",
			    "Rainbow"
		    };
		    Palette.ShortColorNames = array;
		    Palette.PlayerColors = array2;
		    Palette.ShadowColors = array3;
		    MedScanMinigame.ColorNames = array4;
	    }
    }
}