using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    private void Awake()
    {
        Cursor.visible = false;
    }

    private void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        StartCoroutine(WaitForCutsceneToEnd((float)videoPlayer.clip.length));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("MainIsland");
        }
    }

    private IEnumerator WaitForCutsceneToEnd(float duration)
    {
        yield return new WaitForSeconds(duration);
        SceneManager.LoadScene("MainIsland");
    }
}
