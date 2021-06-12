/*===============================================================================
Copyright (c) 2016-2017 PTC Inc. All Rights Reserved.

Confidential and Proprietary - Protected under copyright and other laws.
Vuforia is a trademark of PTC Inc., registered in the United States and other 
countries.
===============================================================================*/
using System.Collections;
using UnityEngine;
using Vuforia;

/// <summary>
/// A custom handler which uses the VuMarkManager.
/// </summary>
public class VuforiaHandler : MonoBehaviour, IVirtualButtonEventHandler, ITrackableEventHandler
{
    #region MEMBER_VARIABLES

    VuMarkManager m_VuMarkManager;
	VuMarkTarget m_ClosestVuMark;
	VuMarkTarget m_CurrentVuMark;
	public VirtualButtonBehaviour V;
	public TrackableBehaviour mTrackableBehaviour;
	int VumarkCount;
	float Timer;
	public static int TrackingType = 0;//0:Do nothing 1:Dice 2:Character card 3:Skill Card 6:NumberCard

    #endregion 


    #region UNITY_MONOBEHAVIOUR_METHODS

    void Start()
    {
        // register callbacks to VuMark Manager
		V.RegisterEventHandler(this);
		mTrackableBehaviour.RegisterTrackableEventHandler (this);
        m_VuMarkManager = TrackerManager.Instance.GetStateManager().GetVuMarkManager();
        m_VuMarkManager.RegisterVuMarkDetectedCallback(OnVuMarkDetected);
        m_VuMarkManager.RegisterVuMarkLostCallback(OnVuMarkLost);
    }

    void Update()
    {
		switch (TrackingType) {
		case 0:
			break;
		case 1:case 4:
			break;
		case 2:
			if (VumarkCount == 1) {
				foreach (VuMarkBehaviour bhr in m_VuMarkManager.GetActiveBehaviours()) {
					int Idx = int.Parse (bhr.VuMarkTarget.InstanceId.ToString ());
					if (Idx <= Configurations.Character_Amount) {
						Known.VumarkCardID_Idx = Idx;
						TimerRun ();
					}
				}
			}
			break;
		case 3:
			if (VumarkCount == 1) {
				foreach (VuMarkBehaviour bhr in m_VuMarkManager.GetActiveBehaviours()) {
					int Idx = int.Parse (bhr.VuMarkTarget.InstanceId.ToString ());
					if (Idx >= Configurations.Skill_Card_from && Idx <= Configurations.Skill_Card_to) {
						Known.VumarkCardID_Idx = Idx;
						TimerRun ();
					} else {
						QA.Invoke (Tag.UIALL_CENTRAL_HINT, "放錯卡了喔!");
					}
				}
			} else if (VumarkCount == 0) {
				QA.Invoke (Tag.UIALL_CENTRAL_HINT, Game_Manager.Current_Player.Chinese_Name + "請放入道具卡");
			}
			break;
		case 6:
			if (VumarkCount == 1) {
				foreach (VuMarkBehaviour bhr in m_VuMarkManager.GetActiveBehaviours()) {
					int Idx = int.Parse (bhr.VuMarkTarget.InstanceId.ToString ());
					if (Idx >= Configurations.NumberCardFrom && Idx <= Configurations.NumberCardTo) {
						Known.VumarkCardID_Idx = Idx;
						TimerRun ();
					} else {
						QA.Invoke (Tag.UI_MAIN_UI_BIGHINT_SHOW, "出錯卡了喔!");
					}
				}
			} else if (VumarkCount == 0) {
				QA.Invoke (Tag.UI_MAIN_UI_BIGHINT_SHOW, "請出行動卡");
			}
			break;
		}

    }

    void OnDestroy()
    {
        // unregister callbacks from VuMark Manager
        m_VuMarkManager.UnregisterVuMarkDetectedCallback(OnVuMarkDetected);
        m_VuMarkManager.UnregisterVuMarkLostCallback(OnVuMarkLost);
    }

    #endregion // UNITY_MONOBEHAVIOUR_METHODS



    #region PUBLIC_METHODS

