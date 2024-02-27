using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] private AudioSource buttonClick;
    public void playButtonClick()
    {
        buttonClick.Play();
    }
}
