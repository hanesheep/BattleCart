using System.Collections;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject bulletPrefab; //Bulletのプレハブ情報
    Transform player; //プレイヤーのTransform情報
    GameObject gate; //プレイヤーについているGateオブジェクトの情報
    public float shootSpeed = 100f; //投げた時の力
    public float upSpeed = 8f;　//投げた時の上向きの力

    bool possibleShoot; //ショット可能フラグ

    public int shotPower = 10;
    public int recoverySeconds = 3;
    Camera cam; //カメラ情報の取得

    PlayerController playerCnt; //他オブジェクトについているPlayerControllerスクリプト

    void Start()
    {
        playerCnt = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        Invoke("ShootEnabled", 0.5f);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        gate = player.Find("Gate").gameObject;
        cam = Camera.main;
    }

    void Update()
    {
        if (GameManager.gameState != GameState.playing) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (possibleShoot) Shot();
        }
    }

    void ShootEnabled()
    {
        possibleShoot = true;
    }

    void Shot()
    {
        if (player == null || shotPower <= 0) return;

        playerCnt.SEPlay(SEType.Shot); //ショット音を鳴らす

        GameObject obj = Instantiate(bulletPrefab, gate.transform.position, Quaternion.identity);
        Rigidbody rbody = obj.GetComponent<Rigidbody>();
        Vector3 v = new Vector3(cam.transform.forward.x * shootSpeed,cam.transform.forward.y + upSpeed,cam.transform.forward.z * shootSpeed);
        rbody.AddForce(v, ForceMode.Impulse);
        ConsumePower();
    }

    void ConsumePower()
    {
        shotPower--;
        StartCoroutine(RecoverPower());
    }

    IEnumerator RecoverPower()
    {
        yield return new WaitForSeconds(recoverySeconds);
        shotPower++;
    }
}
