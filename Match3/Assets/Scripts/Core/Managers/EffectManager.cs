// Copyright (c) 2012-2025 FuryLion Group. All Rights Reserved.

using UnityEngine;
using Core.Grid;

namespace Core.Managers
{
    public class EffectManager : MonoBehaviour
    {
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private Transform _effectsContainer;
        [SerializeField] private ParticleSystem _bombEffectElement;
        [SerializeField] private ParticleSystem _rocketEffectElement;

        private static EffectManager _instance;

        private void Awake()
        {
            if (_instance != null)
                Destroy(this);

            _instance = this;
        }

        public static void SpawnRocketEffect(int i)
        {
            var j = 0;
            ElementView element;
            while ((element = _instance._gridManager.GetElementView(i, j)) != null)
            {
                Instantiate(_instance._rocketEffectElement,
                    new Vector2(element.transform.position.x, element.transform.position.y), Quaternion.identity,
                    _instance._effectsContainer);

                j++;
            }
        }

        public static void SpawnBombEffect(int x, int y, int bombSize)
        {
            var startI = x - bombSize;
            var endI = x + bombSize;
            var startJ = y - bombSize;
            var endJ = y + bombSize;

            for (var i = startI; i <= endI; i++)
            {
                for (var j = startJ; j <= endJ; j++)
                {
                    var element = _instance._gridManager.GetElementView(i, j);

                    if (element == null)
                        continue;

                    Instantiate(_instance._bombEffectElement,
                        new Vector2(element.transform.position.x, element.transform.position.y), Quaternion.identity,
                        _instance._effectsContainer);
                }
            }
        }
    }
}