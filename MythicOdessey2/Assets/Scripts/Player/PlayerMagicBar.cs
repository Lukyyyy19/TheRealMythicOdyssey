using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMagicBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxMagic(float magic)
    {
        slider.maxValue = magic;
        slider.value = magic;
    }

    public void SetMagic(float magic)
    {
        slider.value = magic;
    }
}
