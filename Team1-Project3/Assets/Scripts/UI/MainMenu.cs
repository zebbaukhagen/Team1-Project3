using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private List<GameObject> listOfPanels = new();
    /// <summary>
    /// 0 = Main
    /// 1 = Instructions 
    /// 2 = Settings
    /// 3 = Credits
    /// 4 = Level select
    /// </summary>
    /// 

    [SerializeField] private List<AudioClip> listOfAudioClips = new();

    [SerializeField] private GameObject sceneLoaderPrefab;
    private SceneLoader sceneLoader;

    private int currentPanel = 0;

    private void Start()
    {
        Debug.Log("Start method called.");
        if (!GameObject.Find("SceneLoader"))
        {
            Debug.Log("Instantiating scene loader.");
            var gameObject = Instantiate(sceneLoaderPrefab);
            gameObject.name = "SceneLoader";
            sceneLoader = SceneLoader.instance;
        }

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void OnClickPlayButton()
    {
        sceneLoader.LoadLevel(1);
        audioSource.PlayOneShot(listOfAudioClips[3]);
    }
    public void OnClickInstructionsButton()
    {
        listOfPanels[currentPanel].SetActive(false);
        listOfPanels[1].SetActive(true);
        currentPanel = 1;
    }
    public void OnClickSettingsButton()
    {
        listOfPanels[currentPanel].SetActive(false);
        listOfPanels[2].SetActive(true);
        currentPanel = 2;
    }

    public void OnClickCreditsButton()
    {
        listOfPanels[currentPanel].SetActive(false);
        listOfPanels[3].SetActive(true);
        currentPanel = 3;
    }
    public void OnClickLevelSelectButton()
    {
        listOfPanels[currentPanel].SetActive(false);
        listOfPanels[4].SetActive(true);
        currentPanel = 4;
    }

    public void OnClickBackButton()
    {
        listOfPanels[currentPanel].SetActive(false);
        listOfPanels[0].SetActive(true);
        currentPanel = 0;
        audioSource.PlayOneShot(listOfAudioClips[2]);
    }

    public void OnClickQuitButton()
    {
        Application.Quit();
    }

    public void OnButtonHover()
    {
        audioSource.PlayOneShot(listOfAudioClips[1]);
    }

    public void ButtonClickNoise()
    {
        audioSource.PlayOneShot(listOfAudioClips[0]);
    }
}
