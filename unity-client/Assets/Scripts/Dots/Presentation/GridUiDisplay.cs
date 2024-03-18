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
        [SerializeField] private DotUiDisplay _dotPrefab;
        [SerializeField] private SwipeController _swipeController;
        [SerializeField] private DotUiDisplay _currentSequenceValueDisplay;

        private IDotGridDataReader _dotGridDataReader;
        private IDotsService _dotsService;
        private INotificationService _notificationService;

        private Dictionary<IDot, DotUiDisplay> _dotsDisplays = new();

        private List<DotUiDisplay> _currentDotsSwipedOver = new();

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
            DotUiDisplay unifiedDot = _dotsDisplays[notification.UnifiedDot];
            unifiedDot.UpdateDotValue();

            foreach (IDot removedDot in notification.RemovedDots)
            {
                _dotsDisplays[removedDot].MergeInto(unifiedDot);
                _dotsDisplays.Remove(removedDot);
            }
        }

        private void OnGridUpdated(GridUpdatedNotification _)
        {
            UpdateGrid();
        }

        private void UpdateGrid()
        {
            IDot[,] grid = _dotGridDataReader.Grid;

            for (int x = 0; x < grid.GetLength(0); ++x)
            {
                for (int y = 0; y < grid.GetLength(1); ++y)
                {
                    IDot dot = grid[x, y];
                    if (dot == null) continue;

                    Transform dotHolder = _dotHolders[x + y * grid.GetLength(0)];

                    if (_dotsDisplays.TryGetValue(dot, out DotUiDisplay dotDisplay))
                    {
                        dotDisplay.UpdatePosition(dotHolder);
                    }
                    else
                    {
                        dotDisplay = Instantiate(_dotPrefab, dotHolder);
                        dotDisplay.Setup(dot);
                        _dotsDisplays[dot] = dotDisplay;
                    }
                }
            }
        }

        private void OnBeginSwipe(ISwipable swipable)
        {
            DotUiDisplay dotPressed = ((Swipable) swipable).Dot;

            _currentDotsSwipedOver.Add(dotPressed);
            dotPressed.SetSelected(true);

            _currentSequenceValueDisplay.SetVisible(true);
            _currentSequenceValueDisplay.SetValue(_dotGridDataReader.GetSequenceValue(
                _currentDotsSwipedOver.Select(x => (x.Dot.X, x.Dot.Y)).ToArray()));
        }

        private void OnSwipeOverDot(ISwipable swipable)
        {
            DotUiDisplay dotSwipedOver = ((Swipable)swipable).Dot;

            for (int i = 0; i < _currentDotsSwipedOver.Count; ++i)
            {
                if (_currentDotsSwipedOver[i] != dotSwipedOver) continue;

                if (i == _currentDotsSwipedOver.Count - 1) return;

                for (int j = _currentDotsSwipedOver.Count - 1; j > i; --j)
                {
                    _currentDotsSwipedOver[j].SetSelected(false);
                }

                _currentDotsSwipedOver.RemoveRange(i + 1, _currentDotsSwipedOver.Count - i - 1);

                _currentSequenceValueDisplay.SetValue(_dotGridDataReader.GetSequenceValue(
                    _currentDotsSwipedOver.Select(x => (x.Dot.X, x.Dot.Y)).ToArray()));

                return;
            }

            IDot lastSequencedDot = _currentDotsSwipedOver[^1].Dot;
            IDot nextDot = dotSwipedOver.Dot;
            if (!_dotGridDataReader.CanConnect(lastSequencedDot.X, lastSequencedDot.Y, nextDot.X, nextDot.Y)) return;

            _currentDotsSwipedOver.Add(dotSwipedOver);
            dotSwipedOver.SetSelected(true);

            _currentSequenceValueDisplay.SetValue(_dotGridDataReader.GetSequenceValue(
                _currentDotsSwipedOver.Select(x => (x.Dot.X, x.Dot.Y)).ToArray()));
        }

        private void OnEndSwipe()
        {
            _dotsService.ConnectSequence(_currentDotsSwipedOver.Select(x => (x.Dot.X, x.Dot.Y)).ToArray());

            for (int i = 0; i < _currentDotsSwipedOver.Count; ++i)
            {
                _currentDotsSwipedOver[i].SetSelected(false);
            }
            _currentDotsSwipedOver.Clear();

            _currentSequenceValueDisplay.SetVisible(false);
        }

        private IEnumerator InitializeGrid()
        {
            yield return new WaitForEndOfFrame();

            UpdateGrid();
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
