using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerManager))]
public class Movement : MonoBehaviour
{
    public float moveSpeed = 2f;

    PlayerManager pManager;
    private float yPos;
    private Vector3 moveDirection;
    private CharacterController charController;
    private GameObject kickTriggerX;
    private GameObject kickTriggerZ;

    void Start()
    {
        pManager = GetComponent<PlayerManager>();
        yPos = transform.position.y;
        kickTriggerX = pManager.kickTriggerX;
        kickTriggerZ = pManager.kickTriggerZ;
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis(pManager.GetInput(PlayerManager.PlayerInput.Horizontal, pManager.playerNum)) > 0)
        {
            kickTriggerX.transform.position = new Vector3(transform.position.x + 0.5f, kickTriggerX.transform.position.y, kickTriggerX.transform.position.z);
        }
        else if (Input.GetAxis(pManager.GetInput(PlayerManager.PlayerInput.Horizontal, pManager.playerNum)) < 0)
        {
            kickTriggerX.transform.position = new Vector3(transform.position.x - 0.5f, kickTriggerX.transform.position.y, kickTriggerX.transform.position.z);
        }

        if (Input.GetAxis(pManager.GetInput(PlayerManager.PlayerInput.Vertical, pManager.playerNum)) > 0)
        {
            kickTriggerZ.transform.position = new Vector3(kickTriggerZ.transform.position.x, kickTriggerZ.transform.position.y, transform.position.z + 0.5f);
        }
        else if (Input.GetAxis(pManager.GetInput(PlayerManager.PlayerInput.Vertical, pManager.playerNum)) < 0)
        {
            kickTriggerZ.transform.position = new Vector3(kickTriggerZ.transform.position.x, kickTriggerZ.transform.position.y, transform.position.z - 0.5f);
        }



        moveDirection = transform.TransformDirection(new Vector3(Input.GetAxis(pManager.GetInput(PlayerManager.PlayerInput.Horizontal, pManager.playerNum)), 0.0f, Input.GetAxis(pManager.GetInput(PlayerManager.PlayerInput.Vertical, pManager.playerNum))));
        moveDirection *= moveSpeed;

        charController.Move(moveDirection * Time.deltaTime);
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
