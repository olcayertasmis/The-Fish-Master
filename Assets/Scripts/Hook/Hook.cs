using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

        //List<Fish>

        private Tweener _cameraTweener;

        private void Awake()
        {
            _camera = Camera.main;
            _collider = gameObject.GetComponent<Collider2D>();
            //List<Fish>
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
            _length = -50; //Idle Manager
            _strength = 3; //Idle Manager
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

            //Screen(GAME)
            _collider.enabled = false;
            _canMove = true;
            //Clear
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
                //Clearing out the hook from the fishes
                //Idle Manager totalgain = num
                //Screen manager end screen
            });
        }
    }
}