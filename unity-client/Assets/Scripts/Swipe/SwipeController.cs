using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Mdb.Ctd.Swipe
{
    public class SwipeController : MonoBehaviour
    {
        private enum State
        {
            Idle,
            BeginSwipe,
            Swiping,
            EndSwipe
        }

        private State _state = State.Idle;
        private bool _skipFrame = false;
        private ISwipable _currentSwipableUnderPointer;

        private Action<ISwipable> _onBeginSwipe;
        private Action<ISwipable> _onSwipeOverElement;
        private Action _onEndSwipe;

        public void Initialize(Action<ISwipable> onBeginSwipe, Action<ISwipable> onSwipeOverElement,
            Action onEndSwipe)
        {
            _onBeginSwipe = onBeginSwipe;
            _onSwipeOverElement = onSwipeOverElement;
            _onEndSwipe = onEndSwipe;
        }

        private void Update()
        {
            State lastStateInFrame;

            do
            {
                lastStateInFrame = _state;
                ProcessState();

                if (_skipFrame)
                {
                    _skipFrame = false;
                    break;
                }

            } while (lastStateInFrame != _state);
        }

        private void ProcessState()
        {
            switch (_state)
            {
                case State.Idle:
                    Idle();
                    break;
                case State.BeginSwipe:
                    BeginSwipe();
                    break;
                case State.Swiping:
                    Swiping();
                    break;
                case State.EndSwipe:
                    EndSwipe();
                    break;
                default:
                    break;
            }
        }

        private void Idle()
        {
            if (!Pointer.current.press.wasPressedThisFrame) return;

            if (!TryGetSwipableUnderPointer(out ISwipable swipable)) return;

            _currentSwipableUnderPointer = swipable;
            _state = State.BeginSwipe;
        }

        private void BeginSwipe()
        {
            _onBeginSwipe.Invoke(_currentSwipableUnderPointer);
            _state = State.Swiping;
            _skipFrame = true;
        }

        private void Swiping()
        {
            if (Pointer.current.press.isPressed)
            {
                if (!TryGetSwipableUnderPointer(out ISwipable swipable)) return;

                if (swipable == _currentSwipableUnderPointer) return;

                _currentSwipableUnderPointer = swipable;
                _onSwipeOverElement.Invoke(_currentSwipableUnderPointer);
            }
            else
            {
                _state = State.EndSwipe;
            }
        }

        private void EndSwipe()
        {
            _onEndSwipe.Invoke();
            
            _currentSwipableUnderPointer = null;
            _state = State.Idle;
            _skipFrame = true;
        }

        private bool TryGetSwipableUnderPointer(out ISwipable swipable)
        {
            PointerEventData pointerPosition = new (EventSystem.current);
            pointerPosition.position = Pointer.current.position.ReadValue();
            List<RaycastResult> raycastResults = new();
            EventSystem.current.RaycastAll(pointerPosition, raycastResults);

            foreach (RaycastResult result in raycastResults)
            {
                if (result.gameObject.TryGetComponent(out swipable)) return true;
            }

            swipable = null;
            return false;
        }
    }
}
