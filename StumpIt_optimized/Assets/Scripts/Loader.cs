using UnityEngine;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    public float speed = 200f; // Rotation speed
    private RectTransform rectComponent;

    void Start()
    {
        rectComponent = GetComponent<RectTransform>();
    }

    void Update()
    {
        rectComponent.Rotate(0f, 0f, speed * Time.deltaTime);
    }
}
