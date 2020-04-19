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
        cameraMovement.cameraMovementEnabled = false;
        winState.SetActive(true);
        LeanTween.alphaCanvas(winState.GetComponent<CanvasGroup>(), 1f, 2f).setEase(LeanTweenType.easeInCirc);
    }
}
