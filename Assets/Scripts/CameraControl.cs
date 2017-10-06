using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraControl : MonoBehaviour, IDragHandler {

    [SerializeField]
    private float camSizeMin = 2f, camSizeMax = 5f; //拡大・縮小の極限値
    [SerializeField]
    private float zoomRate = 1f;    //拡大率(1cmあたり)
    [SerializeField]
    private float moveRate = 1f;    //移動速度(1cmあたり)

    private Touch[] pastTouch = new Touch[2];  //1フレーム前のタッチ情報

    //このコンポーネントは原則cameraにしか使わない唯一のものだからstaticで重複防止は不要
    private FieldBoard board;
    private FieldBoardBuilder builder;
    private float topLimit, bottomLimit, leftLimit, rightLimit;

    //1cmあたりのピクセル数を取得
    private float pixelPerCm;

    // Use this for initialization
    void Start ()
    {
        board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();
        builder = board.gameObject.GetComponent<FieldBoardBuilder>();

        //枠のはじっこを取得
        topLimit = board.MapPosToWorldPos(new Vector2(0, 0)).y;
        bottomLimit = board.MapPosToWorldPos(new Vector2(board.MapWidth, board.MapHeight)).y;
        leftLimit = board.MapPosToWorldPos(new Vector2(board.MapWidth, 0)).x;
        rightLimit = board.MapPosToWorldPos(new Vector2(0, board.MapHeight)).x;

        //dpiを取得
        pixelPerCm = Screen.dpi / 2.54f;
    }
	
	// Update is called once per frame
	void Update () {
		
        //ビルダーの選択がない時だけ実行
        if(builder.SelectedFacility == null)
        {
            //if (Input.touchCount == 1)
            //{
            //    //スライド処理
            //    Touch touchPoint = Input.GetTouch(0);

            //    if (touchPoint.phase != TouchPhase.Ended)
            //    {
            //        //カメラの座標差分ベクトルを作成
            //        Vector3 deltaVec = new Vector3(touchPoint.deltaPosition.x, touchPoint.deltaPosition.y, 0f);
            //        deltaVec = -deltaVec * moveRate / pixelPerCm;

            //        //カメラ移動
            //        Vector3 newCamPos = Camera.main.transform.position + deltaVec;

            //        //四隅移動制限処理
            //        if (newCamPos.y > topLimit) newCamPos.y = topLimit;
            //        if (newCamPos.y < bottomLimit) newCamPos.y = bottomLimit;
            //        if (newCamPos.x < leftLimit) newCamPos.x = leftLimit;
            //        if (newCamPos.x > rightLimit) newCamPos.x = rightLimit;

            //        //座標に適用
            //        Camera.main.transform.position = newCamPos;
            //    }

            //    pastTouch[0] = touchPoint;
            //}
            //else if (Input.touchCount >= 2)
            //{
            //    //ピンチイン・ピンチアウト処理
            //    Touch t1 = Input.GetTouch(0);
            //    Touch t2 = Input.GetTouch(1);

            //    if (t1.phase != TouchPhase.Began && t2.phase != TouchPhase.Began)
            //    {
            //        //タッチ距離の差分を取得
            //        float deltaDist = Vector2.Distance(pastTouch[0].position, pastTouch[1].position)
            //                            - Vector2.Distance(t1.position, t2.position);
            //        //拡大
            //        Camera.main.orthographicSize += deltaDist * zoomRate / pixelPerCm;
            //        //最大最小値と照らし合わせる
            //        if (Camera.main.orthographicSize > camSizeMax) Camera.main.orthographicSize = camSizeMax;
            //        if (Camera.main.orthographicSize < camSizeMin) Camera.main.orthographicSize = camSizeMin;
            //    }

            //    pastTouch[0] = t1;
            //    pastTouch[1] = t2;
            //}
        }
    }
    
    public void OnDrag(PointerEventData eventData)
    {

        //ビルダーの選択がない時だけ実行
        if (builder.SelectedFacility == null)
        {
            //タッチかクリックかの判別
            if (Input.touchCount == 0)
            {
                //クリック処理（マウス）

                if (eventData.button == PointerEventData.InputButton.Right)
                {
                    //右クリックされていたらスライド処理

                    //カメラの座標差分ベクトルを作成
                    Vector3 deltaVec = new Vector3(eventData.delta.x, eventData.delta.y, 0f);
                    deltaVec = -deltaVec * moveRate / pixelPerCm;

                    //カメラ移動
                    Vector3 newCamPos = Camera.main.transform.position + deltaVec;

                    //四隅移動制限処理
                    if (newCamPos.y > topLimit) newCamPos.y = topLimit;
                    if (newCamPos.y < bottomLimit) newCamPos.y = bottomLimit;
                    if (newCamPos.x < leftLimit) newCamPos.x = leftLimit;
                    if (newCamPos.x > rightLimit) newCamPos.x = rightLimit;

                    //座標に適用
                    Camera.main.transform.position = newCamPos;
                }
            }
            else
            {
                //タッチ処理（スマートフォン）

                if (Input.touchCount == 1)
                {
                    //スライド処理
                    Touch touchPoint = Input.GetTouch(0);

                    if (touchPoint.phase != TouchPhase.Ended)
                    {
                        //カメラの座標差分ベクトルを作成
                        Vector3 deltaVec = new Vector3(touchPoint.deltaPosition.x, touchPoint.deltaPosition.y, 0f);
                        deltaVec = -deltaVec * moveRate / pixelPerCm;

                        //カメラ移動
                        Vector3 newCamPos = Camera.main.transform.position + deltaVec;

                        //四隅移動制限処理
                        if (newCamPos.y > topLimit) newCamPos.y = topLimit;
                        if (newCamPos.y < bottomLimit) newCamPos.y = bottomLimit;
                        if (newCamPos.x < leftLimit) newCamPos.x = leftLimit;
                        if (newCamPos.x > rightLimit) newCamPos.x = rightLimit;

                        //座標に適用
                        Camera.main.transform.position = newCamPos;
                    }

                    pastTouch[0] = touchPoint;
                }
                else if (Input.touchCount >= 2)
                {
                    //ピンチイン・ピンチアウト処理
                    Touch t1 = Input.GetTouch(0);
                    Touch t2 = Input.GetTouch(1);

                    if (t1.phase != TouchPhase.Began && t2.phase != TouchPhase.Began)
                    {
                        //タッチ距離の差分を取得
                        float deltaDist = Vector2.Distance(pastTouch[0].position, pastTouch[1].position)
                                            - Vector2.Distance(t1.position, t2.position);
                        //拡大
                        Camera.main.orthographicSize += deltaDist * zoomRate / pixelPerCm;
                        //最大最小値と照らし合わせる
                        if (Camera.main.orthographicSize > camSizeMax) Camera.main.orthographicSize = camSizeMax;
                        if (Camera.main.orthographicSize < camSizeMin) Camera.main.orthographicSize = camSizeMin;
                    }

                    pastTouch[0] = t1;
                    pastTouch[1] = t2;
                }
            }
        }
    }
}
