using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    private Image fadeImage;
    private Animator fadeAnimator;

    public GameObject gameOverPanel;
    public GameObject winPanel;
    

    void Awake()
    {
        BaseController.OnBaseDestroyed += HandleBaseDestroyed;
    }

    private void HandleBaseDestroyed(string team)
    {
        Debug.Log($"Base destroyed for team: {team}");
        switch (team)
        {
            case "Lwoas":
                Debug.Log("Lwoas team base destroyed. Game over for Lwoas.");
                LooseGame();
                break;
            case "Cogu":
                Debug.Log("Cogu team base destroyed. Game over for Cogu.");
                WinGame();
                break;
            default:
                Debug.Log("Unknown team base destroyed.");
                break;
        }



    }


    private void WinGame()
    {
        StartCoroutine(WinSequence());
    }

    private void LooseGame()
    {
        StartCoroutine(LooseSequence());
    }

    IEnumerator WinSequence()
    {

        yield return new WaitForSeconds(2f);
        winPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0); // Volta para a cena do menu
        
    }
    
    IEnumerator LooseSequence()
    {
        yield return new WaitForSeconds(2f);
        gameOverPanel.SetActive(true);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(0); // Volta para a cena do menu
    }

}
