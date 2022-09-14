using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    
    public Transform m_tr;

    public float distance = 10.0f;

    
    public RaycastHit hit;

    public LayerMask m_layerMask = -1;
    public RaycastHit[] hits;
    public int checkWall = 0;


    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;
    //[SerializeField] private Animator _animator;

    [SerializeField] private float _moveSpeed;
    
    public int stageNum;

    public int lunchitem =0;
    public int timeItem;
    public int saveItem;

    public int maxItem;
    public int maxTimeItem;
    public int maxSaveItem;

    Text lunchCount;
    public static int lunchAmount;
    
    public Transform[] ItemPoint;

    //Audio
    public AudioSource CoinSound;
    public AudioSource GameOverSound;

    //animator
    public Animator anim;

    //HideZone
    public bool isHide= false;


    Vector3 LookDir;
    private bool isWall = false;
    
    private void Start()
    {
        lunchitem = 0;
        m_tr = GetComponent<Transform>();

    }

    private void LateUpdate()
    {
        LookDir = _joystick.Vertical * Vector3.forward + _joystick.Horizontal * Vector3.right;
        transform.rotation = Quaternion.LookRotation(LookDir);


        Ray ray = new Ray();

        ray.origin = m_tr.position;

        ray.direction = m_tr.forward;


        Vector3 rayforward = new Vector3(0,0,1);
        Vector3 rayright = new Vector3(1, 0, 0);

        Vector3 rayleft = new Vector3(-1, 0, 0);

        Vector3 rayback = new Vector3(0, 0, -1);


        //forward
        if (Physics.Raycast(transform.position, rayforward, out hit, 0.3f))
        {
            if (hit.transform.tag == "Wall")
            {
                checkWall = 1;
                isWall = true;
                Debug.DrawLine(m_tr.position, m_tr.position + m_tr.forward * hit.distance, Color.red);
                PlayerMove();

            }

        }

        //right
        else if (Physics.Raycast(transform.position, rayright, out hit, 0.3f))
        {
            if (hit.transform.tag == "Wall")
            {
                checkWall = 2;
                isWall = true;
                Debug.DrawLine(m_tr.position, m_tr.position + m_tr.forward * hit.distance, Color.red);
                PlayerMove();

            }
        }
        //back
        else if (Physics.Raycast(transform.position, rayback, out hit, 0.3f))
        {
            if (hit.transform.tag == "Wall")
            {
                checkWall = 3;
                isWall = true;
                Debug.DrawLine(m_tr.position, m_tr.position + m_tr.forward * hit.distance, Color.red);
                PlayerMove();

            }
        }

        //left
        else if (Physics.Raycast(transform.position,rayleft, out hit, 0.3f))
        {
            if (hit.transform.tag == "Wall")
            {
                checkWall = 4;
                isWall = true;
                Debug.DrawLine(m_tr.position, m_tr.position + m_tr.forward * hit.distance, Color.red);
                PlayerMove();

            }
        }

        

        else 
        {
            checkWall = 0;
            isWall = false;
            PlayerMove();

        }
    }

    private void PlayerMove()
    {
        // if (checkWall == 1 && isWall == true)
        // {
        //     Debug.Log("1");
        //     _rigidbody.velocity = new Vector3(_joystick.Horizontal  * _moveSpeed, 0, 0 * _moveSpeed);

        // }
        // else if(checkWall == 2 && isWall == true)
        // {
        //     Debug.Log("2");
        //     _rigidbody.velocity = new Vector3(0 * _moveSpeed, 0, _joystick.Vertical * _moveSpeed);
        // }
        // else if(checkWall == 3 && isWall == true )
        // {
        //     Debug.Log("3");
        //     _rigidbody.velocity = new Vector3(_joystick.Horizontal  * _moveSpeed, 0, 0 * _moveSpeed);
        // }
        // else if(checkWall == 4 && isWall == true)
        // {
        //     Debug.Log("4");
        //     _rigidbody.velocity = new Vector3(0 * _moveSpeed, 0, _joystick.Vertical  * _moveSpeed);
        // }
        //else 
        //{
            _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, 0, _joystick.Vertical * _moveSpeed);
        //}
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        if (other.tag == "Wall")
        {
            isWall = true;
        }


        else if (other.tag == "Item")
        {

            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.LunchItem:
                    CoinSound.Play();
                    lunchitem += item.value;
                    if (lunchitem > maxItem)
                        lunchitem = maxItem;
                    break;

            }
            Debug.Log("Ate Item");
            if(GameManager.I.stage == 1)
            {
                Debug.Log("lunchitem: " + lunchitem);

                GameManager.I.StageEnd();
            }
            else
            {
                if (lunchitem < GameManager.I.haveToEatItem)
                {
                    GameManager.I.ItemToPut();
                    Destroy(other.gameObject);
                }
                if (lunchitem >= GameManager.I.haveToEatItem)
                {
                    GameManager.I.ItemToPut();
                    Destroy(other.gameObject);

                    Debug.Log("Eat all Lunch at This Stage");
                    GameManager.I.StageEnd();
                }
            }

        }

        else if(other.tag == "TimeItem")
        {
            CoinSound.Play();
            if(GameManager.I.stage <= 20)
            {
                if (GameManager.I.playTime > 30)
                    GameManager.I.playTime = 10;
                else if (GameManager.I.playTime > 50)
                    GameManager.I.playTime = 30;
            }
            else
            {
                if (GameManager.I.playTime > 30)
                    GameManager.I.playTime = 15;
                else if (GameManager.I.playTime > 50)
                    GameManager.I.playTime = 35;
            }

            Destroy(other.gameObject);
        }
        else if (other.tag == "HideZone")
        {
            isHide = true;
            Debug.Log("Sit Down Animation");
            anim.SetBool("SitDown", true);
        }

        else if (other.tag == "Enemy")
        {
            if(isHide == false)
            {
                if(GameManager.I.isPlay == true)
                {
                    if (GameManager.I.alreadyDead == false)
                    {
                        GameOverSound.Play();
                        GameManager.I.alreadyDead = true;
                        GameManager.I.isPlay = false;
                        GameManager.I.isDead = true;
                        GameManager.I.LevlSet = false;

                        GameManager.I.GameOver();
                    }
                    else
                    {
                        GameManager.I.isPlay = false;
                        GameManager.I.isDead = true;
                        GameManager.I.LevlSet = false;

                        GameManager.I.GameOver();
                    }
                }
                
                
            }
            
        }
        else if (other.tag == "KillingPlant")
        {
            if (GameManager.I.isPlay == true)
            {
                if (GameManager.I.alreadyDead == false)
                {
                    GameOverSound.Play();
                    GameManager.I.alreadyDead = true;
                    GameManager.I.isPlay = false;
                    GameManager.I.isDead = true;
                    GameManager.I.LevlSet = false;

                    GameManager.I.GameOver();

                }
                else
                {
                    GameManager.I.isPlay = false;
                    GameManager.I.isDead = true;
                    GameManager.I.LevlSet = false;

                    GameManager.I.GameOver();
                }
            }
        }

        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Wall")
        {
            isWall = false;
        }
        else if(other.tag == "HideZone")
        {
            isHide = false;
            Debug.Log("Sit Down Animation Exit");
            anim.SetBool("SitDown", false);
        }
    }
}