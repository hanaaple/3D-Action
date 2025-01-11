using Data.ViewModel;
using UnityEngine;

namespace UI
{
    // Primitive? Essential? UI Manager? Provider?
    public class PrimitiveUIManager : MonoBehaviour
    {
        private static PrimitiveUIManager _instance;
        public static PrimitiveUIManager instance => _instance;
        
        public SelectedUIViewModel selectedUiViewModel { get; private set; }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(_instance);
            }
            else
            {
                DestroyImmediate(_instance);
            }

            Initialize();
        }

        private void Initialize()
        {
            selectedUiViewModel = new SelectedUIViewModel();
        }
    }
}