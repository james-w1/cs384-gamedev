using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScript : MonoBehaviour
{

    [SerializeField] Image loadingBar;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadScene());
	loadingBar.fillAmount = 0;
    }

    void StartLoading()
    {
	StartCoroutine(LoadScene());
    }

    IEnumerator LoadScene()
    {
	yield return null;

	AsyncOperation asyncOp = SceneManager.LoadSceneAsync("GameScene");

	while (!asyncOp.isDone)
	{
		loadingBar.fillAmount = asyncOp.progress;
		yield return new WaitForEndOfFrame();
	}

	asyncOp.allowSceneActivation = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
