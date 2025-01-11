using UI.Selectable.Container.Item;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotIcon : MonoBehaviour
{
    private static readonly int Selected = Animator.StringToHash("Selected");
    public EventTrigger slotIcon;
    public ItemContainerType equipType;
    
    public RectTransform rectTransform;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        rectTransform = transform as RectTransform;
    }

    public void Select(bool isSelect)
    {
        //Debug.LogWarning($"Try Select {isSelect}  {equipType}");
        _animator.SetBool(Selected, isSelect);
    }
}
