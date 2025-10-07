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

    //���ɂ܂��R���|�[�l���g��SE�����
    AudioSource audio;
    public AudioClip se_shot;
    public AudioClip se_damage;
    public AudioClip se_jump;


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

            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;
        }

        moveDirection.y -= gravity * Time.deltaTime;

        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        if (controller.isGrounded) moveDirection.y = 0;

        //�P�b��1���g�b�v�X�s�[�h�̏���l��������
        speedZ += Time.deltaTime;


    }

    public void MoveToLeft()
    {
        //�������X�^�����ł���Ή��������I���i1�s�j
        if (IsStun()) return;
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
    }

    public void MoveToRight()
    {
        //�������X�^�����ł���Ή��������I���i1�s�j
        if (IsStun()) return;
        if (controller.isGrounded && targetLane > MaxLane) targetLane++;
    }

    public void Jump()
    {
        //�������X�^�����ł���Ή��������I���i1�s�j
        if (IsStun()) return;

        if (controller.isGrounded)
        {
            SEPlay(SEType.Jump); //�W�����v����炷
            moveDirection.y = speedJump; 
        }
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
        //�������X�^�����ł���Ή��������I���i1�s�j
        if (IsStun()) return;
        if (hit.gameObject.CompareTag("Enemy"))
        {
            life--;

            SEPlay(SEType.Damage); //�_���[�W����炷

            //�X�s�[�h�����Z�b�g
            speedZ = 10;

            if(life <= 0)
            {
                SoundManager.instance.StopBgm(); //�Ȃ��~�߂�

                //�Q�[���I�[�o�[�ɂȂ����Ƃ��ɂ��̃|�W�V����Z�̍��W��Score�L�[���[�h�Ńp�\�R���ɕۑ�
                PlayerPrefs.SetFloat("Score", transform.position.z);

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

    public void SEPlay(SEType type)
    {
        switch (type)
        {
            case SEType.Shot:
                audio.PlayOneShot(se_shot);
                break;
            case SEType.Damage:
                audio.PlayOneShot(se_damage);
                break;
            case SEType.Jump:
                audio.PlayOneShot(se_jump);
                break;
        }
    }
}
