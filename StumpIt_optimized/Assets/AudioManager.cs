using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class AudioManager : MonoBehaviour
{
    public Toggle audioToggle;
    public AudioClip[] playCommentery; // Array to hold your sounds
    public AudioClip[] Misssound;
    public AudioClip stumpSound;
    public AudioClip crowdSound;

    public AudioSource audioSource1; // AudioSource component
    public AudioSource audioSource2;
    public AudioSource audioSource3;
    public AudioSource audioSource4;

    void Start()
    {
        // Get the AudioSource component
        audioToggle.onValueChanged.AddListener(OnToggleValueChanged);
    }

    public void Init()
    {
        PlayCrowedLoopSound();
       // audioSource = GetComponent<AudioSource>();
    }
    public void PlayRandomRunOutSound()
    {
        if (playCommentery.Length > 0) // Check if there are any sounds to play
        {
            UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
            int randomIndex = Random.Range(0, playCommentery.Length); // Select a random sound
            audioSource1.clip = playCommentery[randomIndex]; // Set the AudioSource clip to the selected sound
            audioSource1.Play(); // Play the sound
        }
        audioSource2.PlayOneShot(stumpSound);
    }
    public void PlayMissSound()
    {
        if (Misssound.Length > 0) // Check if there are any sounds to play
        {
            UnityEngine.Random.seed = System.DateTime.Now.Millisecond;
            int randomIndex = Random.Range(0, Misssound.Length); // Select a random sound
            audioSource3.clip = Misssound[randomIndex]; // Set the AudioSource clip to the selected sound
            audioSource3.Play(); // Play the sound
        }
    }

    public void OnToggleValueChanged(bool isOn)
    {
        // Handle the toggle state change
        if (isOn)
        {
            Debug.Log("Toggle is On");
            audioSource1.mute = true;
            audioSource2.mute = true;
            audioSource3.mute = true;
            audioSource4.mute = true; 
            audioToggle.transform.GetChild(0).GetComponent<Image>().enabled = false;
            audioToggle.transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
        }
        else
        {
            Debug.Log("Toggle is Off");
            audioSource1.mute = false;
            audioSource2.mute = false;
            audioSource3.mute = false;
            audioSource4.mute = false;
            audioToggle.transform.GetChild(0).GetComponent<Image>().enabled = true;
            audioToggle.transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
        }
    }

    public void PlayCrowedLoopSound()
    {
        //audioSource4.loop = true;
        audioSource4.Play();
    }
    public void StopCrowedSound() 
    {
        //audioSource4.loop = false;
        audioSource1.mute = true;
        audioSource2.mute = true;
        audioSource3.mute = true;
        audioSource4.mute = true;
    }
    void OnDestroy()
    {
        if (audioToggle != null)
        {
            audioToggle.onValueChanged.RemoveListener(OnToggleValueChanged);
        }
    }
}


