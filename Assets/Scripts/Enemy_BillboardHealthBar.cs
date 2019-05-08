using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BillboardHealthBar : MonoBehaviour
{
    public Camera mainCamera;     //dichiara la telecamera verso cui la healthbar guarderà

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.back,
                        mainCamera.transform.rotation * Vector3.down);
    }
    
}
