using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed;

    private Vector2 _moveInput;
    private bool _interactInput;

    private Vector2 _facingDir;

    public LayerMask interactLayerMask;

    public Rigidbody2D rig;
    public SpriteRenderer sr;

    private void Update()
    {
        if (_moveInput.magnitude != 0.0f)
        {
            _facingDir = _moveInput.normalized;
            sr.flipX = _moveInput.x > 0;
        }

        if (_interactInput)
        {
            TryInteractTile();
            _interactInput = false;
        }
    }

    private void FixedUpdate()
    {
        rig.velocity = _moveInput.normalized * moveSpeed;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void OnInteractInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _interactInput = true;
        }
    }

    void TryInteractTile()
    {
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position + _facingDir, Vector3.up, 0.1f, interactLayerMask);

        if (hit.collider != null)
        {
            FieldTile tile = hit.collider.GetComponent<FieldTile>();
            tile.Interact();
        }
    }
}
