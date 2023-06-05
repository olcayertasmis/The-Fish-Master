using System;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Fish
{
    public class Fish : MonoBehaviour
    {
        private Fish.FishType _type;

        private CircleCollider2D _collider;

        private SpriteRenderer _sprite;

        private float _screenLeft;

        private Tweener _tweener;

        public Fish.FishType Type
        {
            get { return Type; }
            set
            {
                Type = value;
                _collider.radius = Type.colliderRadius;
                _sprite.sprite = Type.sprite;
            }
        }

        private void Awake()
        {
            _collider = GetComponent<CircleCollider2D>();
            _sprite = GetComponent<SpriteRenderer>();
            _screenLeft = Camera.main.ScreenToWorldPoint(Vector3.zero).x;
            
            ResetFish();
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

            float num2 = 1;
            float y = Random.Range(posY - num2, posY + num2);
            Vector2 pos = new Vector2(-position.x, y);

            float num3 = 3;
            float delay = Random.Range(0, 2 * num3);
            _tweener = transform.DOMove(pos, num3, false).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(delay).OnStepComplete(delegate
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
    }
}