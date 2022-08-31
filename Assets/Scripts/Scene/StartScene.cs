using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class StartScene : MonoBehaviour
{
    [SerializeField]
    private Transform layoutPatent;
    private void Awake()
    {
        List<GameInProgress.Info> games = GameInProgress.CheckAll();

        Object slotPrefab = Resources.Load("Prefabs/SaveSlotButton");

        foreach (GameInProgress.Info game in games)
        {
            GameObject slot = (GameObject)Instantiate(slotPrefab, layoutPatent.transform);
            SaveSlotButton existingGame = slot.AddComponent<SaveSlotButton>();
            existingGame.Set(game.slot);
        }

        if(games.Count < GameInProgress.MAX_SLOTS)
        {
            GameObject slot = (GameObject)Instantiate(slotPrefab, layoutPatent.transform);
            SaveSlotButton existingGame = slot.AddComponent<SaveSlotButton>();
            existingGame.Set(GameInProgress.FirstEmpty());
        }

        GameInProgress.curSlot = 0;
    }
    public void MoveToTitleScene()
    {
        SceneManager.LoadScene(0);
    }

    private class SaveSlotButton : MonoBehaviour
    {
        private TextMeshProUGUI textObj;
        private GameObject deleteButton;
        private Button button;

        private int slot;
        private bool newGame;

        private void Awake()
        {
            textObj = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);

            deleteButton = transform.GetChild(1).gameObject;
        }

        public void Set(int slot)
        {

            this.slot = slot;
            GameInProgress.Info info = GameInProgress.Check(slot);
            newGame = info == null;
            if (newGame)
            {
                textObj.text = "New Game";
                Destroy(deleteButton);
            }
            else
            {
                textObj.text = "Slot " + slot.ToString();
                deleteButton.AddComponent<DeleteSaveButton>().Set(slot);
            }
        }

        protected void OnClick()
        {
            GameInProgress.curSlot = slot;
            Dungeon.hero = null;

            if (newGame)
            {
                InterLevelScene.mode = InterLevelScene.Mode.DESCEND;
            }
            else
            {
                InterLevelScene.mode = InterLevelScene.Mode.CONTINUE;
            }
        
            SceneManager.LoadScene(2);
        }

        private class DeleteSaveButton : MonoBehaviour
        {
            private Button button;

            private int slot;

            private void Awake()
            {
                button = GetComponent<Button>();
                button.onClick.AddListener(OnClick);
            }

            public void Set(int slot)
            {
                this.slot = slot;
            }

            protected void OnClick()
            {
                Dungeon.DeleteGame(slot, true);
                SceneManager.LoadScene(1);
            }
        }
    }
}
