using Unity.VisualScripting;
using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    const float Lanewidth = 6.0f; //レーン幅

    public GameObject[] dangerPrefab; //生成される危険車のプレハブ

    public float minIntervalTime = 0.1f; //インターバルの最小
    public float maxIntervalTime = 3.0f; //インターバルの最大

    float timer; //時間経過を観測
    float posX; //危険車の出現X座標

    GameObject cam; //カメラオブジェクト

    //初期位置
    public Vector3 defaultPos = new Vector3(0, 10, -60);

    Vector3 diff;
    public float followSpeed = 8; //ジェネレーターの補間スピード

    int isSky;

    void Start()
    {
        transform.position = defaultPos;
        cam = Camera.main.gameObject;
        diff = transform.position - cam.transform.position;
    }

    void Update()
    {
        if (GameManager.gameState != GameState.playing) return;

        timer -= Time.deltaTime;

        if (timer < 0)
        {
            DangerCreated();
            maxIntervalTime -= 0.1f;
            maxIntervalTime = Mathf.Clamp(maxIntervalTime, 0.1f, 3.0f);
            timer = Random.Range(minIntervalTime, maxIntervalTime + 1);
        }

        void FixedUpdate()
        {
            transform.position = Vector3.Lerp(transform.position, cam.transform.position + diff, followSpeed * Time.deltaTime);
        }

        void DangerCreated()
        {
            isSky = Random.Range(0, 2);
            int rand = Random.Range(-2, 3);
            posX = rand * Lanewidth;
        }

        Vector3 v = new Vector3(posX, transform.position.y, transform.position.z);

        if (isSky == 0) v.y = 1;

        Instantiate(dangerPrefab[isSky], v, dangerPrefab[isSky].transform.rotation);
    }
}
