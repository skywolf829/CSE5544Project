using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
public class CustomControllerEvents : MonoBehaviour
{
    VRTK_ControllerReference controllerRef;

    GameObject selectionBubble;
    public Material transparentMat;

    
    // Start is called before the first frame update
    void Start()
    {

        GetComponent<VRTK_ControllerEvents>().GripPressed +=
            new ControllerInteractionEventHandler(VizControllerScript.instance.GripPressed);
        GetComponent<VRTK_ControllerEvents>().GripReleased +=
            new ControllerInteractionEventHandler(VizControllerScript.instance.GripReleased);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased +=
            new ControllerInteractionEventHandler(VizControllerScript.instance.TriggerReleased);
        GetComponent<VRTK_ControllerEvents>().ButtonOnePressed +=
            new ControllerInteractionEventHandler(VizControllerScript.instance.ButtonOnePressed);
        GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged +=
            new ControllerInteractionEventHandler(VizControllerScript.instance.TouchpadInput);
        GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed +=
            new ControllerInteractionEventHandler(VizControllerScript.instance.ButtonTwoPressed);

    }
}
