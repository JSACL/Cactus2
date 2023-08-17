using System.Collections.Generic;
using System.Timers;
using UnityEngine;
using UnityEngine.Rendering;

public class ModelSystem : MonoBehaviour
{
    [SerializeField]
    bool _displayModel;

    [SerializeField]
    GameObject _playerView;
    [SerializeField]
    GameObject _playerEntityView;
    [SerializeField]
    List<GameObject> _fugaEnemyViews = new();

    private void Start()
    {
        var player = new Player();
        _playerView.AddComponent<PlayerBehaviour>().Model = player;
        _playerEntityView.AddComponent<EntityTransformViewBehaviour>().Model = player;

        foreach (var species1View in _fugaEnemyViews)
        {
            species1View.AddComponent<Species1Behaviour>().Model = new FugaEnemy();
        }
    }
}