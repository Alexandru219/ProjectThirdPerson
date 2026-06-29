using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }
    
    [SerializeField] private string emptySceneName = "EmptyScene";

    [SerializeField] private float minimumLoadTime = 0.5f;

    [SerializeField] private CanvasGroup fadeCanvasGroup;
    [SerializeField] private float fadeDuration = 0.3f;

    private string _targetSceneName;
    private bool   _isLoading;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadScene(string sceneName)
    {
        if (_isLoading)
        {
            Debug.LogWarning("[SceneLoader] Incarcare in curs, cererea a fost ignorata.");
            return;
        }
        _targetSceneName = sceneName;
        StartCoroutine(TransitionRoutine());
    }

    public void LoadScene(int sceneIndex)
    {
        LoadScene(SceneManager.GetSceneByBuildIndex(sceneIndex).name);
    }

    public void ReloadCurrentScene()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator TransitionRoutine()
    {
        _isLoading = true;

        if (fadeCanvasGroup != null)
            yield return StartCoroutine(Fade(0f, 1f));

        yield return SceneManager.LoadSceneAsync(emptySceneName);
        Debug.Log($"[SceneLoader] Acum in scena tampon: {emptySceneName}");

        float timer = 0f;

        AsyncOperation loadOp = SceneManager.LoadSceneAsync(_targetSceneName);
        loadOp.allowSceneActivation = false;  

        while (!loadOp.isDone)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(loadOp.progress / 0.9f); // 0..1
            Debug.Log($"[SceneLoader] Progress: {progress * 100:F0}%");

            if (loadOp.progress >= 0.9f && timer >= minimumLoadTime)
            {
                loadOp.allowSceneActivation = true; // Activeaza scena
            }

            yield return null;
        }

        Debug.Log($"[SceneLoader] Scene: {_targetSceneName}");

        if (fadeCanvasGroup != null)
            yield return StartCoroutine(Fade(1f, 0f));

        _isLoading = false;
    }

    private IEnumerator Fade(float from, float to)
    {
        float elapsed = 0f;
        fadeCanvasGroup.alpha = from;
        fadeCanvasGroup.blocksRaycasts = true;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / fadeDuration);
            yield return null;
        }

        fadeCanvasGroup.alpha = to;
        fadeCanvasGroup.blocksRaycasts = (to > 0.5f);
    }
}
