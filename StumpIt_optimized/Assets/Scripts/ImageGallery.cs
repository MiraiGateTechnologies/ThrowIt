using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ImageGallery : MonoBehaviour
{
    public GameObject galleryPopup;
    public Button addProfilePicButton;
    public Button removeProfilePicButton;
    public Image[] images; // Array of all images in the gallery
    public Color highlightColor; // Color to highlight the selected image
    public Image profileImage;
    public TMP_InputField inputField;
    public TMP_Text validationText;
    public Button playButton;
    private Image selectedImage; // Currently selected image
    float clicked = 0;
    float clicktime = 0;
    float clickdelay = 0.5f;
    void Start()
    {
        // Attach double click listener to all images
        foreach (Image image in images)
        {
            AddDoubleClickEvent(image);
        }
        addProfilePicButton.onClick.AddListener(()=> galleryPopup.SetActive(true));
        removeProfilePicButton.onClick.AddListener(() => 
        { 
            addProfilePicButton.gameObject.SetActive(true);
            removeProfilePicButton.gameObject.SetActive(false);
        
        });
        // Add a listener to the input field's onValueChanged event
        inputField.onValueChanged.AddListener(ValidateInput);

        // Add a listener to the play button's onClick event
        playButton.onClick.AddListener(OnPlayButtonClick);
    }

    void ValidateInput(string input)
    {
        // Your validation logic goes here
        if (string.IsNullOrEmpty(input))
        {
            // Input is empty or null
            validationText.text = "Please enter name...";
            playButton.interactable = false; // Disable the play button
        }
        else
        {
            // Input is not empty, clear validation text
            validationText.text = "";
            playButton.interactable = true; // Enable the play button
        }
    }

    void OnPlayButtonClick()
    {
        string inputValue = inputField.text;
        Debug.Log("Input value: " + inputValue);
    }


    void AddDoubleClickEvent(Image image)
    {
        EventTrigger trigger = image.gameObject.AddComponent<EventTrigger>();

        // Create a pointer event for double click
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClick((PointerEventData)data); });

        // Add the event to the trigger
        trigger.triggers.Add(entry);
    }

    void OnPointerClick(PointerEventData eventData)
    {
        Image clickedImage = eventData.pointerPress.GetComponent<Image>();

        // Deselect previously selected image
        if (selectedImage != null)
        {
            selectedImage.color = Color.white; // Reset color
        }

        // Select the clicked image
        selectedImage = clickedImage;
        selectedImage.color = highlightColor; // Highlight the selected image
        clicked++;

        if (clicked == 1)
            clicktime = Time.time;

        if (clicked > 1 && Time.time - clicktime < clickdelay)
        {
            // Double click detected
            clicked = 0;
            clicktime = 0;
            Debug.Log("Double Click: " + this.GetComponent<RectTransform>().name);
            galleryPopup.SetActive(false);
            addProfilePicButton.gameObject.SetActive(false);
            profileImage.sprite = clickedImage.sprite;
        }
        else if (clicked > 2 || Time.time - clicktime > 1)
            clicked = 0;
       
    }
}
