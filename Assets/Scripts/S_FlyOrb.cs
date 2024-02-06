using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_FlyOrb : MonoBehaviour
{
    // Start is called before the first frame update
    [HideInInspector]
    public S_BeaconManager beaconManager;


    [SerializeField]
    private List<MeshRenderer> meshes = new List<MeshRenderer>();
    [Header("FlyOrb Speed per second")]
    [Range(0.1f, 10f)]
    public float travelSpeed = 2.2f;

    [Space(10)]

    [Header("Intro Anim settiongs")]
    public float targetHeight = 1.5f;
    public float introDuration = 4;

    [SerializeField]
    private AnimationCurve curve_Intro;

    private TrailRenderer trail;
    private AudioSource hitSound;
    
    
    void Start()
    {
        hitSound = GetComponent<AudioSource>();
        trail = GetComponent<TrailRenderer>();
        StartCoroutine(IntroAnimation());
    }

    // Update is called once per frame
    IEnumerator IntroAnimation() //Animation
    {
        Vector3 startPos = transform.position;
        float CurrentTime = 0.0f;
        while (CurrentTime < introDuration)
        {
            CurrentTime += Time.deltaTime;
            transform.position = startPos + new Vector3(startPos.x, startPos.y + curve_Intro.Evaluate(Mathf.Clamp(CurrentTime,0,1)) * targetHeight, startPos.z);//Moves the Orb into the sky

            yield return null;
        }
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            if (beaconManager != null)
            {
                StartCoroutine(Travel());
                break;
            }        
            yield return null;
        }
        
    }
    IEnumerator Travel() //Animation
    {
        int targetIndex = 0;//current becon target
        float CurrentTime = 1.0f;
        while (true)
        {
            GameObject targetObject = beaconManager.spawnedGameObjects[targetIndex];
            CurrentTime = 1.0f;//used for a speed boost at the beginning of 
            while (Vector3.Distance(targetObject.transform.position, transform.position) > 0.15) //Animates FlyOrb movements
            {
                CurrentTime = Mathf.Clamp(CurrentTime - Time.deltaTime, 0, 1);

                //Turn FlyOrb towards target Beacon  
                transform.rotation = Quaternion.Lerp(transform.rotation,
                                                    Quaternion.LookRotation(targetObject.transform.position - transform.position, Vector3.up),
                                                    Mathf.Lerp((Time.deltaTime * 5), 1, 1 - Mathf.Clamp(Vector3.Distance(targetObject.transform.position, transform.position), 0, 1))); 


                transform.position += transform.forward * (Time.deltaTime * (travelSpeed + (travelSpeed * CurrentTime))); // Move Forwards
                yield return null;
            }
            hitSound.Stop();//stops hitsSound to prevent sound overlaping
            hitSound.Play();
            UpdateOrbColor(targetObject.GetComponent<IGetHit>().GetHit());
            targetIndex = (targetIndex + 1) % beaconManager.spawnedGameObjects.Count;//get next Beacon (lopping)
            yield return null;
            
        }
    }
    private void UpdateOrbColor(Color aColor)
    {
        foreach (MeshRenderer mesh in meshes)
        {
            mesh.material.SetColor("_EmissionColor", aColor); //Swap meshes color to target Beacon color
        }
        trail.material.SetColor("_EmissionColor", aColor);//Swap trail color to target Beacon color
    }
}
