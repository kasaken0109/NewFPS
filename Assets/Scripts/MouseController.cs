using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController: MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform character;    //キャラクターをInspectorウィンドウから選択してください
    [SerializeField]
    private Transform pivot;    //キャラクターの中心にある空のオブジェクトを選択してください

    void Start()
    {
        //エラーが起きないようにNullだった場合、それぞれ設定
        if (character == null)
            character = transform.parent;
        if (pivot == null)
            pivot = transform;
    }
    //カメラ上下移動の最大、最小角度です。Inspectorウィンドウから設定してください
    [Range(-0.999f, -0.5f)]
    public float maxYAngle = -0.5f;
    [Range(0.5f, 0.999f)]
    public float minYAngle = 0.5f;
    // Update is called once per frame
    void Update()
    {
        //マウスのX,Y軸がどれほど移動したかを取得します
        float X_Rotation = Input.GetAxis("Mouse X");
        float Y_Rotation = Input.GetAxis("Mouse Y");
        //Y軸を更新します（キャラクターを回転）取得したX軸の変更をキャラクターのY軸に反映します
        character.transform.Rotate(0, X_Rotation, 0);

        //次はY軸の設定です。
        float nowAngle = pivot.transform.localRotation.x;
        //最大値、または最小値を超えた場合、カメラをそれ以上動かない用にしています。
        //キャラクターの中身が見えたり、カメラが一回転しないようにするのを防ぎます。
        if (-Y_Rotation != 0)
        {
            if (0 < Y_Rotation)
            {
                if(minYAngle >= nowAngle) 
                {
                    //pivot.transform.Rotate(-Y_Rotation, 0, 0);
                    Camera.main.transform.LookAt(pivot, Vector3.up);
                    Camera.main.transform.position += new Vector3(0, -Y_Rotation * Time.deltaTime * 3, 0);
                }
                //pivot.transform.Rotate(-Y_Rotation, 0, 0);
            }
            else
            {
                if (nowAngle >= maxYAngle)
                {
                    Camera.main.transform.LookAt(pivot, Vector3.up);
                    Camera.main.transform.position += new Vector3(0, -Y_Rotation * Time.deltaTime * 3, 0);
                }
                //pivot.transform.Rotate(-Y_Rotation, 0, 0);
            }
        }
       // pivot.transform.Rotate(-Y_Rotation, 0, 0);

    }
}
