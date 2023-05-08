using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ChromaticAberrationEffect : MonoBehaviour
{
    private Volume postProcess;
    private ChromaticAberration ch;

    private bool lowHealthEffect = false;
    private float maxIntensityValue = 0.8f;
    private float startIntensity = 0f;

    private bool goUp = true;

    // Start is called before the first frame update
    void Start()
    {
        postProcess = GetComponent<Volume>();
        postProcess.profile.TryGet(out ch);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(lowHealthEffect)
        {

            if(goUp)
            {
                startIntensity += 0.01f;
                ch.intensity.value = startIntensity;
                Debug.Log("going up");
                if(startIntensity >= maxIntensityValue)
                {
                    goUp = false;
                }
            }
            else
            {
                startIntensity -= 0.01f;
                ch.intensity.value = startIntensity;
                Debug.Log("going down");
                if(startIntensity <= 0.2f)
                {
                    goUp = true;
                }
            }


        }
        else
        {
            ch.intensity.value = 0f;
        }
        
    }

    public void SetLowHealthEffect(bool active)
    {
        lowHealthEffect = active;
    }
}
