using Mdb.Ctd.Dots.Data;
using Mdb.Ctd.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mdb.Ctd.Dots.Presentation
{
    public class GridDotUiDisplay : DotUiDisplay
    {
        [SerializeField] private ConnectionUiDisplay _northConnection;
        [SerializeField] private ConnectionUiDisplay _southConnection;
        [SerializeField] private ConnectionUiDisplay _eastConnection;
        [SerializeField] private ConnectionUiDisplay _westConnection;
        [SerializeField] private ConnectionUiDisplay _northEastConnection;
        [SerializeField] private ConnectionUiDisplay _northWestConnection;
        [SerializeField] private ConnectionUiDisplay _southEastConnection;
        [SerializeField] private ConnectionUiDisplay _southWestConnection;

        private List<ConnectionUiDisplay> _allConnections => new List<ConnectionUiDisplay>()
        {
            _northConnection,
            _southConnection,
            _eastConnection,
            _westConnection,
            _northEastConnection,
            _northWestConnection,
            _southEastConnection,
            _southWestConnection
        };

        public IDot Dot { get; private set; }

        public void Setup(IDot dot)
        {
            Dot = dot;

            UpdateDotValue();

            Rect parent = ((RectTransform)transform.parent).rect;
            ((RectTransform)transform).sizeDelta = new Vector2(parent.width, parent.height);
        }

        public void SetVisibleAsNew(float delay)
        {
            StartCoroutine(SetVisibleAsNewAfterDelay(delay));
        }

        private IEnumerator SetVisibleAsNewAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            Animator.SetTrigger(AnimatorUtils.New);
            SetVisible(true);
        }

        public void SetSelected(bool selected)
        {
            Animator.SetBool(AnimatorUtils.Selected, selected);
        }

        public void UpdateDotValue()
        {
            SetValue(Dot.Value);

            _allConnections.ForEach(connection => connection.SetValue(Dot.Value));
        }

        public void MergeInto(Transform unifiedDotHolder, float animationTime)
        {
            UpdatePosition(unifiedDotHolder, animationTime, () => Destroy(gameObject));
        }

        public void UpdatePosition(Transform dotHolder, float animationTime, Action callback = null)
        {
            if (transform.parent == dotHolder) return;

            transform.SetParent(dotHolder, true);

            StartCoroutine(ResetLocalPosition(animationTime, callback));
        }

        private IEnumerator ResetLocalPosition(float animationTime, Action callback)
        {
            float totalTime = 0;
            Vector3 initialPosition = transform.localPosition;
            Vector3 finalPosition = new Vector3(0, 0, initialPosition.z);

            while (true)
            {
                if (totalTime >= animationTime)
                {
                    transform.localPosition = finalPosition;
                    break;
                }
                else
                {
                    transform.localPosition = Vector3.Lerp(initialPosition, finalPosition, totalTime / animationTime);
                }

                yield return null;

                totalTime += Time.deltaTime;
            }

            callback?.Invoke();
        }

        public void SetConnectionVisible(Direction direction, bool visible)
        {
            switch (direction)
            {
                case Direction.North:
                    _northConnection.SetVisible(visible);
                    break;
                case Direction.South:
                    _southConnection.SetVisible(visible);
                    break;
                case Direction.East:
                    _eastConnection.SetVisible(visible);
                    break;
                case Direction.West:
                    _westConnection.SetVisible(visible);
                    break;
                case Direction.Northeast:
                    _northEastConnection.SetVisible(visible);
                    break;
                case Direction.Northwest:
                    _northWestConnection.SetVisible(visible);
                    break;
                case Direction.Southeast:
                    _southEastConnection.SetVisible(visible);
                    break;
                case Direction.Southwest:
                    _southWestConnection.SetVisible(visible);
                    break;
            }
        }

        public void AnimateFall()
        {
            Animator.SetTrigger(AnimatorUtils.Fall);
        }

        public void HideAllConnections()
        {
            _allConnections.ForEach(connection => connection.SetVisible(false));
        }
    }
}
