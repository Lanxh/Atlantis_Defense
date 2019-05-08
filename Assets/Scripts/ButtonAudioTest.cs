using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonAudioTest : MonoBehaviour
{
    public void PlayCopter()
    {
        AudioManager.instance.Play("Suono_Copter");
    }

    public void PlayBling()
    {
        AudioManager.instance.Play("Suono_Bling");
    }
}
