using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class S_ChestAmim : MonoBehaviour
{


    public enum ChestState
    {
        Idle,  //Chest lies Still, can't be interated with
        Shaking, //Chest jumps around, can be open
        Open, //Chest opens, can't be interated with
    }
    [Header("Start State")]
    public ChestState chestState;
    
    [Space(10)]

    [Header("Shake Anim settiongs")]
    [SerializeField]
    private GameObject ChestPivotObject;
    public float ShakeDuration = 2;
    public float MaxRange = 1;
    [Space(5)]
    public AnimationCurve Curve_SideMovement;
    public AnimationCurve Curve_Jump;
    [SerializeField]
    private AudioSource ShakeSound;

    [Space(10)]

    private Vector3 StartPos;
    private Vector3 TargetPos;

    [Header("Open Anim settiongs")]
   

    [SerializeField]
    private GameObject LidPivotObject;

    public float OpeningDuration = 2;

    [SerializeField]
    private AudioSource OpenSound;


    private void Start()
    {
        setChestState(chestState);
    }
    public void setChestState(ChestState newState)
    {
        //if (chestState != newState) return;//must be a diffrent state
        chestState = newState;

        switch (chestState) // Apply New State
        {
            case ChestState.Idle:
                // code block
                break;
            case ChestState.Shaking:
                StartCoroutine(ShakeAnimation(new Vector2(1, 0)));
                break;
            case ChestState.Open:
                StartCoroutine(OpenChestAnim());
                break;
            default:
                // code block
                break;
        }
    }

    IEnumerator ShakeAnimation(Vector2 TargetDir)
    {
        ShakeSound.Play();

        float CurrentTime = 0.0f;
        Vector2 LerpVecor = GetOpposite(TargetDir);//get a new Dir in the general opposit diraction of the 
        TargetPos = new Vector3(LerpVecor.x * MaxRange, 0, LerpVecor.y * MaxRange);
        

        while (CurrentTime < ShakeDuration)
        {
            CurrentTime = Mathf.Clamp(CurrentTime + Time.deltaTime, 0, ShakeDuration);
            ChestPivotObject.transform.localPosition = Vector3.Lerp(StartPos, TargetPos, Curve_SideMovement.Evaluate(CurrentTime / ShakeDuration));
            ChestPivotObject.transform.localPosition = new Vector3(ChestPivotObject.transform.localPosition.x, Curve_Jump.Evaluate(CurrentTime / ShakeDuration) * 0.05f, ChestPivotObject.transform.localPosition.y);
            ChestPivotObject.transform.localRotation = Quaternion.Euler(LerpVecor.x * Curve_Jump.Evaluate(CurrentTime / ShakeDuration) * -8, 0, LerpVecor.y * Curve_Jump.Evaluate(CurrentTime / ShakeDuration) * -5);
            yield return null;
        }
        StartPos = TargetPos;
        

        yield return new WaitForSeconds(Random.Range(0.3f, 0.6f));
        if (chestState == ChestState.Shaking)
        {
            StartCoroutine(ShakeAnimation(LerpVecor));
        }
    }

    Vector2 GetOpposite(Vector2 lastVector)
    {
        //Generates a vector in the opposite direction

        Vector2 NewVector = new Vector2(0, 0);
        for (int i = 0; i < 10; i++)//Has max 10 attempts to get a good enough Vector
        {
            NewVector = Random.insideUnitCircle;
            if (Vector3.Dot(Vector3.Normalize(lastVector), Vector3.Normalize(NewVector)) <= -0.5)
            {
                break;
            }
        }

        return Vector3.Normalize(NewVector);
    }

    IEnumerator OpenChestAnim()
    {       
        float CurrentTime = 0.0f;

        while (CurrentTime < OpeningDuration)
        {
            CurrentTime = Mathf.Clamp(CurrentTime + Time.deltaTime, 0, OpeningDuration);
            LidPivotObject.transform.localRotation = Quaternion.Euler(Curve_SideMovement.Evaluate(CurrentTime / OpeningDuration) * -140, 0, 0);
            yield return null; 
        }
        OpenSound.Play();
    }
}
