using System.Collections.Generic;
using UnityEngine;

public class StageGenerator : MonoBehaviour
{
    const int StageChipSize = 120; //生成したチップを配置するにあたってのチップの大きさ

    int currentChipIndex; //現在どのチップまで作ったか

    Transform player; //プレイヤーのTransform情報の取得

    public GameObject[] stageChips; //生成すべきオブジェクトを配列に格納

    public int startChipIndex; //チップ番号の開始
    public int preInstantiate; //余分に作っておく数

    //現在生成したオブジェクトの管理用
    public List<GameObject> generatedStageList = new List<GameObject>();

    void Start()
    {
        //Transformを獲得
        player = GameObject.FindGameObjectWithTag("Player").transform;

        currentChipIndex = startChipIndex - 1;
        UpdateStage(preInstantiate);
    }

    void Update()
    {
        if (player != null)
        {
            // キャラクターの位置から現在のステージチップのインデックスを計算
            int charaPositionIndex = (int)(player.position.z / StageChipSize);

            // 次のステージチップに入ったらステージの更新処理をおこなう
            if (charaPositionIndex + preInstantiate > currentChipIndex)
            {
                UpdateStage(charaPositionIndex + preInstantiate);
            }
        }
    }

    // 指定のIndexまでのステージチップを生成して、管理化に置く
    void UpdateStage(int toChipIndex)
    {
        if (toChipIndex <= currentChipIndex) return;

        // 指定のステージチップまでを作成 
        for (int i = currentChipIndex + 1; i <= toChipIndex; i++)
        {
            GameObject stageObject = GenerateStage(i);

            // 生成したステージチップを管理リストに追加
            generatedStageList.Add(stageObject);
        }

        // ステージ保持上限内になるまで古いステージを削除
        while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();

        currentChipIndex = toChipIndex;
    }

    // 指定のインデックス位置にStageオブジェクトをランダムに生成
    GameObject GenerateStage(int chipIndex)
    {
        int nextStageChip = Random.Range(0, stageChips.Length);

        GameObject stageObject = Instantiate(
            stageChips[nextStageChip],
            new Vector3(0, 0, chipIndex * StageChipSize),
            Quaternion.identity
        );

        return stageObject;
    }

    // 一番古いステージを削除
    void DestroyOldestStage()
    {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }
}