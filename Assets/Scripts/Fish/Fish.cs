using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Fish
{
    public class Fish : MonoBehaviour
    {
        private Fish.FishType _type;

        [Header("Components")]
        private CircleCollider2D _collider;
        private SpriteRenderer _sprite;
        private Tweener _tweener;

        private float _screenLeft;

        public Fish.FishType Type
        {
            get { return _type; }
            set
            {
                _type = value;
                _collider.radius = _type.colliderRadius;
                _sprite.sprite = _type.sprite;
            }
        }

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _sprite = GetComponentInChildren<SpriteRenderer>();
            _screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        }

        public void ResetFish()
        {
            _tweener?.Kill(false);

            float posY = Random.Range(_type.minLenght, _type.maxLenght);
            _collider.enabled = true;

            Vector3 position = transform.position;
            position.y = posY;
            position.x = _screenLeft;
            transform.position = position;

            float num1 = 1;
            float newPosY = Random.Range(posY - num1, posY + num1);
            Vector2 newPosition = new Vector2(-position.x, newPosY);

            float num3 = 3;
            float delay = Random.Range(0, 2 * num3);
            _tweener = transform.DOMove(newPosition, num3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay).OnStepComplete(delegate
            {
                Vector3 localScale = transform.localScale;
                localScale.x = -localScale.x;
                transform.localScale = localScale;
            });
        }

        public void Hooked()
        {
            _collider.enabled = false;
            _tweener.Kill(false);
        }

        [Serializable]
        public class FishType
        {
            public int price;

            public float fishCount;

            public float minLenght;

            public float maxLenght;

            public float colliderRadius;

            public Sprite sprite;
        }
    } //Class
}