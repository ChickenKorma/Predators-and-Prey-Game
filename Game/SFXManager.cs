using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] AudioSource eaten, birth, lose;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        else
        {
            instance = this;
        }
    }

    public void Eat()
    {
        if (!eaten.isPlaying)
        {
            eaten.Play();
        }
    }

    public void Birth()
    {
        if (!birth.isPlaying)
        {
            birth.Play();
        }
    }

    public void Lose()
    {
        if (!lose.isPlaying)
        {
            lose.Play();
        }
    }
}
