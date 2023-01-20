using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour
{
    [SerializeField] private string mainGameScene = "";

    [SerializeField] private GameObject instructions;
    public void Play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void Instructions()
    {
        instructions.SetActive(true);
    }

    public void DisableInstructions()
    {
        instructions.SetActive(false);
    }
}
