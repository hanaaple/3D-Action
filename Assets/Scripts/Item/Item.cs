using UnityEngine;

namespace Item
{
    public abstract class Item : ScriptableObject
    {
        public Sprite slotImage;

        // 중복이 불가능함 - 무기, 방어구, 악세사리
        // 중복이 가능함 - 일부 도구
        // 단일 아이템 - 일부 도구

        // 으아아아악

        // 어디선가 Item[] items를 갖고 있지

        // 장착 중인 아이템 (Selected, Idle)까지 구분해서

        public int numberOfPossession;
        public int maximumNumberOfPossession;

        public int itemEffectDescription;
        
        
        
    }
}

// 각 아이템은 Object(Ref)로 존재하고
// 아이템의 상태 (Model)가 달라지면 각 View에 업데이트 시켜야됨

// 즉
// 여러 개의 View 단일 Model과 연결
// Model은 여러 개의 모델과 연결

// OnModelUpdate -> Views.foreach(v -> v.UpdateDisplay(Model))

// 한마디로, 너 mvvm인지 mvp인지 뭔지 공부해야된다 ㅋㅋ 힘내라


// 1. 데이터 기반으로 인벤토리 채우기 Update

// 2. 인벤토리에서 아이템 선택 시 Update  