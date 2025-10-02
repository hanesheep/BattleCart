using UnityEngine;

public class PlayerController : MonoBehaviour
{
    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 6.0f;
    const float StunDuration = 0.5f;
    float recoverTime = 0.0f;

    public int life = 10;

    CharacterController controller;

    Vector3 moveDirection = Vector3.zero;
    int targetLane;

    public float gravity = 9.81f;

    public float speedZ = 10;
    public float accelerationZ = 8;

    public float speedX = 10;

    public float speedJump = 10;

    public GameObject body;
    public GameObject boms;

    
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        if(GameManager.gameState == GameState.playing)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) MoveToLeft();
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) MoveToRight();
            if (Input.GetKeyDown(KeyCode.Space)) Jump();
        }
        if (IsStun())
        {
            moveDirection.x = 0;
            moveDirection.z = 0;

            recoverTime -= Time.deltaTime;

            Blinking();
        }
        else
        {
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);
        }

        moveDirection.y -= gravity * Time.deltaTime;

        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        if (controller.isGrounded) moveDirection.y = 0;
    }

    public void MoveToLeft()
    {
        //もしもスタン中であれば何もせず終了（1行）
        if (IsStun()) return;
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
    }

    public void MoveToRight()
    {
        //もしもスタン中であれば何もせず終了（1行）
        if (IsStun()) return;
        if (controller.isGrounded && targetLane > MaxLane) targetLane++;
    }

    public void Jump()
    {
        //もしもスタン中であれば何もせず終了（1行）
        if (IsStun()) return;
        if (controller.isGrounded) moveDirection.y = speedJump;
    }

    public int Life()
    {
        return life;
    }

    bool IsStun()
    {
        bool stun = recoverTime > 0.0f || life <= 0;
        if (!stun) body.SetActive(true);
        return stun;

    }
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //もしもスタン中であれば何もせず終了（1行）
        if (IsStun()) return;
        if (hit.gameObject.CompareTag("Enemy"))
        {
            life--;

            if(life <= 0)
            {
                GameManager.gameState = GameState.gameover;
                Instantiate(boms, transform.position, Quaternion.identity);
                Destroy(gameObject, 0.5f);
            }

            recoverTime = StunDuration;
            Destroy(hit.gameObject);
        }
    }

    void Blinking()
    {
        float val = Mathf.Sin(Time.time * 50);
        if (val >= 0) body.SetActive(true);
        else body.SetActive(false);
    }
}
