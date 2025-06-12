using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    [Header("UI Panels")]
    [SerializeField] private GameObject menuPannel;
    [SerializeField] private GameObject instructionsPannel;


    void Start()
    {
        menuPannel.SetActive(true);
        instructionsPannel.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ToggleMenuPanel()
    {
        menuPannel.SetActive(true);
        instructionsPannel.SetActive(false);
    }

    public void ToggleInstructionsPanel()
    {
        menuPannel.SetActive(false);
        instructionsPannel.SetActive(true);
    }
}

