using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterListener : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		if(Camera.main != null)
        {
            //カメラが存在すれば、その座標に自らを移動する
            transform.position = Camera.main.transform.position;

            //※MasterListenerは独立シーンAudioに存在するため子オブジェクトにして追従することは出来ない
        }
	}
}
