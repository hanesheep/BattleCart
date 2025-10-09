using UnityEngine;

public class EnemyController : MonoBehaviour
{
    CharacterController controller;

    Vector3 moveDirection = Vector3.zero;

    public float gravity = 9.81f; //重力

    public float speedZ = -10; //前進方向のスピードの上限値
    public float accelerationZ = -8; //加速度

    public float deletePosY = -10f; //削除される基準のY座標値
    public bool useGravity; //重力に絞られるか空を飛ぶかのフラグ

    //GameObject camera; //カメラ情報

    void Start()
    {
        //カメラのオブジェクト情報の取得
        //camera = Camera.main.gameObject;

        controller = GetComponent<CharacterController>();

        //空中車
        if (useGravity)
        {
            //空中にいる車は時間経過で消滅
            Destroy(gameObject, 20);
        }
    }

    void Update()
    {
        //カメラより後ろにいったら消滅
        //if(transform.position.z < camera.transform.position.z)
        //{
        //    Destroy(gameObject);
        //}

        //ステージ外に落ちたら消滅
        if (transform.position.y <= deletePosY)
        {
            Destroy(gameObject);
            return;
        }

        //徐々に加速しZ方向に常に前進させる
        float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
        moveDirection.z = Mathf.Clamp(acceleratedZ, speedZ, 0);

        //地面を走るフラグ
        if (useGravity)
        {
            //重力分の力をフレーム追加
            moveDirection.y -= gravity * Time.deltaTime;
        }
        else //空を飛ぶフラグ
        {
            moveDirection.y = 0;
        }

        //移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後接地してたらY方向の速度はリセットする
        if (controller.isGrounded) moveDirection.y = 0;
    }
}