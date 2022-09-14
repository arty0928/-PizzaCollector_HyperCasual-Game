using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    
    public static GameManager I;
    public Transform dirrectArrow;//ȭ��ǥ
    public int dirrectArrowSpeed;//ȭ��ǥ �ӵ�

    public GameObject menuCam;
    public GameObject gameCam;
    public PlayerController player; //player ��ũ��Ʈ�� �ޱ�
    //public Enemy boss; //�� ��ũ��Ʈ�� �ޱ�
    //public EnemyControllerAngle enemyScript;

    public int stage= 1;
    public float playTime;//���ӽð�

    public int EnemyCnt1; //enemyB + enemyC + enemyD
    public int EnemyCnt2; //Plant

    //UI
    public GameObject manuPanel;
    public GameObject gamePanel;
    public GameObject LevelUpPanel;
    public GameObject joystick;
    //public Text maxScoreTxt; //manuPanel

    //gamePanel
    public Text StageTxt; //stage �ܰ�
    public Text ItemTxt;  //���� ������ 
    public Text Enemy1Txt; //������ ���� enemyB + enemyC + enemyD
    public Text Enemy2Txt; //������ ���� Plant


    // public Text SaveTxt;  //��� ��
    public Text playTimeTxt;  //���� �ð�
    //public bool time;

    //������
    public Vector3 itemPos;
    public GameObject ItemPrefab;
    public Transform target;

    //enemy
    public Transform[] EnemyPoint;
    public Vector3 EnemyPos;
    public GameObject EnemyB_Prefab;
    public GameObject EnemyC_Prefab; //SizeUp

    public GameObject EnemyD_Prefab; //double RayCast
    //KillingPlant
    public GameObject EnemyPlant0_Prefab;
    public GameObject EnemyPlant1_Prefab;
    public GameObject EnemyPlant2_Prefab;
    public GameObject EnemyPlant3_Prefab;
    //Transform[] EnemyPlants;

    //HideZonde
    public GameObject HideZone_Prefab;
    private int HideZoneCallCount;

    //TiemItem
    public GameObject TimeItem_Prefab;

    //levelUp�� ���� enemy ��ȭ
    //public int count = 0;

    public bool isPlay; //���� �ο�� �ִ°�
    //Game Over
    public bool isDead; //�׾��°�
    public bool LevlSet; //level ������ �Ϸ�Ǿ��°�
    public GameObject overPanel;

    //audio
    public AudioSource GameOverSound;
    public bool alreadyDead = false;
    public AudioSource bgm;

    //play
    public int haveToEatItem;

    public bool LevelUpCheck = true;

    private void Awake()
    {
        I = this;
        Application.targetFrameRate = 60;
    }

    public void GameStart()
    {
        //I = this;
        stage = 1;
        Debug.Log("GameStart");
        Debug.Log("isPlay: " + isPlay);
        Debug.Log("isDead: " + isDead);

        //�޴� ���� ������Ʈ ��Ȱ��ȭ, ���� ���� ������Ʈ Ȱ��ȭ
        menuCam.SetActive(false);
        gameCam.SetActive(true);

        manuPanel.SetActive(false);
        gamePanel.SetActive(true);
        joystick.SetActive(true);

        player.gameObject.SetActive(true);
        bgm.Play();
        StageStart();
    }

    public void StageStart()
    {
        Debug.Log("StageStart()");
        Debug.Log("isPlay: " + isPlay);
        Debug.Log("isDead: " + isDead);
        isPlay = true;
        isDead = false;
        LevlSet = true;

        Debug.Log("isPlay: " + isPlay);
        Debug.Log("isDead: " + isDead);
        playTime = 0;

}
    private void HideLevelUpPanel()
    {
        Debug.Log("LevelUpPanel setactive false");
        LevelUpPanel.SetActive(false);
        isPlay = true;
    }
    public void StageEnd()
    {

        Debug.Log("StageEnd");
        //isPlay = false;
        //GameOverSound.Play();
        Debug.Log("LevelUpPanel setactive");

        isPlay = false;
        Debug.Log("isPlay: " + isPlay);
        LevelUpPanel.SetActive(true);
        Invoke("HideLevelUpPanel", 0.5f);

        Debug.Log("isPlay: " + isPlay);

        alreadyDead = false;

        playTime = 0;
        player.lunchitem = 0;


        stage++;
        ItemInStage();

        var Enemy = GameObject.FindGameObjectsWithTag("Enemy");
        for (var i = 0; i < Enemy.Length; i++)
        {
            Destroy(Enemy[i]);
        }

        EnemyToPut();



        if (stage > 5)
        {
            var KillingPlant = GameObject.FindGameObjectsWithTag("KillingPlant");
            for (var i = 0; i < KillingPlant.Length; i++)
            {
                Destroy(KillingPlant[i]);
            }

            if (stage > 7)
            {
                var HideZoneExist = GameObject.FindGameObjectsWithTag("HideZone");
                for (var j = 0; j < HideZoneExist.Length; j++)
                {
                    Destroy(HideZoneExist[j]);
                }
                HideZoneToPut();
            }
            PlantToPut();
        }
        else if (stage > 15)
        {
            var TimeItems = GameObject.FindGameObjectsWithTag("TimeItem");
            for (var j = 0; j < TimeItems.Length; j++)
            {
                Destroy(TimeItems[j]);
            }
            TimeItemToPut();
        }


        Debug.Log("playerPositionReset");
        player.transform.position = Vector3.zero;
        Debug.Log("LevelUp: " + stage);
        Debug.Log("player.lunchitem: " + player.lunchitem);
        Debug.Log("=========================================");
        Debug.Log("stage: " + stage);
        Debug.Log("player.lunchitem: " + player.lunchitem);
        LevlSet = true;
    }

    public void GameOver()
    {

        if (isDead == true)
        {
            bgm.Stop();
            //GameOverSound.Play();
            Debug.Log("Game Over");
            Debug.Log("isPlay: " + isPlay);
            Debug.Log("isDead: " + isDead);
            //isDead = true;
            OnDie();

            //isPlay = false;

            isPlay = true;
            isDead = true;
            LevlSet = false;
            Debug.Log("after Game Over");
            Debug.Log("isPlay: " + isPlay);
            Debug.Log("isDead: " + isDead);
            gamePanel.SetActive(false);
            overPanel.SetActive(true);
            stage = 0;

        }

        EnemyCnt1 = 0;
        EnemyCnt2 = 0;
        //count = 0;
        HideZoneCallCount = 0;


    }

    public void OnDie()
    {
        //isDead = true;
        //isDead = true;
        //isPlay = false;
    }

    public void ReStart()
    {
        Debug.Log("ReStart");

        SceneManager.LoadScene(0);

        //count = 0;
    }
    public void ItemInStage()
    {
        if (stage == 1)
            haveToEatItem = 1;

        else if (2 <= stage && stage <= 5)
            haveToEatItem = 2;

        else if (6 <= stage && stage <= 10)
            haveToEatItem = 3;

        else if (11 <= stage && stage <= 14)
            haveToEatItem = 4;

        else if (15 <= stage && stage <= 18)
            haveToEatItem = 5;

        else if (19 <= stage && stage <= 20)
            haveToEatItem = 6;

        else if (21<= stage)
            haveToEatItem = 7;
    }

    private void Start()
    {
        
        ItemToPut();
        EnemyToPut();

    }

    public void ItemToPut()
    {

        var items = GameObject.FindGameObjectsWithTag("LunchToPut").Select(ItemToPut => ItemToPut.transform.position).ToArray();
        items = items.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
        itemPos = items[0];

        //Debug.Log("ItemToPut");
        target = Instantiate(ItemPrefab, new Vector3(itemPos.x, itemPos.y, itemPos.z), transform.rotation).transform;
        Debug.Log("stage: " + stage);


        Debug.Log("After_ItemToput");

    }

    public void EnemyToPut()
    {
        Debug.Log("EnemyToPut");
        var Enemies = GameObject.FindGameObjectsWithTag("LunchToPut").Select(EnemyToPut => EnemyToPut.transform.position).ToArray();
        Enemies = Enemies.OrderBy(Enemy => Random.Range(-1.0f, 1.0f)).ToArray();

        PlantToPut();
        HideZoneToPut();
        TimeItemToPut();

        //Instantiate(EnemyB_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
        //Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
        //Instantiate(EnemyD_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
        if(stage ==0 || stage ==1){
            EnemyCnt1 = 2;
            for(var i=0; i<2;i++){
                Instantiate(EnemyB_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
            }
        }
        else if(2<=stage && stage <=8){
            for (var i = 0; i < 3; i++)
            {
                Instantiate(EnemyB_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
            }

            if(stage == 2){
                EnemyCnt1 = 3;
            }

            else if(stage == 3){
                EnemyCnt1 = 3+1;
                Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
            }

            else if (4 <= stage && stage <= 5)
            {
                EnemyCnt1 = 3 + 2;
                for (var i = 0; i < 2; i++)
                {
                    Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                }
                
            }

            else if (6 <= stage && stage <= 8)
            {
                EnemyCnt1 = 3 + 3;
                for (var i = 0; i < 3; i++)
                {
                    Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                }

            }
        }
        else if (9 <= stage && stage <= 11)
        {
            for (var i = 0; i < 4; i++)
            {
                Instantiate(EnemyB_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
            }

            if(9 <= stage && stage <= 10)
            {
                EnemyCnt1 = 4+3;
                for (var i = 0; i < 3; i++)
                {
                    Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                }
            }

            else if(stage== 11){
                EnemyCnt1 = 4 + 4;
                for (var i = 0; i < 4; i++)
                {
                    Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                }
            }
        }    
        else if (12 <= stage && stage <= 14)
        {
            for (var i = 0; i < 5; i++)
            {
                Instantiate(EnemyB_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
            }
            if(stage == 12){
                EnemyCnt1 = 5 + 4;
                    for (var i = 0; i < 4; i++)
                    {
                        Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                    }
            }
            else if(13<= stage && stage <=14){
                EnemyCnt1 = 5 + 5;
                for (var i = 0; i < 5; i++)
                {
                    Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                }
            }
        }
        else if (15 <= stage && stage <= 17)
        {
            for (var i = 0; i < 6; i++)
            {
            Instantiate(EnemyB_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
            }
            if(stage == 15){
                EnemyCnt1 = 6 + 5 + 1;
                    for (var i = 0; i < 5; i++)
                    {
                        Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                    }
                    Instantiate(EnemyD_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);

            }
            else if (16 <= stage && stage <= 17){
                EnemyCnt1 = 6 + 6 + 1;
                for (var i = 0; i < 6; i++)
                {
                    Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                }
                Instantiate(EnemyD_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);

                }


        }
        else if (18 <= stage && stage <= 20)
        {
            for (var i = 0; i < 7; i++)
            {
                Instantiate(EnemyB_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
            }

            if(stage == 18){
                EnemyCnt1 = 7 + 6 + 1;
                for (var i = 0; i < 6; i++)
                {
                    Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                }
                Instantiate(EnemyD_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);

            }
            else if(19 <= stage && stage <= 20)
            {
                EnemyCnt1 = 7 + 7 + 1;
                for (var i = 0; i < 7; i++)
                {
                    Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                }
                Instantiate(EnemyD_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);

            }
        }
        else if(stage >=21)
        {
            EnemyCnt1 = 8 + 7 + 1;
            for (var i = 0; i < 8; i++)
            {
                Instantiate(EnemyB_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                Instantiate(EnemyC_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);
                Instantiate(EnemyD_Prefab, new Vector3(Enemies[Random.Range(0, Enemies.Length)].x, 0, Enemies[Random.Range(0, Enemies.Length)].z), transform.rotation);

            }
        }
    
    }

    //Level6����
    public void PlantToPut()
    {
        if (stage >= 5)
        {
            var Plants = GameObject.FindGameObjectsWithTag("KillingPlantToPut").Select(PlantToPut => PlantToPut.transform.position).ToArray();
            Plants = Plants.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

            if (stage == 5)
            {
                EnemyCnt2 = 1;
                int count = Random.Range(0, 4);
                switch (count)
                {
                    case 0:
                        Instantiate(EnemyPlant0_Prefab, Plants[0], transform.rotation);
                        break;
                    case 1:
                        Instantiate(EnemyPlant1_Prefab, Plants[0], transform.rotation);
                        break;
                    case 2:
                        Instantiate(EnemyPlant2_Prefab, Plants[0], transform.rotation);
                        break;
                    case 3:
                        Instantiate(EnemyPlant3_Prefab, Plants[0], transform.rotation);
                        break;
                }

            }
            if (stage == 6)
            {
                EnemyCnt2 = 2;

                for(var i =0; i<2; i++){

                    int count = Random.Range(0, 4);
                    switch (count)
                    {
                        case 0:
                            Instantiate(EnemyPlant0_Prefab, Plants[0], transform.rotation);
                            break;
                        case 1:
                            Instantiate(EnemyPlant1_Prefab, Plants[0], transform.rotation);
                            break;
                        case 2:
                            Instantiate(EnemyPlant2_Prefab, Plants[0], transform.rotation);
                            break;
                        case 3:
                            Instantiate(EnemyPlant3_Prefab, Plants[0], transform.rotation);
                            break;
                    }
                }
                

            }
            else if (7 <= stage && stage <= 9)
            {
                EnemyCnt2 = 3;

                for (var i = 0; i < 3; i++)
                {
                    int count = Random.Range(0, 4);
                    switch (count)
                    {
                        case 0:
                            Instantiate(EnemyPlant0_Prefab, Plants[i], transform.rotation);
                            break;
                        case 1:
                            Instantiate(EnemyPlant1_Prefab, Plants[i], transform.rotation);
                            break;
                        case 2:
                            Instantiate(EnemyPlant2_Prefab, Plants[i], transform.rotation);
                            break;
                        case 3:
                            Instantiate(EnemyPlant3_Prefab, Plants[i], transform.rotation);
                            break;
                    }
                }
            }

            else if (10 <= stage && stage <= 13)
            {
                EnemyCnt2 = 4;
                for (var i = 0; i < 4; i++)
                {
                    int count = Random.Range(0, 4);
                    switch (count)
                    {
                        case 0:
                            Instantiate(EnemyPlant0_Prefab, Plants[i], transform.rotation);
                            break;
                        case 1:
                            Instantiate(EnemyPlant1_Prefab, Plants[i], transform.rotation);
                            break;
                        case 2:
                            Instantiate(EnemyPlant2_Prefab, Plants[i], transform.rotation);
                            break;
                        case 3:
                            Instantiate(EnemyPlant3_Prefab, Plants[i], transform.rotation);
                            break;
                    }
                }
            }

            else if (14 <= stage && stage <= 16)
            {
                EnemyCnt2 = 5;
                for (var i = 0; i < 5; i++)
                {
                    int count = Random.Range(0, 4);
                    switch (count)
                    {
                        case 0:
                            Instantiate(EnemyPlant0_Prefab, Plants[i], transform.rotation);
                            break;
                        case 1:
                            Instantiate(EnemyPlant1_Prefab, Plants[i], transform.rotation);
                            break;
                        case 2:
                            Instantiate(EnemyPlant2_Prefab, Plants[i], transform.rotation);
                            break;
                        case 3:
                            Instantiate(EnemyPlant3_Prefab, Plants[i], transform.rotation);
                            break;
                    }
                }
            }

            else if (17 <= stage && stage <= 19)
            {
                EnemyCnt2 = 6;
                for (var i = 0; i < 6; i++)
                {
                    int count = Random.Range(0, 4);
                    switch (count)
                    {
                        case 0:
                            Instantiate(EnemyPlant0_Prefab, Plants[i], transform.rotation);
                            break;
                        case 1:
                            Instantiate(EnemyPlant1_Prefab, Plants[i], transform.rotation);
                            break;
                        case 2:
                            Instantiate(EnemyPlant2_Prefab, Plants[i], transform.rotation);
                            break;
                        case 3:
                            Instantiate(EnemyPlant3_Prefab, Plants[i], transform.rotation);
                            break;
                    }
                }
            }
            else
            {
                EnemyCnt2 = 7;
                for (var i = 0; i < 7; i++)
                {
                    int count = Random.Range(0, 4);
                    switch (count)
                    {
                        case 0:
                            Instantiate(EnemyPlant0_Prefab, Plants[i], transform.rotation);
                            break;
                        case 1:
                            Instantiate(EnemyPlant1_Prefab, Plants[i], transform.rotation);
                            break;
                        case 2:
                            Instantiate(EnemyPlant2_Prefab, Plants[i], transform.rotation);
                            break;
                        case 3:
                            Instantiate(EnemyPlant3_Prefab, Plants[i], transform.rotation);
                            break;
                    }
                }
            }


        }

    }

    public void HideZoneToPut()
    {
        if (stage >= 7)
        {
            var HideZone = GameObject.FindGameObjectsWithTag("HideZoneToPut").Select(HideZone => HideZone.transform.position).ToArray();
            HideZone = HideZone.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();

            
            if (7 <= stage && stage <= 9)
            {
                HideZoneCallCount = 1;
                for (var i = 0; i < 1; i++)
                {
                    Instantiate(HideZone_Prefab, HideZone[i], transform.rotation);
                }

            }

            else if (10 <= stage && stage <= 13)
            {
                HideZoneCallCount = 2;
                for (var i = 0; i < 2; i++)
                {
                    Instantiate(HideZone_Prefab, HideZone[i], transform.rotation);
                }

            }
            else
            {
                HideZoneCallCount = 3;
                for (var i = 0; i < 3; i++)
                {
                    Instantiate(HideZone_Prefab, HideZone[i], transform.rotation);
                }
            }

        }


    }

    public void TimeItemToPut()
    {

        if (stage >= 12)
        {
            var TimeItems = GameObject.FindGameObjectsWithTag("TimeToPut").Select(HideZone => HideZone.transform.position).ToArray();
            TimeItems = TimeItems.OrderBy(item => Random.Range(-1.0f, 1.0f)).ToArray();
            Debug.Log("TimeItems.length: "+TimeItems.Length);

            Debug.Log("TimeItemToPut");
            Instantiate(TimeItem_Prefab, TimeItems[0], transform.rotation);
        }

    }
    private void Update()
    {
        //ItemInStage();
        if (stage == 1)
            haveToEatItem = 1;

        else if (stage == 2 && stage == 3)
            haveToEatItem = 2;

        else if (4 <= stage && stage <= 7)
            haveToEatItem = 3;

        else if (8 <= stage && stage <= 10)
            haveToEatItem = 4;

        else if (10 <= stage && stage <= 15)
            haveToEatItem = 5;

        else if (stage > 15)
            haveToEatItem = 6;

        Debug.Log("stage: " + stage);
        Debug.Log("havetoeatItem: " + haveToEatItem);

        
        if (player.lunchitem >= haveToEatItem)
        {
            Debug.Log("havetoEatItem: " + haveToEatItem);
            Debug.Log("player.lunchItem: " + player.lunchitem);
            Debug.Log("Eat all Lunch at This Stage");
            player.lunchitem = 0;
            StageEnd();
        }

        


    }


    //Update()�� ���� �� ȣ��Ǵ� �����ֱ�
    private void LateUpdate()

    {
        if (isPlay == true)
        {
            playTime += Time.deltaTime;//���ѽð� 60�� ī��Ʈ�ٿ�

            //������ ����Ű�� ȭ��ǥ 
            Vector3 targetPosition = target.transform.position;
            targetPosition.y = transform.position.y;
            dirrectArrow.LookAt(targetPosition);
            dirrectArrow.transform.rotation = new Quaternion(0, dirrectArrow.transform.rotation.y, 0, dirrectArrow.transform.rotation.w);

        }
        Debug.Log("LateUpDate");
        
        {
            //Stage text
            StageTxt.text = "LEVEL " + stage;

        
            int hour = (int)(playTime / 3600);
            int min = (int)((playTime - hour * 3600) / 60);
            int sec = 60 - (int)(playTime % 60);

            playTimeTxt.text = string.Format("{0:00}", sec);

           
            if (playTime >= 60)
            {
                GameOver();
                Debug.Log("Play Time Over");
            }


            ItemInStage();
            ItemTxt.text = string.Format("{0:n0}", player.lunchitem) + "/" + string.Format("{0:n0}", (haveToEatItem)); 
            Enemy1Txt.text = "x" + string.Format("{0:n0}", EnemyCnt1); //enemyB + enemyC +enemyD

            Enemy2Txt.text = "x" + string.Format("{0:n0}", EnemyCnt2); 
        }

    }


}