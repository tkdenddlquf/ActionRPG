using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    private void Awake() { _instance = this; }

    public Camera Cam { get; private set; }

    public CharManager character;
    public CinemachineOrbitalFollow cineOrbit;
    public CinemachineCamera cineCam;

    [Header("PANEL")]
    public CharPanel charPanel;
    public BossPanel bossPanel;

    private int slowTime;

    private void Start()
    {
        Cam = Camera.main;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SetTimeScale(float _scale = 0.1f, int _time = 50)
    {
        if (Time.timeScale == _scale) return;

        Time.timeScale = _scale;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

        if (slowTime <= 0)
        {
            slowTime = _time;

            StartCoroutine(SlowMotion());
        }
        else slowTime = _time;
    }

    private IEnumerator SlowMotion()
    {
        while (true)
        {
            if (slowTime <= 0)
            {
                slowTime = 0;

                SetTimeScale(1);

                break;
            }
            else slowTime--;

            yield return null;
        }
    }
}
