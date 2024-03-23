using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    private int age = 0;

    public bool isAlive = false;
    public bool isBorder = false;
    public bool isPlayer = false;
    public bool isFood = false;
    private bool isRainbow = false;

    private float timer = 0f;

    private static int arcobalenoStage = 0;
    private int arcStage;

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = true;
    }
    private void Update()
    {
        if(timer > GameSnake.speed)
        {
            age--;
            if(isPlayer)
            {
                CheckAge();
            }
            if(isRainbow)
            {
                CiclaColori();
            }
            timer = 0f;
        }
        else
        {
            timer += Time.deltaTime;
        }
    }
    public void SetState(int state)
    {
        if (state == 1)//alive
        {
            isAlive = true;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else if(state == 2)//border
        {
            isBorder = true;
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (state == 3)//player
        {
            isFood = false;
            isPlayer = true;
            GetComponent<SpriteRenderer>().color = Color.green;
        }
        else if (state == 4)//food
        {
            isFood = true;
            GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.8f, 0f);
        }
        else if (state == 5)// player age over 100
        {
            isFood = false;
            isPlayer = true;
            isRainbow = true;

            switch (ArcobalenoStage)
            {
                case 1:
                    {
                        GetComponent<SpriteRenderer>().color = new Color(1f, 0.647f, 0);//orange
                        arcStage = ArcobalenoStage;
                        ArcobalenoStage++;
                        break;
                    }
                case 2:
                    {
                        GetComponent<SpriteRenderer>().color = Color.yellow;//giallo
                        arcStage = ArcobalenoStage;
                        ArcobalenoStage++;
                        break;
                    }                
                case 3:
                    {
                        GetComponent<SpriteRenderer>().color = Color.green;//verde
                        arcStage = ArcobalenoStage;
                        ArcobalenoStage++;
                        break;
                    }                
                case 4:
                    {
                        GetComponent<SpriteRenderer>().color = Color.blue;//blu
                        arcStage = ArcobalenoStage;
                        ArcobalenoStage++;
                        break;
                    }                
                case 5:
                    {
                        GetComponent<SpriteRenderer>().color = new Color(0.51f, 0, 1f);//viola
                        arcStage = ArcobalenoStage;
                        ArcobalenoStage++;
                        break;
                    }
                default:
                    {
                        GetComponent<SpriteRenderer>().color = Color.red;//rosso
                        arcStage = ArcobalenoStage;
                        ArcobalenoStage++;
                        break;
                    }
            }
        }
        else//state == 0 dead
        {
            isBorder = false;
            isAlive = false;
            isPlayer = false;
            isFood = false;
            isRainbow = false;
            GetComponent<SpriteRenderer>().color = Color.black;
        }
    }
    public void SetHead(bool isHead)
    {
        if(isHead == true)
        {
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            if(age > 100)
            {
                SetState(5);
            }
            else
            {
                SetState(3);
            }
        }
    }
    private void CheckAge()
    {
        if(age == 0)
        {
            SetState(0);
        }
    }
    private void CiclaColori()
    {
        switch (ArcStage)
        {
            case 1:
                {
                    GetComponent<SpriteRenderer>().color = new Color(1f, 0.647f, 0);//orange
                    ArcStage++;
                    break;
                }
            case 2:
                {
                    GetComponent<SpriteRenderer>().color = Color.yellow;//giallo
                    ArcStage++;
                    break;
                }
            case 3:
                {
                    GetComponent<SpriteRenderer>().color = Color.green;//verde
                    ArcStage++;
                    break;
                }
            case 4:
                {
                    GetComponent<SpriteRenderer>().color = Color.blue;//blu
                    ArcStage++;
                    break;
                }
            case 5:
                {
                    GetComponent<SpriteRenderer>().color = new Color(0.51f, 0, 1f);//viola
                    ArcStage++;
                    break;
                }
            default:
                {
                    GetComponent<SpriteRenderer>().color = Color.red;//rosso
                    ArcStage++;
                    break;
                }
        }
    }
    public int Age
    {
        get { return age; }
        set { age = value; }
    }
    public static int ArcobalenoStage
    {
        get { return arcobalenoStage; }
        set 
        { 
            if(arcobalenoStage == 5)
            {
                arcobalenoStage = 0;
            }
            else
            {
                arcobalenoStage = value;
            }
        }
    }    
    private int ArcStage
    {
        get { return arcStage; }
        set 
        { 
            if(arcStage == 5)
            {
                arcStage = 0;
            }
            else
            {
                arcStage = value;//+1
            }
        }
    }
}