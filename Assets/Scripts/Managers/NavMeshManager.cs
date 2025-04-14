using Cysharp.Threading.Tasks;
using Unity.AI.Navigation;
using UnityEngine;

public class NavMeshManager : MonoBehaviour
{
    [SerializeField] private NavMeshSurface surface;
    
    [SerializeField] private float updateInterval = 2f; // Kaç saniyede bir navmesh güncellenecek

    private void Start()
    {
        UpdateNavMeshLoop().Forget(); // UniTask'i başlat
    }

    private async UniTaskVoid UpdateNavMeshLoop()
    {
        while (Application.isPlaying)
        {
            surface.BuildNavMesh(); // NavMesh'i yeniden inşa et
            
            await UniTask.Delay(System.TimeSpan.FromSeconds(updateInterval), ignoreTimeScale: false); 
        }
    }
}