using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isLeftHash;
    int isBackwardHash;
    int isRightHash;
    int isAttackingHash;
    int isMagicHash;
    public GameObject Kopesh;

    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isForward");
        isRunningHash = Animator.StringToHash("isSprinting");
        isLeftHash = Animator.StringToHash("isLeft");
        isBackwardHash = Animator.StringToHash("isBackward");
        isRightHash = Animator.StringToHash("isRight");
        isAttackingHash = Animator.StringToHash("isAttacking");
        isMagicHash = Animator.StringToHash("isMagic");
    }

    void Update()
    {
        bool isRunning = animator.GetBool(isRunningHash);
        bool forwardPressed = Input.GetKey("w");
        bool isWalking = animator.GetBool(isWalkingHash);
        bool runPressed = Input.GetKey("left shift");
        bool isAttacking = animator.GetBool(isAttackingHash);
        bool isMagic = animator.GetBool(isMagicHash);

        // Check for moving forward
        if (!isWalking && forwardPressed) { animator.SetBool(isWalkingHash, true); }
        if (!forwardPressed && isWalking) { animator.SetBool(isWalkingHash, false); }

        // Check for running
        if (!isRunning && (forwardPressed && runPressed)) { animator.SetBool(isRunningHash, true); }
        if (isRunning && (!forwardPressed || !runPressed)) { animator.SetBool(isRunningHash, false); }

        // Check for moving left
        bool isLeft = animator.GetBool(isLeftHash);
        bool leftPressed = Input.GetKey("a");
        if (!isLeft && leftPressed) { animator.SetBool(isLeftHash, true); }
        if (isLeft && !leftPressed) { animator.SetBool(isLeftHash, false); }

        // Check for moving backward
        bool isBackward = animator.GetBool(isBackwardHash);
        bool backwardPressed = Input.GetKey("s");
        if (!isBackward && backwardPressed) { animator.SetBool(isBackwardHash, true); }
        if (isBackward && !backwardPressed) { animator.SetBool(isBackwardHash, false); }

        // Check for moving right
        bool isRight = animator.GetBool(isRightHash);
        bool rightPressed = Input.GetKey("d");
        if (!isRight && rightPressed) { animator.SetBool(isRightHash, true); }
        if (isRight && !rightPressed) { animator.SetBool(isRightHash, false); }

        // Check for attacking when left mouse button is clicked, check kopesh is turned on, turn on if not already
        if (Input.GetMouseButtonDown(0) && !isAttacking) { animator.SetBool(isAttackingHash, true); }
        if (Input.GetMouseButtonUp(0) && isAttacking) { animator.SetBool(isAttackingHash, false); }

        // Check for using magic when right mouse button is clicked, turn off Kopesh whilst magicking
        if (Input.GetMouseButtonDown(1) && !isMagic) { animator.SetBool(isMagicHash, true); }
        if (Input.GetMouseButtonUp(1) && isMagic) { animator.SetBool(isMagicHash, false); }
        if (isMagic) { HideKopesh(true); } else if (!isMagic) { HideKopesh(false); }
    }

    public void HideKopesh(bool hideOrNot) { Kopesh.SetActive(!hideOrNot); }
}