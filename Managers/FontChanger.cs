using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FontChanger : MonoBehaviour
{
    [SerializeField] Font[] fonts;

    private void Start()
    {
        for(int i = 0; i < fonts.Length; i++)
        {
            fonts[i].material.mainTexture.filterMode = FilterMode.Point;
        }
    }
}
