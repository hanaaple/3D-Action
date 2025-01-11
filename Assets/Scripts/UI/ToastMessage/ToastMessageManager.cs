using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace UI.ToastMessage
{
    public class ToastMessageManager : MonoBehaviour
    {
        [SerializeField] private Transform root;
        [SerializeField] private GameObject toastPrefab;
        [SerializeField] private float waitSec;
        [SerializeField] private float fadeoutSec;

        private List<ToastMessageEntity> _toastEntities;
        private ObjectPool<ToastMessageEntity> _toastEntityObjectPool;

        private readonly int _stackCount = Animator.StringToHash("StackCount");
        private readonly int _fadeOut = Animator.StringToHash("FadeOut");
        private readonly int _fadeOutSec = Animator.StringToHash("FadeOutSec");

        private void Start()
        {
            _toastEntities = new List<ToastMessageEntity>();

            _toastEntityObjectPool = new ObjectPool<ToastMessageEntity>(
                () =>
                {
                    var toastEntity = Instantiate(toastPrefab, root).GetComponent<ToastMessageEntity>();
                    toastEntity.gameObject.SetActive(false);
                    return toastEntity;
                },
                toastEntity =>
                {
                    ((RectTransform)toastEntity.transform).anchoredPosition = Vector2.zero;
                    toastEntity.gameObject.SetActive(true);
                },
                toastEntity =>
                {
                    toastEntity.gameObject.SetActive(false);
                    toastEntity.context.text = "";
                    toastEntity.count.text = "";
                    toastEntity.iconImage.sprite = null;
                    toastEntity.animator.SetInteger(_stackCount, 0);
                },
                Destroy
            );
        }

        /// <param name="itemName">아이템 아이콘</param>
        /// <param name="itemIconSprite">아이템 이름</param>
        /// <param name="itemCount">아이템 개수</param>
        public void ToastMessage(string itemName, Sprite itemIconSprite, int itemCount)
        {
            foreach (var toastMessageEntity in _toastEntities)
            {
                toastMessageEntity.animator.SetInteger(_stackCount, toastMessageEntity.animator.GetInteger(_stackCount) + 1);
            }   

            var toastEntity = _toastEntityObjectPool.Get();
            toastEntity.animator.SetFloat(_fadeOutSec, 1 / fadeoutSec);
            toastEntity.context.text = itemName;
            toastEntity.count.text = $"x {itemCount.ToString()}";
            toastEntity.iconImage.sprite = itemIconSprite;

            StartCoroutine(RemoveToastEntity(toastEntity));

            _toastEntities.Add(toastEntity);
        }
        
        private IEnumerator RemoveToastEntity(ToastMessageEntity toastEntity)
        {
            yield return new WaitForSeconds(waitSec);
            toastEntity.animator.SetTrigger(_fadeOut);
            yield return new WaitForSeconds(fadeoutSec);

            _toastEntityObjectPool.Release(toastEntity);
            _toastEntities.Remove(toastEntity);
        }
    }
}