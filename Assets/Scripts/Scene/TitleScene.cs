using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TitleScene : MonoBehaviour
{
    public void NewGame()
    {
        if(GameInProgress.CheckAll().Count == 0)
        {
            Debug.Log("no game");
            GameInProgress.curSlot = 1;

            Dungeon.hero = null;
            InterLevelScene.mode = InterLevelScene.Mode.DESCEND;
            LoadingSceneController.LoadScene(2);
        }
        else
        {
            Debug.Log("exist game");
            SceneManager.LoadScene(1);
        }
        
    }
}
