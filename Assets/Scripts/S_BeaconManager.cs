using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(requiredComponent: typeof(S_Trace))]

public class S_BeaconManager : MonoBehaviour , IHaveIndex
{

    private S_Trace aS_Trace;
    private Ray ray;

    [Header("Spawn Settings")]
    public GameObject spawnGameObjectPrefab;
    public List<Color> TotalVariants;
    [HideInInspector]
    public List <GameObject> spawnedGameObjects;//chache of all Instantiated Beacons
    public float placeRange = 2;

    [Space(10)]

    public GameObject flyOrbPrefab;

    [Space(5)]

    public Camera aCamera;
    public S_ButtonManager buttonManager;

    [Space(10)]

    public S_ChestAmim[] allChests;

    void Start()
    {
        aS_Trace = GetComponent<S_Trace>();
        spawnedGameObjects = new List<GameObject>(new GameObject[TotalVariants.Count]);
        if (buttonManager != null)buttonManager.SpawnButtons(TotalVariants, gameObject);
        
    }

    public void GiveIndex(int newIndex)// IHaveIndex //
    {
        if(spawnedGameObjects[newIndex] == null)//if no spawned objet at index, spawn one
        {
            spawnedGameObjects[newIndex] = Instantiate(original: spawnGameObjectPrefab, position: new Vector3(0,0,0), rotation: new Quaternion(0, 0, 0, 0));
            spawnedGameObjects[newIndex].GetComponent<ICanBeColored>().SetColor(TotalVariants[newIndex]);
            if (!spawnedGameObjects.Contains(null))
            {
                foreach(S_ChestAmim chest in allChests)
                { 
                    if (chest != null) chest.setChestState(S_ChestAmim.ChestState.Shaking);
                }
            }
        }
        spawnedGameObjects[newIndex].transform.position = traceBeacon();
        spawnedGameObjects[newIndex].transform.rotation = new Quaternion(0, 0, Random.value * 360, 0);       
    }
    public Color GetBeaconColor(int index)
    {
        return TotalVariants[index];
    }
    public Vector3 GetBeaconPos(int index)
    {
        return spawnedGameObjects[index].transform.position;
    }
    public Vector3 traceBeacon()
    {
        RaycastHit hit;
        ray = new Ray(aCamera.transform.position, aCamera.transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        else
        {
            return aCamera.transform.position + (aCamera.transform.forward * placeRange);
        }
    }
    public void SpawnBall(Vector3 startPos)
    {
        GameObject flyOrb = Instantiate(original: flyOrbPrefab, position: startPos, rotation: new Quaternion(0, 0, 0, 0));
        flyOrb.GetComponent<S_FlyOrb>().beaconManager = GetComponent<S_BeaconManager>();

    }
}
