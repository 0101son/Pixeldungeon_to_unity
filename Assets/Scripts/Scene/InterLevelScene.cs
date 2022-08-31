using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class InterLevelScene : MonoBehaviour
{

    public enum Mode
    {
        DESCEND, ASCEND, CONTINUE, NONE
    }

    public static Mode mode;

    public enum Phase
    {
            FADE_IN, STATIC, FADE_OUT
    }
    private Phase phase;

    private static Task task;
    private static Exception error;

    // Start is called before the first frame update
    void Awake()
    {

        phase = Phase.FADE_IN;

        Debug.Log("I: Inter Level Scene with mod : " + mode);
        //task = new Task(() =>
        //{
            //try {
                    Debug.Log("I: Thread running");
                    switch (mode)
                    {
                        case Mode.DESCEND:
                            Descend();
                            break;
                        case Mode.ASCEND:
                            Ascend();
                            break;
                        case Mode.CONTINUE:
                            Restore();
                            break;
                    }
            //}
            //catch(Exception e)
            //{
            //    error = e;
            //}

            if (phase == Phase.STATIC && error == null)
            {
                phase = Phase.FADE_OUT;
                Debug.Log("I: FADE OUT");
                
            }
            
        //});
        //task.Start();
    }
    void Update()
    {
        Debug.Log("Updete: " + phase);
        switch (phase)
        {
            case Phase.FADE_IN:

                if (/*task.IsCompleted && */error == null)
                {
                    phase = Phase.FADE_OUT;
                    Debug.Log("I: FADE_OUT");
                }
                else
                {
                    phase = Phase.STATIC;
                    Debug.Log("I: STATIC");
                }
                break;

            case Phase.FADE_OUT:
                Debug.Log("I: GO TO GAME SCENE");
                SceneManager.LoadScene(3);
                task = null;
                error = null;
                break;

            case Phase.STATIC:
                if(error != null)
                {
                    Debug.Log(error);
                    Application.Quit();
                }
                break;
        }
    }

    private void Descend()
    {
        Debug.Log("I: Descending");
        if (Dungeon.hero == null)
        {
            Debug.Log("I: Hero Null");
            Dungeon.Initiate();
            Debug.Log("I: Dungeon Initiated");
            Level level = Dungeon.NewLevel();
            Debug.Log("I: New level gen");
            Dungeon.SwitchLevel(level, level.entrance);
            Debug.Log("I: Switcherd to level");
        }
        else
        {
            Debug.Log("I: Hero !Null");
            Dungeon.SaveAll();

            Level level;
            Dungeon.depth += 1;

            if (Dungeon.depth > Statistics.deepestFloor)
            {
                level = Dungeon.NewLevel();
            }
            else
            {
                level = Dungeon.LoadLevel(GameInProgress.curSlot);
            }

            Dungeon.SwitchLevel(level, level.entrance);
        }
        Debug.Log("I: Descending fin");
    }

    private void Ascend()
    {
        Dungeon.SaveAll();
        Dungeon.depth -= 1;
        Level level = Dungeon.LoadLevel(GameInProgress.curSlot);

        Dungeon.SwitchLevel(level, level.exit);
    }

    private void Restore()
    {

        Dungeon.LoadGame(GameInProgress.curSlot);

        Level level = Dungeon.LoadLevel(GameInProgress.curSlot);
        Dungeon.SwitchLevel(level, Dungeon.hero.position);

    }

    // Update is called once per frame
    
}
