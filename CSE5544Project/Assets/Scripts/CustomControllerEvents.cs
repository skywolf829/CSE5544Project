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

        GetComponent<VRTK_ControllerEvents>().TriggerPressed +=
            new ControllerInteractionEventHandler(VizControllerScript.instance.TriggerPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased +=
            new ControllerInteractionEventHandler(VizControllerScript.instance.TriggerReleased);


    }
}
