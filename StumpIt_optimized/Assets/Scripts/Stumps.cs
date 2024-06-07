using System.Collections;
using UnityEngine;

public class Stumps : MonoBehaviour
{
    Rigidbody rb;
    public float forceMultiplier = 3.0f;
    public Stump stump;
    public bool down;
    Material material; // Assign the material with emission in the Inspector
    Coroutine colorBlink;

    private void Start()
    {
        rb=GetComponent<Rigidbody>();
        down = false;
        GetComponentInChildren<TrailRenderer>().enabled = false;
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }
       
    }
    void OnCollisionEnter(Collision collision)
    {
        
        if ((collision.gameObject.CompareTag("ball") || collision.gameObject.CompareTag("Stump")) && down==false)
        {
            rb.isKinematic = false;
            ApplyForceToStump(collision);
        }

        //StartCoroutine(ResetStumpsAndBails());
    }
    private void ApplyForceToStump(Collision collision)
    {
        GetComponentInChildren<TrailRenderer>().enabled = true;
        GameManager.Instance.stumpsDown += 1;
        if(GameManager.Instance.stumpsDown == 1&& stump==Stump.Red)
        {
            GameManager.Instance.firstHitRed = true;
            GameManager.Instance.scoreManager.IncreaseScore(stump);
        }
        else if(stump==Stump.Red)
        {

            colorBlink = StartCoroutine(BlinckRed());
            GameManager.Instance.scoreManager.IncreaseScore(stump);
            
        }
        else if(stump==Stump.Blue)
        {
            GameManager.Instance.scoreManager.IncreaseScore(stump);
        }

        float impactForce = collision.rigidbody.velocity.magnitude;
        Vector3 direction = this.transform.position - collision.contacts[0].point;
        direction.Normalize();// = -direction.normalized;

        // Adjust this value as needed
        float forceMagnitude = impactForce * forceMultiplier; // increase force magnitude
        rb.AddForce(direction * forceMultiplier, ForceMode.Impulse);
        down = true;
    }
    private IEnumerator BlinckRed()
    {

        material.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(0.1f);
        material.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(0.1f);
        material.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(0.1f);
        material.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(0.1f);
        material.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(0.1f);
        material.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(0.1f);
        material.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(0.1f);
        material.SetColor("_EmissionColor", Color.red);
        yield return new WaitForSeconds(0.1f);
        material.SetColor("_EmissionColor", Color.white);
        yield return new WaitForSeconds(0.1f);
        material.SetColor("_EmissionColor", Color.red);
        StopCoroutine(BlinckRed());


    }
}
public enum Stump
{
    Red,
    Blue,
    Bail
}
