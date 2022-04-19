using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorTrigger : MonoBehaviour
{
    bool once = true;

    // Update is called once per frame
    void Update()
    {
        if (AracControl.instance.isAracActive && once)
        {
            once = false;
            GetComponent<Animator>().SetTrigger("run");
        }
        else if (!AracControl.instance.isAracActive && !once)
		{
            GetComponent<Animator>().SetTrigger("idle");
            once = true;
        }

    }
}
