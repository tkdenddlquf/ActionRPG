using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    private static SceneNames sceneNames;

    private AsyncOperation asyncOperation;

    private readonly LerpSlider percent = new();
    private readonly LerpUIAction lerpAction = new();

    private void Start()
    {
        percent.callback = Callback;
        percent.slider = FindAnyObjectByType<Slider>();
        percent.speed = 0.1f;

        StartCoroutine(LoadSceneProcess());
    }

    private void FixedUpdate()
    {
        lerpAction.actions?.Invoke();
    }

    public static void LoadScene(SceneNames _name)
    {
        SceneManager.LoadScene((int)SceneNames.Loading);
        sceneNames = _name;
    }

    private IEnumerator LoadSceneProcess()
    {
        asyncOperation = SceneManager.LoadSceneAsync((int)sceneNames);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            percent.SetData(lerpAction, asyncOperation.progress);

            yield return null;
        }
    }

    private void Callback(float _value)
    {
        if (_value == 0.9f) asyncOperation.allowSceneActivation = true;
    }
}

public enum SceneNames
{
    Lobby,
    Loading,
    Main
}
