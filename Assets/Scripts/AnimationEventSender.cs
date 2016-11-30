using UnityEngine;
using System.Collections;

public class AnimationEventSender : MonoBehaviour {

	public void SendAnimationDoneMessage()
    {
        DialogueManager._instance.AnimationPlaying = false;
    }
}
