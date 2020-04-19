using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    public GameObject gameMenu;
    public GameObject failState;
    public GameObject winState;
    public CameraMovement cameraMovement;
    bool isOpen = false;
    public GameObject escapePod;
    public GameObject mainCam;
    public GameObject podDestination;
    public GameObject podEffects;
    public GameObject topCanvas;
    public GameObject deckCanvas;
    bool podLaunched;
    public GameObject helpMenu;
    public AudioSource audioSource;
    public AudioSource spaceShipAudioSource;
    public AudioClip spaceship;
    public AudioClip win;

    void Awake()
    {
        audioSource.clip = win;
        spaceShipAudioSource.clip = spaceship;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isOpen)
            {
                gameMenu.SetActive(true);
                isOpen = true;

            }
            else
            {
                gameMenu.SetActive(false);
                isOpen = false;
            }

        }
        if (podLaunched)
        {
            mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, new Vector3(escapePod.transform.position.x, mainCam.transform.position.y, escapePod.transform.position.z - 6), 40 * Time.deltaTime);
            escapePod.transform.position = Vector3.MoveTowards(escapePod.transform.position, podDestination.transform.position, 5 * Time.deltaTime);
            podEffects.SetActive(true);
        }
    }
    public void OpenMenu()
    {
        gameMenu.SetActive(true);
        isOpen = true;
    }
    public void Continue()
    {
        gameMenu.SetActive(false);
        isOpen = false;
    }

    public void RestartGame()
    {
        Application.LoadLevel(Application.loadedLevel);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void FailState()
    {
        cameraMovement.cameraMovementEnabled = false;
        failState.SetActive(true);
        LeanTween.alphaCanvas(failState.GetComponent<CanvasGroup>(), 1f, 2f).setEase(LeanTweenType.easeInCirc);
    }
    public void WinState()
    {
        podLaunched = true;
        topCanvas.SetActive(false);
        deckCanvas.SetActive(false);
        StartCoroutine(ShowEndScreen());

    }
    private IEnumerator ShowEndScreen()
    {
        spaceShipAudioSource.Play();
        yield return new WaitForSeconds(4f);
        audioSource.Play();
        cameraMovement.cameraMovementEnabled = false;
        winState.SetActive(true);
        LeanTween.alphaCanvas(winState.GetComponent<CanvasGroup>(), 1f, 2f).setEase(LeanTweenType.easeInCirc);
    }
    public void OpenHelpMenu()
    {
        helpMenu.SetActive(true);
    }
    public void CloseHelpMenu()
    {
        helpMenu.SetActive(false);
    }
}
