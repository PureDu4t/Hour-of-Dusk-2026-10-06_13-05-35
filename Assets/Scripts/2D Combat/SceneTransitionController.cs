using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionController : MonoBehaviour
{
    public static SceneTransitionController Instance;
    [SerializeField] private AudioManager _audioManager;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            _audioManager.PlayUIStinger();
            SceneManager.LoadScene("AttackPrototype");
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SceneManager.GetActiveScene().name == "MainTitle")
            {
                Application.Quit();
            }

            else
            {
                _audioManager.PlayUIStinger();
                SceneManager.LoadScene("MainTitle");
            }
        }
    }
}