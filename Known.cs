using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Known : MonoBehaviour {

	public static bool DiceThrowReady;
	public static bool ChrCardReady;
	public static bool SkillCardReady;
	public static bool ImageBackground;
	public static bool VirtualButtonReady;
	public static bool NumberCardReady;

//	public static bool P1_Ready;
//	public static bool P2_Ready;
//	public static bool Players_Ready;
	public static int VumarkCardID_Idx = -1;


	public static void Reset(){
		DiceThrowReady = false;
		ChrCardReady = false;
		SkillCardReady = false;
		NumberCardReady = false;
		ImageBackground = true;
	}

	public static void SetTrackingType(int T){
		VuforiaHandler.TrackingType = T;
		//Vuforia.VuforiaBehaviour.Instance.enabled = (T == 0) ? false : true;
		//Game_Manager.TrackingTypeChange (T);
		Reset ();
		//0:Do nothing
		//1:Throw Dice
		//2:Chr Card
		//3:Skill Card
	}
}
