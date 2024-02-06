using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_BeaconScript : MonoBehaviour, ICanBeColored,IGetHit
{

    //RotateAnimation Varibles
    public float rotationSpeed = 50;
    private float speedup = 0;
    float rotation;
    private Color myColor;
    Vector3 Dir;
    //

    public List<MeshRenderer> meshes = new List<MeshRenderer>();

    void Start()
    {
        Dir = Random.onUnitSphere;
        StartCoroutine(RotateAnimation());
    }
    public void SetColor(Color newColor) // ICanBeColored //Set the color of the Beacon from S_beaconManager when Beacon Spawned 
    {
        myColor = newColor;
        UpdateColor(myColor);
    }
    public void UpdateColor(Color aColor) //apply color to meshes
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.color = aColor;
        }
    }
    IEnumerator RotateAnimation() //Animation
    {
        while (true)
        {           
            rotation += Time.deltaTime * (rotationSpeed * Mathf.Lerp(1,3, speedup));
            transform.rotation = Quaternion.Euler(Dir * rotation);
            yield return null;
        }
    }

    public Color GetHit()// IGetHit //triggerd by S_FlyOrb, adds some feedack to when it gets hit
    {
        if (speedup <= 0) StartCoroutine(SpeedBostAnim());
        else speedup = 1;
        return myColor;
    }
    IEnumerator SpeedBostAnim() //Animation
    {
        speedup = 1;
        while (speedup > 0)
        {
            speedup = Mathf.Clamp(speedup - (Time.deltaTime * 2), 0, 1);
            transform.localScale = Vector3.Lerp(new Vector3(1, 1, 1), new Vector3(1.3f, 1.3f, 1.3f), speedup);
            UpdateColor(Color.Lerp(myColor, Color.white, speedup));
            yield return null;
        }
    }



 }
