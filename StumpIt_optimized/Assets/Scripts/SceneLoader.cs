using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{
    public void ResetScene()
    {
        SceneManager.LoadScene(0);
    }
    public void startGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
