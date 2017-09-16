using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class Tutorial1 : MonoBehaviour {

    private static int state = 0;

    private static FieldBoard board;

	// Use this for initialization
	void Start ()
    {
        board = GameObject.FindGameObjectWithTag("FieldBoard").GetComponent<FieldBoard>();

        TutorialManager.PutMessage("はじめまして！", 0);
        TutorialManager.PutMessage("本日はこのゲームをプレイして頂き、ありがとうございます！\nまだまだ完成とは言えませんが、ぜひお楽しみください。", 1);
        TutorialManager.PutMessage("それでは、簡単な解説をさせて頂きます！", 2);

        TutorialManager.PutMessage("まず、左上の日付の書いてあるボタンを押してください\nこれで時間が「進む/止まる」を切り替えられます。", 0);
    }
	
	// Update is called once per frame
	void Update () {

        if(state != 100 && board.Money < 0)
        {
            FieldTimeManager.ToggleClockEnabledStatic();
            TutorialManager.PutMessage("...ありゃ～...", 4);
            TutorialManager.PutMessage("どうやら資金が底をついちゃったみたいですね...", 4);
            TutorialManager.PutMessage("宇宙生物ちゃんに逃げられちゃったか、はたまた建物の建て過ぎか...", 4);
            TutorialManager.PutMessage("くよくよしても仕方ないですね！とりあえずルールなのでゲームオーバーですが、次こそは！応援しています！", 1);
            
            state = 100;
        }

        switch (state)
        {
            case 0:
                if(FieldTimeManager.FieldTime >= new VirtualClock(2000, 1, 1, 6, 30, 0, false))
                {
                    FieldTimeManager.ToggleClockEnabledStatic();
                    TutorialManager.PutMessage("そんな感じです！", 1);
                    TutorialManager.PutMessage("続いて、左下にある青いボタンが「建築メニュー」です。\n押すといろんな建物のパネルが出てきます。", 2);
                    TutorialManager.PutMessage("今回は「木」をひとつ建ててみましょう！木は特別な効果はありませんが、木が生えている場所はあらゆる生き物が通り抜けできません。", 0);
                    TutorialManager.PutMessage("マップ上を1本指でぐりぐりすれば移動、2本指でうにうにすれば拡大縮小ができます。好きな所に建ててください。", 0);
                    TutorialManager.PutMessage("建てると相応の資金を消費します。\nうまく建てられたら、再度時間を進めてみてください。", 0);
                    state++;
                }
                break;
            case 1:
                if (FieldTimeManager.FieldTime >= new VirtualClock(2000, 1, 1, 7, 0, 0, false))
                {
                    FieldTimeManager.ToggleClockEnabledStatic();
                    TutorialManager.PutMessage("さて、マップ内に2つ、ぷよぷよしたものが見えますね。", 0);
                    TutorialManager.PutMessage("あれが私たちの飼育している「宇宙生物」です。", 2);
                    TutorialManager.PutMessage("とてもかわいい子たちなのですが、夜な夜な逃げ出してしまう困ったちゃんです", 3);
                    state++;
                }
                break;
            case 2:
                if (FieldTimeManager.FieldTime >= new VirtualClock(2000, 1, 1, 8, 0, 0, false))
                {
                    FieldTimeManager.ToggleClockEnabledStatic();
                    TutorialManager.PutMessage("画面内をうようよしている黒い人影が見えますか？あれが宇宙生物たちを見に来たお客様です。", 1);
                    TutorialManager.PutMessage("お客様はショップやレストランなどの特定の施設の近くにいると、たまにお金を使ってくれます。", 2);
                    TutorialManager.PutMessage("つまり、「お客様が通りやすい所にショップやレストランを置く」と、たくさんお金が稼げるのです。", 4);
                    TutorialManager.PutMessage("やってみてください！", 1);
                    state++;
                }
                break;
            case 3:
                if (FieldTimeManager.FieldTime >= new VirtualClock(2000, 1, 1, 18, 0, 0, false))
                {
                    FieldTimeManager.ToggleClockEnabledStatic();
                    TutorialManager.PutMessage("お疲れ様でした！今日の営業は終了です", 1);
                    TutorialManager.PutMessage("...なんですが、この仕事はこれからが本番みたいなとこあります。", 3);
                    TutorialManager.PutMessage("そう、彼らが逃げ出すのを阻止しなければならないのです。", 2);
                    TutorialManager.PutMessage("とりあえずの対策として、小型タレットが用意されています。\n最初のうちはこれで十分でしょう。「タレット」を2,3個、入場門の近くに置いてください。", 1);
                    TutorialManager.PutMessage("逃したらうちの予算で弁償ですからね！気をつけてください！", 3);

                    state++;
                }
                break;
            case 4:
                if (FieldTimeManager.FieldTime >= new VirtualClock(2000, 1, 1, 20, 0, 0, false))
                {
                    FieldTimeManager.ToggleClockEnabledStatic();
                    TutorialManager.PutMessage("そろそろ動き出しますよ？準備はいいですか？", 3);
                    state++;
                }
                break;

            case 5:
                if (FieldTimeManager.FieldTime >= new VirtualClock(2000, 1, 2, 6, 0, 0, false))
                {
                    FieldTimeManager.ToggleClockEnabledStatic();
                    TutorialManager.PutMessage("...ふぅ、どうやら無事やり過ごしたみたいですね\n朝になればもう逃げ出したりしないので安心です。", 2);
                    TutorialManager.PutMessage("と、こんな感じで進めていくのがこのゲームです。今後は、もっとたくさんの種類の生物を出したり、ストーリーやゲームモードを作っていく所存です。", 1);
                    
                    state++;
                }
                break;
            case 6:
                if (FieldTimeManager.FieldTime >= new VirtualClock(2000, 1, 2, 12, 0, 0, false))
                {
                    FieldTimeManager.ToggleClockEnabledStatic();
                    TutorialManager.PutMessage("...............", 4);
                    TutorialManager.PutMessage("あ、終わりです。もう何もありませんよ？", 2);
                    TutorialManager.PutMessage("...ええ。お察しの通りですが、わりと進捗ダメでしたこれ", 1);
                    TutorialManager.PutMessage("何がいけなかったんでしょうかね～...やっぱり直前にSteamのセール漁ったのが悪k", 1);
                    TutorialManager.PutMessage("(どこかから降ってきたタライ)グワッシャァァァン", 5);
                    TutorialManager.PutMessage("(......ぱたん)", 6);
                    TutorialManager.PutMessage("プレイありがとうございました。現時点での改善点など頂ければ幸いです。", 6);




                    state++;
                }
                break;
        }
	}

    public static void FinishTutorial()
    {
        switch (state)
        {
            case 0:
                
                break;
            case 1:
               
                break;
            case 2:
                FieldTimeManager.ToggleClockEnabledStatic();
                break;
            case 3:
                FieldTimeManager.ToggleClockEnabledStatic();
                break;
            case 4:
                
                break;

            case 5:
                
                break;
            case 6:
                
                break;
            case 100:
                state = 0;
                SceneManager.LoadScene("Title");
                break;
        }
    }
}
