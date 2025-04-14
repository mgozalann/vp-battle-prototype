using System;
using UnityEngine;

public class CharacterSelector : MonoBehaviour
{
    private CharacterBase _selectedCharacter;
    private bool _isDragging = false;
    private Vector3 _dragOffset;

    [SerializeField] private GameObject _selectionVfx;

    [SerializeField] private LayerMask groundLayer;
    
    private Vector3 _mouseStartWorldPos;
    private Vector3 _characterStartPos;

    [SerializeField] private Vector2 xBounds = new Vector2(-5f, 5f);
    [SerializeField] private Vector2 zBounds = new Vector2(-5f, 5f);

    private GameObject _selectedVfx;

    private void OnEnable()
    {
        GameManager.I.OnGameStateChanged+=OnOnGameStateChanged;
    }

    private void OnOnGameStateChanged(GameState obj)
    {
        if (obj == GameState.InBattle)
        {
            if (_selectedVfx != null)
            {
                ObjectPoolManager.ReturnObjectToPool(_selectedVfx);
            }
        }
    }

    private void OnDisable()
    {
        GameManager.I.OnGameStateChanged-=OnOnGameStateChanged;

    }

    private void Update()
    {
        if (GameManager.I.CurrentState != GameState.Preparation) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                var character = hit.collider.GetComponent<CharacterBase>();
                if (character != null && character.IsAlly)
                {
                    if (_selectedCharacter != character)
                    {
                        _selectedCharacter = character;
                        
                        if (_selectedVfx != null)
                        {
                            ObjectPoolManager.ReturnObjectToPool(_selectedVfx);
                        }
                        
                        _selectedVfx = ObjectPoolManager.SpawnObject(_selectionVfx, character.transform.position,
                            _selectionVfx.transform.rotation);
                        
                        _selectedVfx.transform.parent = hit.collider.transform;

                    }
                    
                    _isDragging = true;
                    
                    if (Physics.Raycast(ray, out RaycastHit groundHit, 100f, groundLayer))
                    {
                        _mouseStartWorldPos = groundHit.point;
                        _characterStartPos = character.transform.position;
                    }

                }
            }
        }

        if (Input.GetMouseButton(0) && _isDragging && _selectedCharacter != null)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 currentMouseWorldPos = hit.point;
                Vector3 delta = currentMouseWorldPos - _mouseStartWorldPos;

                if (!_isDragging && delta.magnitude > 0.05f)
                {
                    _isDragging = true;
                }

                if (_isDragging)
                {
                    Vector3 targetPos = _characterStartPos + delta;
                    targetPos.y = 0;

                    Vector3 clampedPos = new Vector3(
                        Mathf.Clamp(targetPos.x, xBounds.x, xBounds.y),
                        0,
                        Mathf.Clamp(targetPos.z, zBounds.x, zBounds.y)
                    );

                    _selectedCharacter.transform.position = clampedPos;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            _isDragging = false;
            //_selectedCharacter = null;
        }
    }
}