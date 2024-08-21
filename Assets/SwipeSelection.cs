using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class SwipeSelection : MonoBehaviour
{
    public LayerMask cellLayer;
    public float minSwipeDistance = 1f;

    private Vector2 startMousePosition;
    private Vector2 endMousePosition;
    private List<GameObject> selectedCells = new List<GameObject>();
    private List<GemBase> selectedGems = new List<GemBase>();

    private bool isSelecting = false;

    [SerializeField] private Sprite _sprite;
    [SerializeField] private Sprite _defaultSprite;
    [SerializeField] private Sprite _wrongSprite;
    
    public float rayLength = 1f;
    public int rayCount = 10;
    public float raySpacing = 0.1f;

    private List<GameObject> hitCells = new List<GameObject>();


    private GameObject _previousCollider;

    [SerializeField] private Grid grid;

    private GemType _currentGemType;
    private GameObject firstPressedCell;
    private GameObject previousPressedCell;
    private bool _isFirstCellSelected;
    
    
    private bool isFalling = false;

    public bool _isBlocked;

    public bool IsBlocked => _isBlocked;

    [SerializeField] private Score _score;
    [SerializeField] private AudioControl audioControl;
    
    [SerializeField] private AudioClip _soundWin;
    [SerializeField] private AudioClip _soundLose;

    [SerializeField] private AudioClip _soundPass;
    

    private void OnEnable()
    {
        selectedGems = new List<GemBase>();
        selectedCells = new List<GameObject>();

        if (grid.CellGrid != null)
        {
            foreach (var cell in grid.CellGrid)
            {
                cell.GetComponent<SpriteRenderer>().sprite = _defaultSprite;
            }
        }

        isSelecting = false;
        isFalling = false;
        _isBlocked = false;
        _isFirstCellSelected = false;
        previousPressedCell = null;
        firstPressedCell = null;

    }

    private void OnDisable()
    {
        selectedGems.Clear();
        selectedCells.Clear();
    }

    void Update()
    {
        if (_isBlocked || grid.IsFalling)
        {
            return;
        }

        if (!grid.enabled)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Clear previous selection when starting a new selection
            isSelecting = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isSelecting = false;
            _isFirstCellSelected = false;
            previousPressedCell = null;
            firstPressedCell = null;
            _currentGemType = GemType.None;
            RemoveFinger();
        }
        
        if (isSelecting)
        {
            // During drag, perform raycast and update selection
            PerformSnakeRaycast();
        }
    }
    
    void PerformSnakeRaycast()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Calculate start direction based on mouse movement
        Vector2 startDirection = (Vector2)Input.mousePosition - mousePosition;
        startDirection.Normalize();

        // Calculate perpendicular direction for "snake" pattern
        Vector2 perpendicularDirection = new Vector2(-startDirection.y, startDirection.x);

        for (int i = 0; i < rayCount; i++)
        {
            // Calculate offset for current ray
            float offset = (i - (rayCount - 1) / 2f) * raySpacing;

            // Calculate direction for current ray
            Vector2 rayDirection = startDirection + offset * perpendicularDirection;

            // Perform raycast
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, rayDirection, rayLength, cellLayer);
            
            if (hit.collider != null)
            {
                //if (_previousCollider != null)
                GameObject cell = hit.collider.gameObject;

                if (cell == null)
                {
                    return;
                }

                var pos = cell.transform.localPosition;
                //Debug.Log($"X {(int)pos.x} Y {(int)pos.y}");
                var gem = grid.GemGrid[(int)(pos.x / CustomConstantAttribute.CELL_OFFSET), (int)(pos.y / CustomConstantAttribute.CELL_OFFSET)];
                var gemType = gem.gemType;

                if (isSelecting && !_isFirstCellSelected)
                {
                    firstPressedCell = cell;
                    _currentGemType = gemType;
                    _isFirstCellSelected = true;
                }

                if (previousPressedCell != null)
                {
                    var prevPos = previousPressedCell.transform.localPosition;
                    var chooseSideOnly = Mathf.Abs(pos.x - prevPos.x) + Mathf.Abs(pos.y - prevPos.y) > 1;

                    if (chooseSideOnly)
                    {
                        return;
                    }
                }

                if (_currentGemType != GemType.None && _currentGemType != gemType)
                {
                    return;
                }
                
                // Add hit cell to selected cells if not already selected
                if (!selectedCells.Contains(cell))
                {
                    selectedCells.Add(cell);
                    Debug.Log("selectedCells.Add(cell);");
                    Debug.Log($"cell == null {cell == null}");
                    selectedGems.Add(gem);
                    Debug.Log($"gem == null {gem == null}");

                    HighlightCell(cell);
                    previousPressedCell = cell;
                }

                if (_previousCollider != cell)
                {
                    _previousCollider = cell;
                }
            }
        }
    }

    void HighlightCell(GameObject cell)
    {
        // Implement your own logic to highlight the selected cell
        // For example, you can change its color or scale
        cell.GetComponent<SpriteRenderer>().sprite = _sprite;
    }

    void RemoveFinger()
    {
        var delayNeed = false;

        if (selectedCells.Count == 0)
        {
            return;
        }
        
        if (selectedCells.Count < 3)
        {

            foreach (GameObject cell in selectedCells)
            {
                // Reset cell to its original state (you can implement your own reset mechanism)
                cell.GetComponent<SpriteRenderer>().sprite = _wrongSprite;
            }
            Debug.Log(selectedCells.Count);
        }
        else
        {
            audioControl.PlaySound(_soundPass);

            for (int i = 0; i < selectedGems.Count; i++)
            {
                Debug.Log(selectedGems);
                Debug.Log($"selectedGems.Count {selectedGems.Count}");
                Debug.Log($"selectedGems[i] == null {selectedGems[i] == null}");
                DestroyImmediate(selectedGems[i].gameObject);
                selectedGems[i] = null;
            }
            delayNeed = _score.ScoreAdd(selectedGems.Count);
            grid.FallGems();
        }
        
        StartCoroutine(Delay());

        if (delayNeed)
        {
            audioControl.PlaySound(_soundWin);
            StopAllCoroutines();
            TurnBack();
            _isBlocked = true;
        }
    }

    IEnumerator Delay()
    {
        _isBlocked = true;
        Debug.Log("Da");
        yield return new WaitForSeconds(0.5f);
        TurnBack();
        _isBlocked = false;
        Debug.Log("Da2");

    }

    void TurnBack()
    {
        if (selectedCells == null) return;
        foreach (GameObject cell in selectedCells)
        {
            // Reset cell to its original state (you can implement your own reset mechanism)
            cell.GetComponent<SpriteRenderer>().sprite = _defaultSprite;
        }
        selectedCells.Clear();
        selectedGems?.Clear();

    }

    public void SwitchBlockGame(bool state)
    {
        _isBlocked = state;
    }

    public void Restart()
    {
        _isBlocked = true;
        grid.ReshuffleGrid();
        _isBlocked = false;
    }

    public void PlayLose()
    {
        audioControl.PlaySound(_soundLose);
    }

}
