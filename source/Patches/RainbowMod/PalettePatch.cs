using Assets.CoreScripts;
using UnityEngine;

namespace TownOfUs.RainbowMod
{ 
	
    public static class PalettePatch
    {
	    public static void Load()
	    {
	    
		    var array = new[]
		    {
			    StringNames.VitalsRED,
			    StringNames.VitalsBLUE,
			    StringNames.VitalsGRN,
			    StringNames.VitalsPINK,
			    StringNames.VitalsORGN,
			    StringNames.VitalsYLOW,
			    StringNames.VitalsBLAK,
			    StringNames.VitalsWHTE,
			    StringNames.VitalsPURP,
			    StringNames.VitalsBRWN,
			    StringNames.VitalsCYAN,
			    StringNames.VitalsLIME,
			    // New colours
			    (StringNames)999991,//"MELON",
			    (StringNames)999992,//"CHOCO",
			    (StringNames)999993,//"LTBLUE",
			    (StringNames)999994,//"BEIGE",
			    (StringNames)999995,//"LTPINK",
			    (StringNames)999996,//"TURQ",
			    (StringNames)999997,//"RNBOW",
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
			    // New colours
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
			    // New colours
			    new Color32(101, 30, 37, byte.MaxValue),
			    new Color32(30, 24, 22, byte.MaxValue),
			    new Color32(31, 65, 128, byte.MaxValue),
			    new Color32(120, 106, 83, byte.MaxValue),
			    new Color32(118, 31, 128, byte.MaxValue),
			    new Color32(31, 128, 91, byte.MaxValue),
			    new Color32(0, 0, 0, byte.MaxValue),
			    /*new Color32(0, 0, 0, byte.MaxValue),
			    new Color32(0, 0, 0, byte.MaxValue),*/
		    };
		    var array4 = new[]
		    {
			    StringNames.ColorRed,
			    StringNames.ColorBlue,
			    StringNames.ColorGreen,
			    StringNames.ColorPink,
			    StringNames.ColorOrange,
			    StringNames.ColorYellow,
			    StringNames.ColorBlack,
			    StringNames.ColorWhite,
			    StringNames.ColorPurple,
			    StringNames.ColorBrown,
			    StringNames.ColorCyan,
			    StringNames.ColorLime,
			    // New colours
			    (StringNames)999981,//"Watermelon",
			    (StringNames)999982,//"Chocolate",
			    (StringNames)999983,//"Sky Blue",
			    (StringNames)999984,//"Beige",
			    (StringNames)999985,//"Hot Pink",
			    (StringNames)999986,//"Turquoise",
			    (StringNames)999987,//"Rainbow"
		    };
		    Palette.ShortColorNames = array;
		    Palette.PlayerColors = array2;
		    Palette.ShadowColors = array3;
		    MedScanMinigame.ColorNames = array4;
		    Telemetry.ColorNames = array4;
	    }
    }
}