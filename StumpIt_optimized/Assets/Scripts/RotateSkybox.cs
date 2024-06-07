using UnityEngine;

public class RotateSkybox : MonoBehaviour
{
    public float rotationSpeed = 1.0f;

    void Update()
    {
        float rotationAmount = rotationSpeed * Time.deltaTime;
        RenderSettings.skybox.SetFloat("_Rotation", RenderSettings.skybox.GetFloat("_Rotation") + rotationAmount);
    }
}
