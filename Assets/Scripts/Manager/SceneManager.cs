using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

public class SceneManager : Singleton<SceneManager>
{
    [SerializeField] Image fade;
    [SerializeField] Slider loadingBar;
    [SerializeField] float fadeTime;

    private BaseScene curScene;

    public BaseScene GetCurScene()
    {
        if (curScene == null)
        {
            curScene = FindObjectOfType<BaseScene>();
        }
        return curScene;
    }

    public T GetCurScene<T>() where T : BaseScene
    {
        if (curScene == null)
        {
            curScene = FindObjectOfType<BaseScene>();
        }
        return curScene as T;
    }

   
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadingRoutine(sceneName));
    }

    IEnumerator LoadingRoutine(string sceneName)
    {
        //페이드아웃
        fade.gameObject.SetActive(true);
        yield return FadeOut();
        //청소
        Manager.Pool.ClearPool();
        Manager.Sound.StopSFX();
        Manager.UI.ClearPopUpUI();
        Manager.UI.ClearWindowUI();
        Manager.UI.CloseInGameUI();

        //시간 정지
        Time.timeScale = 0f;
        //로딩바
        loadingBar.gameObject.SetActive(true);
        //비동기 씬로드
        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
        //씬이 로드될떄까지대기
        while (oper.isDone == false)
        {
            loadingBar.value = oper.progress;
            yield return null;
        }
        //EventSystem 없으면 생성
        Manager.UI.EnsureEventSystem();
        //BaseScene = 현재씬
        BaseScene curScene = GetCurScene();
        yield return curScene.LoadingRoutine();

        loadingBar.gameObject.SetActive(false);
        Time.timeScale = 1f;

        yield return FadeIn();
        fade.gameObject.SetActive(false);
    }


    //페이드 인아웃 코르틴
    IEnumerator FadeOut()
    {
        float rate = 0;
        Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

        while (rate <= 1)
        {
            rate += Time.unscaledDeltaTime / fadeTime;
            fade.color = Color.Lerp(fadeInColor, fadeOutColor, rate);
            yield return null;
        }
    }
    IEnumerator FadeIn()
    {
        float rate = 0;
        Color fadeOutColor = new Color(fade.color.r, fade.color.g, fade.color.b, 1f);
        Color fadeInColor = new Color(fade.color.r, fade.color.g, fade.color.b, 0f);

        while (rate <= 1)
        {
            rate += Time.unscaledDeltaTime / fadeTime;
            fade.color = Color.Lerp(fadeOutColor, fadeInColor, rate);
            yield return null;
        }
    }
}
