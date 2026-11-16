using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using NaveExploracion.Data;

public class CollectibleRock : MonoBehaviour
{
    [Header("Mission Save Hook")]
    public string missionId = MissionDatabase.MISSION_01;
    public string returnSceneName = "Base_Scene";
    public float delayBeforeReturn = 2.5f;

    [Header("Settings")]
    public string playerTag = "Player";
    public bool hideOnCollect = true;

    [Header("Victory UI (Optional)")]
    [Tooltip("Drag a Canvas Panel or Text here if you want to display it on screen")]
    public GameObject victoryMessageUI;

    [Header("Extra Actions (Events)")]
    [Tooltip("Connect sounds, particles, or scene changes here from the Inspector")]
    public UnityEvent onRockCollected;

    private bool isCollected = false;

    private void OnTriggerEnter(Collider other)
    {
        if (isCollected || !other.CompareTag(playerTag)) return;

        isCollected = true;

        MissionSaveManager.MarkMissionCompleted(missionId);

        if (victoryMessageUI != null)
        {
            victoryMessageUI.SetActive(true);
        }

        onRockCollected?.Invoke();

        if (hideOnCollect)
        {
            foreach (var r in GetComponentsInChildren<Renderer>()) r.enabled = false;
            foreach (var c in GetComponentsInChildren<Collider>()) c.enabled = false;
        }

        StartCoroutine(ReturnToBaseRoutine());
    }

    private IEnumerator ReturnToBaseRoutine()
    {
        yield return new WaitForSecondsRealtime(delayBeforeReturn);
        Time.timeScale = 1f;
        SceneManager.LoadScene(returnSceneName);
    }

    public void LoadSceneByName(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}