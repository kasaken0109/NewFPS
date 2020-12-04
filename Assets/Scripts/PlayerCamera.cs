using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]

public class PlayerCamera : MonoBehaviour
{
    //[SerializeField] private Animator _animator;
    private float moveSpeed = 7f;

    private CharacterController _characterController;
    [SerializeField] private CinemachineVirtualCamera _camera; //エディタでVirtual Cameraをアタッチ
    private CinemachineOrbitalTransposer _transposer;
    private Transform _transform;

    private Vector3 _moveVelocity;
    private Vector3 world_moveVelocity;
    private Vector3 _cameraRotation;

    void Start()
    {
        _characterController = GetComponent<CharacterController>();
        _transposer = _camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _transform = transform;

        _moveVelocity = Vector3.zero;
        world_moveVelocity = Vector3.zero;
        _cameraRotation = Vector3.zero;
    }

    void Update()
    {
        world_moveVelocity = Vector3.zero;
        _moveVelocity = Vector3.zero;

        _cameraRotation.x = Input.GetAxisRaw("Horizontal2");
        _cameraRotation.z = Input.GetAxisRaw("Vertical2");

        //if (_cameraRotation.magnitude >= 0.1)
        //{
        //    _transposer.m_Heading.m_Bias += _cameraRotation.x * 3f; //Biasを操作
        //    _transposer.m_FollowOffset.y -= _cameraRotation.z / 8f; //Follow Offsetを操作

        //}

        _moveVelocity.x = Input.GetAxisRaw("Horizontal1") * moveSpeed;
        _moveVelocity.z = Input.GetAxisRaw("Vertical1") * moveSpeed;

        if (_moveVelocity.magnitude >= 0.01)
        {
            //座標系の補正
            world_moveVelocity = Quaternion.AngleAxis(_transposer.m_Heading.m_Bias, Vector3.up) * _moveVelocity;

            Vector3 targetPositon = _transform.position + world_moveVelocity;
            //向かせたい方向
            Quaternion targetRotation = Quaternion.LookRotation(targetPositon - _transform.position);

            _transform.rotation = Quaternion.Slerp(_transform.rotation, targetRotation, 0.2f);
        }

        _characterController.Move(world_moveVelocity * Time.deltaTime);

        //_animator.SetFloat("Speed", new Vector3(_moveVelocity.x, 0, _moveVelocity.z).magnitude);

    }
}

