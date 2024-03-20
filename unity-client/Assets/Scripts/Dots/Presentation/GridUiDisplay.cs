using Mdb.Ctd.Data;
using Mdb.Ctd.Dots.Data;
using Mdb.Ctd.Dots.Notifications;
using Mdb.Ctd.Dots.Services;
using Mdb.Ctd.Notifications;
using Mdb.Ctd.Services;
using Mdb.Ctd.Swipe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Mdb.Ctd.Dots.Presentation
{
    public class GridUiDisplay : MonoBehaviour
    {
        [SerializeField] private Transform[] _dotHolders;
        [SerializeField] private GridDotUiDisplay _dotPrefab;
        [SerializeField] private SwipeController _swipeController;
        [SerializeField] private DotUiDisplay _currentSequenceValueDisplay;

        [SerializeField] private float _mergeAnimationTime = 0.1f;
        [SerializeField] private float _fallAnimationTime = 0.1f;

        private IDotGridDataReader _dotGridDataReader;
        private IDotsService _dotsService;
        private INotificationService _notificationService;

        private Dictionary<IDot, GridDotUiDisplay> _dotsDisplays = new();

        private List<GridDotUiDisplay> _currentDotsSwipedOver = new();

        private void Start()
        {
            _dotGridDataReader = DataReaders.Get<IDotGridDataReader>();
            _dotsService = ServiceLocator.Get<IDotsService>();
            _notificationService = ServiceLocator.Get<INotificationService>();

            SubscribeNotifications();

            _swipeController.Initialize(OnBeginSwipe, OnSwipeOverDot, OnEndSwipe);

            StartCoroutine(InitializeGrid());
        }

        private void SubscribeNotifications()
        {
            _notificationService.Subscribe<DotsMergedNotification>(OnDotsMerged);
            _notificationService.Subscribe<GridUpdatedNotification>(OnGridUpdated);
        }

        private void OnDotsMerged(DotsMergedNotification notification)
        {
            StartCoroutine(AnimateDotsMerged(notification.UnifiedDot, notification.RemovedDots));
        }

        private IEnumerator AnimateDotsMerged(IDot unifiedDot, IReadOnlyList<IDot> removedDots)
        {
            Transform unifiedDotHolder =
                _dotHolders[unifiedDot.X + unifiedDot.Y * _dotGridDataReader.Grid.GetLength(0)];

            foreach (IDot removedDot in removedDots)
            {
                _dotsDisplays[removedDot].MergeInto(unifiedDotHolder, _mergeAnimationTime);
                _dotsDisplays.Remove(removedDot);
            }

            GridDotUiDisplay unifiedDotDisplay = _dotsDisplays[unifiedDot];
            unifiedDotDisplay.transform.SetAsLastSibling();

            yield return new WaitForSeconds(_mergeAnimationTime);

            unifiedDotDisplay.UpdateDotValue();
        }

        private void OnGridUpdated(GridUpdatedNotification _)
        {
            StartCoroutine(UpdateGrid(_mergeAnimationTime));
        }

        private IEnumerator UpdateGrid(float delay)
        {
            yield return new WaitForSeconds(delay);

            IDot[,] grid = _dotGridDataReader.Grid;

            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    IDot dot = grid[x, y];
                    if (dot == null) continue;

                    Transform dotHolder = _dotHolders[x + y * grid.GetLength(0)];

                    if (_dotsDisplays.TryGetValue(dot, out GridDotUiDisplay dotDisplay))
                    {
                        dotDisplay.UpdatePosition(dotHolder, _fallAnimationTime);
                    }
                    else
                    {
                        dotDisplay = Instantiate(_dotPrefab, dotHolder);
                        dotDisplay.Setup(dot);
                        dotDisplay.SetVisibleAsNew(_fallAnimationTime);
                        _dotsDisplays[dot] = dotDisplay;
                    }
                }
            }
        }

        private void OnBeginSwipe(ISwipable swipable)
        {
            GridDotUiDisplay dotPressed = ((SwipableDot) swipable).Dot;

            _currentDotsSwipedOver.Add(dotPressed);
            dotPressed.SetSelected(true);

            _currentSequenceValueDisplay.SetVisible(true);
            _currentSequenceValueDisplay.SetValue(_dotGridDataReader.GetSequenceValue(
                _currentDotsSwipedOver.Select(x => (x.Dot.X, x.Dot.Y)).ToArray()));
        }

        private void OnSwipeOverDot(ISwipable swipable)
        {
            GridDotUiDisplay dotSwipedOver = ((SwipableDot)swipable).Dot;
            GridDotUiDisplay lastSequecedDotDisplay = _currentDotsSwipedOver[^1];
            IDot lastSequencedDot = lastSequecedDotDisplay.Dot;

            if (_currentDotsSwipedOver.Count >= 2 && _currentDotsSwipedOver[^2] == dotSwipedOver)
            {
                dotSwipedOver.SetConnectionVisible(GetDirection(dotSwipedOver.Dot, lastSequencedDot), false);

                lastSequecedDotDisplay.SetSelected(false);
                lastSequecedDotDisplay.HideAllConnections();

                _currentDotsSwipedOver.RemoveAt(_currentDotsSwipedOver.Count - 1);
                
                _currentSequenceValueDisplay.SetValue(_dotGridDataReader.GetSequenceValue(
                     _currentDotsSwipedOver.Select(x => (x.Dot.X, x.Dot.Y)).ToArray()));

                return;
            }

            if (_currentDotsSwipedOver.Contains(dotSwipedOver)) return;

            IDot nextDot = dotSwipedOver.Dot;
            if (!_dotGridDataReader.CanConnect(lastSequencedDot.X, lastSequencedDot.Y, nextDot.X, nextDot.Y)) return;

            _currentDotsSwipedOver.Add(dotSwipedOver);
            dotSwipedOver.SetSelected(true);

            lastSequecedDotDisplay.SetConnectionVisible(GetDirection(lastSequencedDot, nextDot), true);
            dotSwipedOver.SetConnectionVisible(GetDirection(nextDot, lastSequencedDot), true);

            _currentSequenceValueDisplay.SetValue(_dotGridDataReader.GetSequenceValue(
                _currentDotsSwipedOver.Select(x => (x.Dot.X, x.Dot.Y)).ToArray()));
        }

        private Direction GetDirection(IDot from, IDot to)
        {
            bool isNorther = from.Y < to.Y;
            bool isSouther = from.Y > to.Y;
            bool isEastern = from.X < to.X;
            bool isWestern = from.X > to.X;

            if (isNorther && isEastern) return Direction.Northeast;
            if (isNorther && isWestern) return Direction.Northwest;

            if (isSouther && isEastern) return Direction.Southeast;
            if (isSouther && isWestern) return Direction.Southwest;

            if (isNorther) return Direction.North;
            if (isSouther) return Direction.South;
            if (isEastern) return Direction.East;
            if (isWestern) return Direction.West;

            throw new ArgumentException();
        }

        private void OnEndSwipe()
        {
            _dotsService.ConnectSequence(_currentDotsSwipedOver.Select(x => (x.Dot.X, x.Dot.Y)).ToArray());

            for (int i = 0; i < _currentDotsSwipedOver.Count; ++i)
            {
                _currentDotsSwipedOver[i].SetSelected(false);
                _currentDotsSwipedOver[i].HideAllConnections();
            }
            _currentDotsSwipedOver.Clear();

            _currentSequenceValueDisplay.SetVisible(false);
        }

        private IEnumerator InitializeGrid()
        {
            yield return new WaitForEndOfFrame();

            IDot[,] grid = _dotGridDataReader.Grid;

            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    IDot dot = grid[x, y];
                    if (dot == null) continue;

                    GridDotUiDisplay dotDisplay = Instantiate(_dotPrefab, _dotHolders[x + y * grid.GetLength(0)]);
                    dotDisplay.Setup(dot);
                    dotDisplay.SetVisible(true);
                    _dotsDisplays[dot] = dotDisplay;
                }
            }
        }

        private void OnDestroy()
        {
            UnsubscribeNotifications();
        }

        private void UnsubscribeNotifications()
        {
            _notificationService.Unsubscribe<DotsMergedNotification>(OnDotsMerged);
            _notificationService.Unsubscribe<GridUpdatedNotification>(OnGridUpdated);
        }
    }
}
