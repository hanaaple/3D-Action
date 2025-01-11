using Manager;
using UI.ToastMessage;
using UnityEngine;
using UnityEngine.UI;

namespace Test_Script.Toast_Message
{
    public class ToastMessageTester : MonoBehaviour
    {
        public Button toastButton;

        public string itemName;
        public Sprite itemSprite;
        public int itemCount;

        void Start()
        {
            toastButton.onClick.AddListener(() =>
            {
                PlaySceneManager.instance.toastMessageManager.ToastMessage(itemName, itemSprite, itemCount);
            });
        }
    }
}
