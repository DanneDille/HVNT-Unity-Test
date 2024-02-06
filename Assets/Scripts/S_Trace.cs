using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;


public class S_Trace : MonoBehaviour
{
    public Camera aCamera;
    private Ray ray;
    public float traceRange = 2.0f;

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }
    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }


    private void FingerDown(EnhancedTouch.Finger finger) //player touch input
    {
        if (finger.index != 0) return;//only allows one finger presses
        RaycastHit hit;
        ray = new Ray(aCamera.ScreenToWorldPoint(new Vector3(Input.GetTouch(0).position.x, Input.GetTouch(0).position.y, 0)), aCamera.transform.forward);
        RaycastHit[] hits = Physics.RaycastAll(ray, traceRange);
        if (Physics.Raycast(ray, out hit, traceRange))

            if (hit.collider.TryGetComponent<S_ChestAmim>(out S_ChestAmim chestRef))//is hit the chest
            {
                if (hit.collider.GetComponent<S_ChestAmim>().chestState == S_ChestAmim.ChestState.Shaking)//if chest is Shaking, tell it to open
                {
                    hit.collider.GetComponent<S_ChestAmim>().setChestState(S_ChestAmim.ChestState.Open);
                    GetComponent<S_BeaconManager>().SpawnBall(chestRef.transform.position + new Vector3(0, 0.48f, 0));
                }
            }
    }
                     
}