	public void OnTrackableStateChanged(
		TrackableBehaviour.Status previousStatus,
		TrackableBehaviour.Status newStatus)
	{
		if (newStatus == TrackableBehaviour.Status.DETECTED ||
			newStatus == TrackableBehaviour.Status.TRACKED ||
			newStatus == TrackableBehaviour.Status.EXTENDED_TRACKED)
		{
			Known.ImageBackground = true;
			//if (TrackingType == 1)
			//Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " found");
			//OnTrackingFound();
		}
		else if (previousStatus == TrackableBehaviour.Status.TRACKED &&
			newStatus == TrackableBehaviour.Status.NOT_FOUND)
		{
			Known.ImageBackground = false;
			//if (TrackingType == 1)
			//Debug.Log("Trackable " + mTrackableBehaviour.TrackableName + " lost");
			//OnTrackingLost();
		}
		else
		{
			Known.ImageBackground = false;
			// For combo of previousStatus=UNKNOWN + newStatus=UNKNOWN|NOT_FOUND
			// Vuforia is starting, but tracking has not been lost or found yet
			// Call OnTrackingLost() to hide the augmentations
			//OnTrackingLost();
		}
	}


	public void OnButtonPressed(Vuforia.VirtualButtonBehaviour vb){
		Known.VirtualButtonReady = false;
		//Debug.Log ("VButtonHide " + Known.VirtualButtonReady);
		if (TrackingType == 1 || TrackingType == 4 || TrackingType == 5 || TrackingType == 7) {
			Known.DiceThrowReady = true;
			//Debug.Log ("Dice Pressed");
		}
	}

	public void OnButtonReleased(Vuforia.VirtualButtonBehaviour vb){
		Known.VirtualButtonReady = true;
		//Debug.Log ("VButtonAppear" + Known.VirtualButtonReady);
		if (TrackingType == 1 || TrackingType == 4 || TrackingType == 5 || TrackingType == 7) {
			Known.DiceThrowReady = false;
			//Debug.Log ("Dice Released");
		}
	}

	public void TimerRun(){
		Timer += Time.deltaTime;
		if (Timer > Configurations.Timer_Known_Delay) {
			switch (TrackingType) {
			case 0:
			case 1:
				break;
			case 2:
				Known.ChrCardReady = true;
				//Debug.Log ("ID:" + Known.VumarkCardID_Idx + "ChrCardReady");
				//Known.SetTrackingType (0);
				break;
			case 3:
				Known.SkillCardReady = true;
				//Debug.Log ("ID:" + Known.VumarkCardID_Idx + "SkillCardReady"+" Type is " +TrackingType);
				//Known.SetTrackingType (0);
				break;
			case 6:
				Known.NumberCardReady = true;
				break;
			}
			Timer = 0;
		}
	}
    /// <summary>
    /// This method will be called whenever a new VuMark is detected
    /// </summary>
    public void OnVuMarkDetected(VuMarkTarget target)
    {
		if (TrackingType == 1 || TrackingType == 2 || TrackingType == 3) {
			
		}
		VumarkCount += 1;
		Known.VumarkCardID_Idx = int.Parse (GetVuMarkId (target));
		//Debug.Log("New VuMark: " + GetVuMarkId(target));
    }

    /// <summary>
    /// This method will be called whenever a tracked VuMark is lost
    /// </summary>
    public void OnVuMarkLost(VuMarkTarget target)
    {
		VumarkCount -= 1;
       // Debug.Log("Lost VuMark: " + GetVuMarkId(target));
		Timer = 0;
		if (Known.VumarkCardID_Idx.ToString() == GetVuMarkId (target)) {
			Known.VumarkCardID_Idx = -1;
			//Debug.Log (Known.VumarkCardID_Idx);
		} 
		Known.Reset ();
    }

    #endregion // PUBLIC_METHODS



    #region PRIVATE_METHODS

    string GetVuMarkDataType(VuMarkTarget vumark)
    {
        switch (vumark.InstanceId.DataType)
        {
            case InstanceIdType.BYTES:
                return "Bytes";
            case InstanceIdType.STRING:
                return "String";
            case InstanceIdType.NUMERIC:
                return "Numeric";
        }
        return string.Empty;
    }

    string GetVuMarkId(VuMarkTarget vumark)
    {
        switch (vumark.InstanceId.DataType)
        {
            case InstanceIdType.BYTES:
                return vumark.InstanceId.HexStringValue;
            case InstanceIdType.STRING:
                return vumark.InstanceId.StringValue;
            case InstanceIdType.NUMERIC:
                return vumark.InstanceId.NumericValue.ToString();
        }
        return string.Empty;
    }
		

    #endregion // PRIVATE_METHODS
}
