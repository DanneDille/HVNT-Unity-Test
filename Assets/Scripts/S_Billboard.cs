using UnityEngine;

public class S_Billboard : MonoBehaviour 
{ 
    void Update() { transform.LookAt(Camera.main.transform.position, -Vector3.up); } 
}
