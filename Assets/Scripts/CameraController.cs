using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 diff; //ターゲットとの距離のさ
    GameObject player;　//ターゲットとなるプレイヤー情報

    public float followSpeed = 8;　//カメラの補完スピード

    //カメラの初期位置
    public Vector3 defaultPos = new Vector3(0,6,6);
    public Vector3 defaultRotate = new Vector3(12,0,0);

    void Start()
    {
        //カメラを変数で決めた初期位置・角度にする
        transform.position = defaultPos;
        transform.rotation = Quaternion.Euler(defaultRotate);　//RotationはQuaternion型

        //プレイヤー情報の取得
        player = GameObject.FindGameObjectWithTag("Player");

        //プレイヤーとカメラの距離を取得しておく
        diff = player.transform.position-transform.position;
    }

    void LateUpdate() //Updateより後に働くもの
    {
        //プレイヤーが見つからなければ何もしない
        if (player == null) return;
        //線形補間を使って、カメラを目的の場所に動かす
        //Lerpメソッド
        transform.position = Vector3.Lerp(transform.position, player.transform.position - diff, followSpeed * Time.deltaTime);
    }
}
