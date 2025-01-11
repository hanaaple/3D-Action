using Save;
using UI.Base;
using UI.Selectable.Container;
using UnityEngine;
using UnityEngine.UI;
using Util;

namespace UI.Title
{
    // Load 및 List UI 만들까, 이전에 만들었던 거 있어서 엄청 오래 걸리진 않는데...
    public class TitleMenu : UIContainerEntity
    {
        [SerializeField] private SelectableSlotContainer selectableSlotContainer;
        
        [Header("Start Game")] [SerializeField]
        private Button newStartGameButton;

        [SerializeField] private Button continueGameButton;

        protected override SelectableSlotContainer GetCurrentContainer()
        {
            return selectableSlotContainer;
        }

        private void Start()
        {
            continueGameButton.gameObject.SetActive(SaveManager.IsLoadEnable());

            newStartGameButton.onClick.AddListener(() =>
            {
                // GameLoad 중 Input이 가능하여 문제가 발생할 수 있다.
                var input = GetUIInput();
                input.Enable = false;
                SceneLoader.Instance.LoadScene("TestScene", SceneLoadType.NewGame);
            });

            continueGameButton.onClick.AddListener(() =>
            {
                var input = GetUIInput();
                input.Enable = false;
                SceneLoader.Instance.LoadScene("TestScene", SceneLoadType.LoadGame);
            });
        }
    }
}