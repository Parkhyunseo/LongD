using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using DG.Tweening;

using Managers;

namespace Characters.Weapons
{
    public class Railroad : MonoBehaviour
    {
        [SerializeField]
        Vector3[] _points;

        [SerializeField]
        int _speed;

        [SerializeField]
        RailType _type;

        Dictionary<RailType, Action<Vector3[]>> _railDict;
        AnimationManager _am;

        public enum RailType
        {
            Linear,
            Curved,
            Basier
        }

        public RailType type
        {
            get
            {
                return _type;
            }
        }
        
        private void Awake()
        {
            _am = AnimationManager.Instance;
            _railDict = new Dictionary<RailType, Action<Vector3[]>>
            {
                { RailType.Linear, LinearRail },
                { RailType.Curved, CurvedRail }
            };
        }

        public void Animation(Vector3[] targetWorldPosArray)
        {
            _railDict[_type](targetWorldPosArray);
        }

        IEnumerator InitializeAnimation(Vector3 target)
        {
            Vector3 next = new Vector3(transform.position.x, transform.position.y + 10, transform.position.z);
            Vector3 growUp = Vector3.one * 5;
            float epsilon = 0.1f;
            float accel = 0.1f;
            while(transform.position.y <= next.y - epsilon)
            {
                accel += 0.1f;
                transform.localScale = Vector3.Slerp(transform.localScale, growUp, Time.deltaTime * 3);
                transform.position = Vector3.Lerp(transform.position, next, Time.deltaTime*accel);
                yield return null;
            }
            StartCoroutine(TrackTheRail(target));
        }

        /// <summary>
        /// Points가 비워질 때 까지 다음 경로를 제공
        /// </summary>
        /// <returns></returns>
        IEnumerator TrackTheRail(Vector3 target)
        {
            //Func<Vector3, Vector3, Vector3> rail = _railDict[_type];
            // TODO :: 에니메이션에서 처리해야하는가 여기서 해야하는가?
            //for(int i = 0; i < _points.Length; i++)
            //{
            var dir = target - transform.position;
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            Debug.Log("transform" + transform.position);
            Debug.Log("target" + target);
            float diff = target.y - transform.position.y;
            Vector3 scale = transform.localScale;

            while (transform.position != target)
            {
                transform.localScale = Vector3.Slerp(transform.localScale, scale, Time.deltaTime * 3);
                //transform.position = LinearRail(transform.position, target);//rail(transform.position, target);
                yield return null;
            }
            //yield return BezierCurve(Mathf.Lerp(0, 1, Time.deltaTime*10), _points[i], _points[i + 1]);
            //}
        }

        private void LinearRail(Vector3[] path)
        {
            Sequence sequence = DOTween.Sequence();

            var baseY = transform.position.y;

            sequence.Append(transform.DOMoveY(baseY-10, 2))
                    .Join(transform.DOScale(Vector2.one * 4, 0.8f))
                    .Append(transform.DOScale(Vector2.one * 1, 0.3f))
                    .Join(transform.DOLocalRotate(Vector3.zero, 0.3f))
                    .Append(transform.DOPath(path, 0.5f, PathType.Linear, PathMode.TopDown2D))
                    .AppendCallback(() => { GetComponent<Weapon>().OnCompletedAction(); })
                    .OnComplete(() =>
                    {
                        Camera.main.transform.DOShakePosition(1f, 10, 20).OnComplete(() =>
                        {
                            Camera.main.transform.DOMove(new Vector3(0, 0, -10), 0.5f);
                        });
                        GetComponent<Weapon>().OnCompletedAction();
                        GameManager.Instance.TurnChange();
                    });
        }

        private void CurvedRail(Vector3[] path)
        {
            Sequence sequence = DOTween.Sequence();
            
            var angle = path[1].x > 0 ? -60 : 60;
            var angle2 = path[1].x > 0 ? -20 : 20;
            var angle3 = path[1].x > 0 ? 70 : -70;

            //.Append(transform.DOLookAt(path[0], 0.4f,AxisConstraint.None, Vector3.forward))
            sequence.Append(transform.DOScale(Vector2.one * 8, 1))
                    .Append(transform.DOScale(Vector2.one * 4, 0.3f))
                    .Append(transform.DOPath(path, 2f, PathType.CatmullRom, PathMode.TopDown2D, 15))
                    .Join(transform.DORotate(new Vector3(0, 0, angle), 0.2f).OnComplete(() => {
                        transform.DORotate(new Vector3(0, 0, angle2), 0.2f).OnComplete(() =>
                        {
                            transform.DORotate(new Vector3(0, 0, angle3), 0.2f);
                        });
                    }))
                    .SetEase(Ease.InQuart)
                    .OnComplete(() =>
                    {
                        Camera.main.transform.DOShakePosition(1f, 10, 20).OnComplete(() => 
                        {
                            Camera.main.transform.DOMove(new Vector3(0, 0, -10), 0.5f);
                        });
                        GetComponent<Weapon>().OnCompletedAction();
                        GameManager.Instance.TurnChange();
                    });
        }
    }
}
