using UnityEngine;

public class LobbyButton : MonoBehaviour
{
    public void StartButton()
    {
        LoadingManager.LoadScene(SceneNames.Main);
    }
}
