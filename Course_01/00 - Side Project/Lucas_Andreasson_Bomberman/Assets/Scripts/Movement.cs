using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class Movement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public GameObject playerModel;

    PlayerManager pManager;
    private Vector3 moveDirection;
    private Vector3 rawAxisInput;
    private CharacterController charController;


    void Start()
    {
        pManager = GetComponent<PlayerManager>();
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {

        moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis(pManager.GetInput(PlayerManager.PlayerInput.Horizontal, pManager.playerNum)), 0.0f, Input.GetAxis(pManager.GetInput(PlayerManager.PlayerInput.Vertical, pManager.playerNum))));
        moveDirection *= moveSpeed;

        //moveDirection = transform.TransformDirection(moveDirection);

        rawAxisInput = new Vector3(Input.GetAxisRaw(pManager.GetInput(PlayerManager.PlayerInput.Horizontal, pManager.playerNum)), 0.0f, Input.GetAxisRaw(pManager.GetInput(PlayerManager.PlayerInput.Vertical, pManager.playerNum)));

        if (rawAxisInput != Vector3.zero)
            playerModel.transform.rotation = Quaternion.LookRotation(rawAxisInput);

        charController.Move(moveDirection * Time.deltaTime);

    }
}
