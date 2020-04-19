using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class intro : MonoBehaviour
{
    public GameObject mainCam;
    bool moveCam = false;
    public int moveSpeed;
    bool menuOpened = false;
    public GameObject mainMenu;

    void Start()
    {
        StartCoroutine(StartIntro());
    }

    void Update()
    {
        if (moveCam) mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, this.transform.position, moveSpeed * Time.deltaTime);
        if (!menuOpened && mainCam.transform.position == this.transform.position) OpenMenu();
    }

    private IEnumerator StartIntro()
    {
        yield return new WaitForSeconds(1f);
        moveCam = true;
    }
    void OpenMenu()
    {
        menuOpened = true;
        mainMenu.SetActive(true);
        LeanTween.alphaCanvas(mainMenu.GetComponent<CanvasGroup>(), 1f, 1f).setEase(LeanTweenType.easeInCirc);
    }
    public void StartGame()
    {
        SceneManager.LoadScene("Prototype");

    }


}
