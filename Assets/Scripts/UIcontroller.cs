using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIcontroller : MonoBehaviour
{
    public Text BonusPoint;
    public GameObject PauseMenu;
    public GameObject DeathMenu;
    private int _point = 0;
 
    public static UIcontroller instanse;

    private void Awake()
    {
        if (UIcontroller.instanse != null)
        {
            Destroy(gameObject);
            return;
        }
        UIcontroller.instanse = this;
        if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ShowHidePauseMenu();
        }
    }
    public void AddBonus(int point)
    {
        _point += point;
        if (_point <= 9999)
        {
            BonusPoint.text = $"{_point:000#}";
        }
        else
        {
            BonusPoint.text = $"{_point:0000#}";
        }
    }
    public void SubtractBonus(int point)
    {
        _point -= point;
        if (_point <= 9999)
        {
            BonusPoint.text = $"{_point:000#}";
        }
        else
        {
            BonusPoint.text = $"{_point:0000#}";
        }
    }
    public int GetPoint() 
    {
        return _point;
    }
    public void ShowHidePauseMenu()
    {
        if (!PauseMenu.activeSelf)
        {
            PauseMenu.SetActive(true);
            PlayerMover.instanse.is_paused = true;
            Time.timeScale = 0;
        }
        else
        {
            PauseMenu.SetActive(false);
            PlayerMover.instanse.is_paused = false;
            Time.timeScale = 1;
        }
    }
    public void ShowDeathMenu()
    {
        DeathMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Restart() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void Exit()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
