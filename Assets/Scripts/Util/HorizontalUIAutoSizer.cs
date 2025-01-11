using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

// gridSize에 따라 CellSize가 조정된다. (Padding, Spacing 자동 조정)
public class HorizontalUIAutoSizer : MonoBehaviour
{
    [SerializeField] private int xSize;
    
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            Undo.RecordObject(this, "Modified Cell Size");
            
            var rectTransform = transform as RectTransform;
            Vector2 cellSize;

            var width = rectTransform.rect.width;
            //var height = rectTransform.rect.height;
        
            // var width = gridLayoutGroup.spacing.x * (gridSize.x - 1) + gridLayoutGroup.cellSize.x * gridSize.x + 
            //                    gridLayoutGroup.padding.left + gridLayoutGroup.padding.right;

            // x, y가 1:1이어야됨.
        
            // x y 중에서 
        
            cellSize.x = (width - (gridLayoutGroup.padding.left + gridLayoutGroup.padding.right) -
                          gridLayoutGroup.spacing.x * (xSize - 1)) / xSize;

            // cellSize.y = (height - (gridLayoutGroup.padding.top + gridLayoutGroup.padding.bottom) -
            //               gridLayoutGroup.spacing.y * (gridSize.y - 1)) / gridSize.y;
        
            // var maxLength = Mathf.Min(cellSize.x, cellSize.y);
            //cellSize.x = maxLength;
            cellSize.y = cellSize.x;
        
            gridLayoutGroup.cellSize = cellSize;
            
            EditorUtility.SetDirty(this);
        }
    }
}
