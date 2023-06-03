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
        private int _fisCount;

        private bool _canMove = false;

        //List<Fish>

        private Tweener _cameraTweener;

        private void Awake()
        {
            _camera = Camera.main;
            _collider = gameObject.GetComponent<Collider2D>();
            //List
        }

        private void Start()
        {
            throw new NotImplementedException();
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
    }
}