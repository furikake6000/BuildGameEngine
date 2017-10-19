using UnityEngine;
using UnityEngine.Assertions;

public class MasterListener : MonoBehaviour {

	// Update is called once per frame
	void Update () {
        Assert.IsNotNull(Camera.main, "Camera does not exist!");

        //カメラが存在すれば、その座標に自らを移動する
        if(Camera.main != null)transform.position = Camera.main.transform.position;

        //※MasterListenerは独立シーンAudioに存在するため子オブジェクトにして追従することは出来ない
    }
}
