using UnityEngine;
using UnityEngine.UI;

public class BuildDetailWindow : MonoBehaviour {

    [SerializeField]
    Text nameText;
    [SerializeField]
    Image thumbImage;
    [SerializeField]
    Text descText;
    [SerializeField]
    Text costText;

    static string fname, desc;
    static int cost;
    static Sprite thumb;

    static bool changedFlag;

	public static void ChangeSelectedFacility(Facility facility)
    {
        //更新予約
        fname = facility.FacilityName;
        thumb = facility.gameObject.GetComponent<SpriteRenderer>().sprite;
        cost = facility.Cost;
        desc = facility.Description;

        changedFlag = true;
    }

    private void Update()
    {
        //更新
        if (changedFlag)
        {
            changedFlag = false;

            nameText.text = fname;
            thumbImage.sprite = thumb;
            costText.text = cost.ToString();
            descText.text = desc;
        }
    }
}
