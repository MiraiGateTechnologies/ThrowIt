using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StumpBailManager : MonoBehaviour
{
    public GameObject[] stumps;
    public GameObject[] bails;
    private GameObject ball;

    private List<Vector3> originalPositionsStumps = new List<Vector3>();
    private List<Vector3> originalPositionsBails = new List<Vector3>();
    private Dictionary<GameObject, Quaternion> originalRotations = new Dictionary<GameObject, Quaternion>();
    private Vector3 BallOriginalPosition;

    void Start()
    {
        //Ball=FindObjectOfType<SwipeScript>().gameObject;
        // Store original positions and rotations of stumps
        
    }
    public void Init()
    {
        foreach (var stump in stumps)
        {
            originalPositionsStumps.Add(stump.transform.position);
            originalRotations[stump] = stump.transform.rotation;
        }

        // Store original positions and rotations of bails
        foreach (var bail in bails)
        {
            originalPositionsBails.Add(bail.transform.position);
            originalRotations[bail] = bail.transform.rotation;
        }

        ShuffleList(originalPositionsStumps);
    }
    public IEnumerator ResetStumpsAndBails()
    {

        yield return new WaitForSeconds(1f);

        int i = 0;
        foreach (var stump in stumps)
        {
            stump.GetComponentInChildren<TrailRenderer>().enabled = false;
            Rigidbody rb = stump.GetComponent<Rigidbody>();
            stump.GetComponent<Stumps>().down = false;
            rb.isKinematic = true;
            stump.transform.position = originalPositionsStumps[i];
            stump.transform.rotation = originalRotations[stump];
            i++;
        }
        i = 0;
        foreach (var bail in bails)
        {
            Rigidbody rb = bail.GetComponent<Rigidbody>();
            rb.isKinematic = true;
            bail.transform.position = originalPositionsBails[i];
            bail.transform.rotation = originalRotations[bail];
            i++;
        }
        ShuffleList(originalPositionsStumps);
    }

    // Fisher-Yates shuffle algorithm for lists
    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            T temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }

    public void StumpDownManager()
    {

    }
}
