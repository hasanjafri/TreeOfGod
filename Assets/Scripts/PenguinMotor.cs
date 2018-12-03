using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenguinMotor : MonoBehaviour {

  private const float LANE_DISTANCE = 3.0f;
  private const float TURN_SPEED = 0.05f;

  private bool isRunning = false;

  //Movement
  private CharacterController controller;
  private Animator myAnimator;
  private float jumpForce = 4.0f;
  private float gravity = 12.0f;
  private float verticalVelocity;
  private float speed = 7.0f;
  private int desiredLane = 1; // 0 = Left, 1 = Middle, 2 = Right

  private void Start()
  {
    controller = GetComponent<CharacterController>();
    myAnimator = GetComponent<Animator>();
  }

  private void Update()
  {
    if (!isRunning)
    {
      return;
    }

    if (MobileInput.Instance.SwipeLeft)
    {
      MoveLane(false);
    }

    if (MobileInput.Instance.SwipeRight)
    {
      MoveLane(true);
    }

    // Calculate where we should be in the future
    Vector3 targetPosition = transform.position.z * Vector3.forward;
    if (desiredLane == 0)
    {
      targetPosition += Vector3.left * LANE_DISTANCE;
    } else if(desiredLane == 2)
    {
      targetPosition += Vector3.right * LANE_DISTANCE;
    }

    //Calculate our move delta
    Vector3 moveVector = Vector3.zero;
    moveVector.x = (targetPosition - transform.position).normalized.x * speed;

    bool isGrounded = IsGrounded();
    myAnimator.SetBool("Grounded", isGrounded);

    if (isGrounded)
    {
      verticalVelocity = -0.01f;

      if (MobileInput.Instance.SwipeUp)
      {
        myAnimator.SetTrigger("Jumping");
        verticalVelocity = jumpForce;
      }
      else if(MobileInput.Instance.SwipeDown)
            {
                StartSliding();
            }
    }
    else
    {
      verticalVelocity -= (gravity * Time.deltaTime);

      if (MobileInput.Instance.SwipeDown)
      {
        verticalVelocity = -jumpForce;
      }
    }
    moveVector.y = verticalVelocity;
    moveVector.z = speed;

    //Move
    controller.Move(moveVector * Time.deltaTime);

    //Rotatation to where penguin is going
    Vector3 dir = controller.velocity;
    if (dir != Vector3.zero)
    {
      dir.y = 0;
      transform.forward = Vector3.Lerp(transform.forward, dir, TURN_SPEED);
    }
  }

    private void StartSliding()
    {
        myAnimator.SetBool("Sliding", true);
        controller.height /= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y / 2, controller.center.z);
    }

    private void StopSliding()
    {
        myAnimator.SetBool("Sliding", false);
        controller.height *= 2;
        controller.center = new Vector3(controller.center.x, controller.center.y * 2, controller.center.z);
    }

  private void MoveLane(bool goingRight)
  {
    desiredLane += (goingRight) ? 1 : -1;
    desiredLane = Mathf.Clamp(desiredLane, 0, 2);
  }

  private bool IsGrounded()
  {
    Ray groundRay = new Ray(new Vector3(controller.bounds.center.x, (controller.bounds.center.y - controller.bounds.extents.y) + 0.2f, controller.bounds.center.z), Vector3.down);
    return Physics.Raycast(groundRay, 0.2f + 0.1f);
  }

  public void StartRunning()
  {
        isRunning = true;
        myAnimator.SetTrigger("Running");
  }

    private void Crash()
    {
        myAnimator.SetTrigger("isDead");
        isRunning = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        switch(hit.gameObject.tag)
        {
            case "Obstacle":
                Crash();
            break;
        }
    }
}
