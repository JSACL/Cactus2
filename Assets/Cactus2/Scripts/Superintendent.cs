using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Superintendent : MonoBehaviour
{
    [SerializeField]
    GameObject _playerView;
    [SerializeField]
    List<GameObject> _fugaEnemyViews = new();

    private void Start()
    {
        _playerView.AddComponent<PlayerBehaviour>().Model = new Player();

        foreach (var species1View in _fugaEnemyViews)
        {
            species1View.AddComponent<Species1Behaviour>().Model = new FugaEnemy();
        }
    }
}