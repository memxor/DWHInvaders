using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameUIManager : MonoBehaviour
{
    public GameObject winPanel;
    public GameObject losePanel;

    public void OnWin()
    {
        winPanel.SetActive(true);
    }

    public void OnLose()
    {
        losePanel.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void TweetWin()
    {
        Application.OpenURL("https://twitter.com/intent/tweet?text=I%20get%20to%20keep%20my%20hat%20in%20DevWifHat%20Invaders!%20%23HatStaysOn%0A%0A@thedevwifhat%20@memxor_%20@ankj90%0A%0ATry%20it%20yourself%20at%20-%20https%3A//memxor.itch.io/dwh-invaders");
    }
}
