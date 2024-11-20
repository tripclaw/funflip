using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(GridLayoutGroup))]
public class LayoutCards : MonoBehaviour
{
    RectTransform rectTransform;
    GridLayoutGroup grid;

    public int itemCountX = 2;
    public int itemCountY = 2;

    private void Awake()
    {
        Init();
    }

    void Start()
    {
        UpdateGrid();
    }

    void Init()
    {
        rectTransform = transform as RectTransform;
        grid = GetComponent<GridLayoutGroup>();
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = itemCountX;
    }
    void OnRectTransformDimensionsChange()
    {
        UpdateGrid();
    }

#if UNITY_EDITOR
    [ExecuteAlways]
    void Update()
    {
        UpdateGrid();
    }
    void OnValidate()
    {
        Init();
    }
#endif
    void UpdateGrid()
    {
        if (grid == null)
            return;

        float containerSizeX = rectTransform.rect.width - grid.padding.left - grid.padding.right;
        float containerSizeY = rectTransform.rect.height - grid.padding.top - grid.padding.bottom;

        float contentAspectRatio = containerSizeX / containerSizeY;
        float gridAspectRatio = (float)itemCountX / (float)itemCountY;

        float cellSize;

        if (contentAspectRatio < gridAspectRatio)
        {
            // fit width
            cellSize = (containerSizeX / itemCountX) - grid.spacing.x;
        }
        else
        {
            // fit height
            cellSize = (containerSizeY / itemCountY) - grid.spacing.y;
        }

        grid.cellSize = new Vector2(cellSize, cellSize);

        // for testing
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(i < itemCountX * itemCountY);
        }
    }
}