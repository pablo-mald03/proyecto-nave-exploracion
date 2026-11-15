using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement; // Required for changing scenes later

public class CollectibleRock : MonoBehaviour
{
    [Header("Settings")]
    public string playerTag = "Player";
    public bool destroyOnCollect = true;

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

        Debug.Log("You win! You found the special rock.");

        if (victoryMessageUI != null)
        {
            victoryMessageUI.SetActive(true);
        }

        onRockCollected?.Invoke();

        if (destroyOnCollect)
        {
            Destroy(gameObject);
        }
    }

    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}