using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenUI : MonoBehaviour
{
    public string Game;
    [SerializeField] private GameObject instructions;

    private void Start()
    {

    }

    public void Play()
    {
        SceneManager.LoadScene(Game);
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
