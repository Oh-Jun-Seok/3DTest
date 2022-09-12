using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    public GameObject cam;
    public GameObject startingPoint;
    public GameObject panel;

    GameObject playerClone;
    private void Start()
    {
        playerClone = Instantiate(player);
        playerClone.transform.position = startingPoint.transform.position;
        playerClone.transform.rotation = startingPoint.transform.rotation;
        cam.GetComponent<CameraMove>().follow = playerClone.transform.Find("CameraFollow").gameObject.transform;
        playerClone.GetComponent<Player>().cam = cam;
        playerClone.GetComponent<Player>().manager = this;

        panel.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    public void PlayerDie()
    {
        Destroy(playerClone);
        Time.timeScale = 0;
        panel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void QuitGame()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }
}
