using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configurations : MonoBehaviour {

	public static int Character_Amount = 8;
	public static int Skill_Card_from = 9;
	public static int Skill_Card_to = 45;
	public static int NumberCardFrom = 46;
	public static int NumberCardTo = 51;
	public static bool NumberCardMode = true;
	public static bool GWW_EndlessMode = true;
	public static float Timer_Known_Delay = 0f;
	public static int DefaultHeartValue = 1;
	public static int MaxHeartValue = 10;
	public static int DefaultHungerValue = 5;
	public static int MaxHungerValue = 20;
	public static int DefaultMoneyValue = 200;
	public static int NormalMapEndPoint = 20;
	public static int VegasMapEndPoint = 30;
	public static float SequencePlayTime = 0.033f;
	public static float D02plus = 1.5f;
	public static bool upsidedown;
	public static bool[] chrs_on = new bool[4];
	//public static bool AllVendorClose;
	//public static bool AllVendorPriceTwice;
}
