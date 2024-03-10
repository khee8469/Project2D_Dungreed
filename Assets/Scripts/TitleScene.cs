using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class TitleScene : BaseScene
{
    public void GameSceneLoad()
    {
        Manager.Scene.LoadScene("MainGame");
    }

    public override IEnumerator LoadingRoutine()
    {
        yield return null;
    }
}
