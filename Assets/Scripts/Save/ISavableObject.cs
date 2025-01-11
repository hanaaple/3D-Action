namespace Save
{
    // 항상 MonoBehaviour를 상속하여 사용하지만 부적합함.
    // 오브젝트 인스턴스
    public interface ISavableObject
    {
        // Awake -> Regist & Load Or Create
        public void RegistSaveData();
        public void LoadOrCreateData();
        public void CreateData();
        // LoadData는 각 상속된 SaveData를 매개변수로 하여 사용.
        public IObjectSaveData GetSaveData();
        public string GetName();
        public string GetLabel();
        public string GetId();
        public void GenerateNewId();
    }
}