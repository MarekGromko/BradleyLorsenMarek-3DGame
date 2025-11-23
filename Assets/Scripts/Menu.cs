
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private Scene levelScene;

    void Start()
    {
        SceneManager.LoadSceneAsync("Level_1", LoadSceneMode.Additive).completed += op =>
        {
            levelScene = SceneManager.GetSceneByName("Level_1");

            UnityEngine.Debug.Log(" jeu charger");

        };
    }

    public void OnPlayButton()
    {
        if (!levelScene.IsValid() || !levelScene.isLoaded)
        {
            UnityEngine.Debug.LogError("Level_1 charment pas fini");

            return;
        }

        GameObject gameplayroot = null;
        foreach (var root in levelScene.GetRootGameObjects())
        {
            if (root.name == "GameRoot")
            {gameplayroot = root;
                break;
            }
        }

        if (gameplayroot == null)
        {
            UnityEngine.Debug.LogError("GameRoot pas present dans Level_1 !");

            return;
        }
        gameplayroot.SetActive(true);

        Transform player = gameplayroot.transform.Find("Player");
        if (player != null)
        {
            player.gameObject.SetActive(true);

            Camera playerCam = player.GetComponentInChildren<Camera>();
            if (playerCam != null) playerCam.enabled = true;

            AudioListener playerAL = playerCam?.GetComponent<AudioListener>();
            if (playerAL != null) playerAL.enabled = true;
        }

        Camera previewCam = GameObject.Find("PreviewCamera")?.GetComponent<Camera>();
        if (previewCam != null) previewCam.enabled = false;

        AudioListener previewAL = previewCam?.GetComponent<AudioListener>();
        if (previewAL != null) previewAL.enabled = false;

        SceneManager.UnloadSceneAsync("GameStart");
    }
    public void OnQuitButton() { 
        Application.Quit();
    }

}
