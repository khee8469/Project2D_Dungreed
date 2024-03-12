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
        //���̵�ƿ�
        fade.gameObject.SetActive(true);
        yield return FadeOut();
        //û��
        Manager.Pool.ClearPool();
        Manager.Sound.StopSFX();
        Manager.UI.ClearPopUpUI();
        Manager.UI.ClearWindowUI();
        Manager.UI.CloseInGameUI();

        //�ð� ����
        Time.timeScale = 0f;
        //�ε���
        loadingBar.gameObject.SetActive(true);
        //�񵿱� ���ε�
        AsyncOperation oper = UnitySceneManager.LoadSceneAsync(sceneName);
        //���� �ε�ɋ��������
        while (oper.isDone == false)
        {
            loadingBar.value = oper.progress;
            yield return null;
        }
        //EventSystem ������ ����
        Manager.UI.EnsureEventSystem();
        //BaseScene = �����
        BaseScene curScene = GetCurScene();
        yield return curScene.LoadingRoutine();

        loadingBar.gameObject.SetActive(false);
        Time.timeScale = 1f;

        yield return FadeIn();
        fade.gameObject.SetActive(false);
    }


    //���̵� �ξƿ� �ڸ�ƾ
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
