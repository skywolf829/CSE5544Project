using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomControllerEvents : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<VRTK.VRTK_ControllerEvents>().TriggerClicked += 
            new VRTK.ControllerInteractionEventHandler(VizControllerScript.instance.TriggerPressed);
        GetComponent<VRTK.VRTK_ControllerEvents>().TriggerReleased +=
            new VRTK.ControllerInteractionEventHandler(VizControllerScript.instance.TriggerReleased);
        //GetComponent<VRTK.VRTK_ControllerEvents>().TouchpadAxisChanged +=
        //    new VRTK.ControllerInteractionEventHandler(VizControllerScript.instance.TouchpadInput);


    }

}
