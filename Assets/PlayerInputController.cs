using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    PlayerInput _playerInput;
    PlayerControll _playerControll;
    // Start is called before the first frame update
    private void Awake()
    {
        TryGetComponent(out _playerInput);
        TryGetComponent(out _playerControll);
    }

    private void OnEnable()
    {
        _playerInput.actions["Move"].performed += OnMove;
        _playerInput.actions["Move"].canceled += OnMoveCanceled;
        _playerInput.actions["Jump"].started += OnJump;
        _playerInput.actions["Dodge"].started += OnDodge;
    }

    private void OnDisable()
    {
        _playerInput.actions["Move"].performed -= OnMove;
        _playerInput.actions["Move"].canceled -= OnMoveCanceled;
        _playerInput.actions["Jump"].started -= OnJump;
        _playerInput.actions["Dodge"].started -= OnDodge;
    }

    private void OnMove(InputAction.CallbackContext obj)
    {
        var direction = obj.ReadValue<Vector2>();
        var dir = new Vector3(direction.x, 0, direction.y);
        _playerControll.Move(dir);
    }

    private void OnMoveCanceled(InputAction.CallbackContext obj)
    {
        _playerControll.Move(Vector3.zero);
    }

    private void OnJump(InputAction.CallbackContext obj)
    {
        _playerControll.Jump();
    }

    private void OnDodge(InputAction.CallbackContext obj)
    {
        _playerControll.Dodge();
    }
}
