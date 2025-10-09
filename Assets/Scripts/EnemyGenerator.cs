using UnityEngine;

public class EnemyGenerator : MonoBehaviour
{
    const float Lanewidth = 6.0f; //レーン幅

    public GameObject[] dangerPrefab; //生成される危険車のプレハブ

    public float minIntervalTime = 0.1f; //インターバルの最小
    public float maxIntervalTime = 1.0f; //インターバルの最大

    float timer; //時間経過を観測
    float posX; //危険車の出現X座標

    GameObject cam; //カメラオブジェクト

    //初期位置
    public Vector3 defaultPos = new Vector3(0, 3, 120);

    Vector3 diff;
    public float followSpeed = 8; //ジェネレーターの補間スピード

    int isSky; //

    void Start()
    {
        transform.position = defaultPos; //ジェネレーターの初期値
        cam = Camera.main.gameObject; //カメラのオブジェクト情報
        diff = transform.position - cam.transform.position; //最初の時点でのカメラとジェネレーターの位置の差

        //タイマーはランダムに決まる
        timer = Random.Range(minIntervalTime, maxIntervalTime + 1);
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.gameState != GameState.playing) return; //ステータスがplayingでなければ何もしない

        timer -= Time.deltaTime; //カウントダウン

        if (timer <= 0)
        { //0になったら
            DangerCreated(); //危険車の生成
            maxIntervalTime -= 0.1f; //生成の度に最大インターバルの間隔を短く
            maxIntervalTime = Mathf.Clamp(maxIntervalTime, 0.1f, .0f); //最小でも0.1f
            timer = Random.Range(minIntervalTime, maxIntervalTime + 1);
        }
    }

    //ジェネレーターがずっと追従してくるように
    private void FixedUpdate()
    {
        //線形補間を使って、ジェネレーターを目的の場所に動かす
        //Lerpメソッド(今の位置、ゴールとすべき位置、割合）
        transform.position = Vector3.Lerp(transform.position, cam.transform.position + diff, followSpeed * Time.deltaTime);
    }

    //危険車の生成メソッド
    void DangerCreated()
    {
        isSky = Random.Range(0, 2); //空中かどうかをランダム 0か1
        int rand = Random.Range(-2, 3);//レーン番号をランダムに取得
        posX = rand * Lanewidth; //レーン番号とレーン幅で座標を決める

        //いったん生成位置情報vの高さはEnemyGeneratorと同じ位置
        Vector3 v = new Vector3(posX, transform.position.y, transform.position.z);

        //もしisSkyが0なら空中座標
        if (isSky == 0) v.y = 1;

        //プレハブ化した危険車を、ジェネレーターのその時のZの位置に、危険車の向きそのままに生成する
        Instantiate(dangerPrefab[isSky], v, dangerPrefab[isSky].transform.rotation);
    }
}