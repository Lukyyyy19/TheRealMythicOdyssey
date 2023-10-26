using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextGameOver : MonoBehaviour
{
    private TMP_Text _text;

    private void Awake()
    {
        _text = GetComponent<TMP_Text>();
    }

    private void Start()
    {
        _text.text = GameManager.Instance.enemiesKilled.ToString();
    }
}
