using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [Header("Health Bar UI")]
    [SerializeField] private Image healthFillImage; 
    
    [SerializeField] private float lerpDuration; 

    [SerializeField] private CharacterBase characterBase;
    
    private Camera _mainCamera; 

    private void OnEnable()
    {
        characterBase.OnDamageTaken+=OnDamageTaken;
        
    }

    private void OnDamageTaken(int currentHealth, int maxHealth)
    {
        UpdateHealthBar(currentHealth, maxHealth);
    }

    private void OnDisable()
    {
        characterBase.OnDamageTaken-=OnDamageTaken;
    }

    private void Start()
    {
        _mainCamera = Camera.main; 
    }

    private void Update()
    {
        LookAtCamera();
    }
    private void LookAtCamera()
    {
        if (_mainCamera != null)
        {
            // transform.rotation = Quaternion.LookRotation(transform.position - _mainCamera.transform.position );
            
            Vector3 cameraForward = _mainCamera.transform.forward;
            transform.forward = -cameraForward;
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        
        float healthPercentage = (float)currentHealth / maxHealth;
        
        healthFillImage.DOFillAmount(healthPercentage, lerpDuration).SetEase(Ease.OutQuad);

        if (currentHealth <= 0)
        {
            healthFillImage.transform.parent.gameObject.SetActive(false);
        }
    }
}