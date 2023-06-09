using System;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using DG.Tweening;
using Managers;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Hook
{
    public class Hook : MonoBehaviour
    {
        [SerializeField] private Transform hookedTransform;

        private Collider2D _collider;
        private Camera _camera;

        private int _length;
        private int _strength;
        private int _fishCount;

        private bool _canMove;

        private List<Fish.Fish> _hookedFishes;

        private Tweener _cameraTweener;

        private void Awake()
        {
            _camera = Camera.main;
            _collider = gameObject.GetComponent<Collider2D>();
            _hookedFishes = new List<Fish.Fish>();
        }

        private void Update()
        {
            if (_canMove && Input.GetMouseButton(0))
            {
                Vector3 mousePos = _camera.ScreenToWorldPoint(Input.mousePosition);
                Vector3 position = transform.position;

                position.x = mousePos.x;
                transform.position = position;
            }
        }

        public void StartFishing()
        {
            _length = IdleManager.Instance.length - 20;
            _strength = IdleManager.Instance.strength;
            _fishCount = 0;
            float time = (-_length) * 0.1f;

            _cameraTweener = _camera.transform.DOMoveY(_length, 1 + time * .25f, false).OnUpdate(delegate
            {
                if (_camera.transform.position.y <= -11) transform.SetParent(_camera.transform);
            }).OnComplete(delegate
            {
                _collider.enabled = true;
                _cameraTweener = _camera.transform.DOMoveY(0, time * 5, false).OnUpdate(delegate
                {
                    if (_camera.transform.position.y >= -25f) StopFishing();
                });
            });

            ScreenManager.Instance.ChangeScreen(Screens.GAME);
            
            _collider.enabled = false;
            _canMove = true;
            _hookedFishes.Clear();
        }

        private void StopFishing()
        {
            _canMove = false;
            _cameraTweener.Kill(false);
            _cameraTweener = _camera.transform.DOMoveY(0, 2, false).OnUpdate(delegate
            {
                if (_camera.transform.position.y >= -11)
                {
                    transform.SetParent(null);
                    transform.position = new Vector2(transform.position.x, -6);
                }
            }).OnComplete(delegate
            {
                transform.position = Vector2.down * 6;
                _collider.enabled = true;
                int num = 0;
                for (int i = 0; i < _hookedFishes.Count; i++)
                {
                    _hookedFishes[i].transform.SetParent(null);
                    _hookedFishes[i].ResetFish();
                    num += _hookedFishes[i].Type.price;
                }

                IdleManager.Instance.totalGain = num;
                ScreenManager.Instance.ChangeScreen(Screens.END);
            });
        }

        private void OnTriggerEnter2D(Collider2D target)
        {
            if (target.CompareTag("Fish") && _fishCount != _strength)
            {
                _fishCount++;

                Fish.Fish fish = target.GetComponent<Fish.Fish>();
                fish.Hooked();
                _hookedFishes.Add(fish);

                var targetT = target.transform;
                targetT.SetParent(transform);
                targetT.position = hookedTransform.position;
                targetT.rotation = hookedTransform.rotation;
                targetT.localScale = Vector3.one;

                targetT.DOShakeRotation(5, Vector3.forward * 45, 10, 90, false).SetLoops(1, LoopType.Yoyo).OnComplete(delegate { targetT.rotation = Quaternion.identity; });
                if (_fishCount == _strength) StopFishing();
            }
        }
    }
}