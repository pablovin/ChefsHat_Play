using UnityEngine;

namespace LoadLocalResources {


public class ResourceCards{
static Sprite CARD_1 =  Resources.Load <Sprite>("cards/Card_1@4x");      //FUL
static Sprite CARD_2 =  Resources.Load <Sprite>("cards/Card_2@4x");      //FUL
static Sprite CARD_3 =  Resources.Load <Sprite>("cards/Card_3@4x");      //FUL
static Sprite CARD_4 =  Resources.Load <Sprite>("cards/Card_4@4x");      //FUL
static Sprite CARD_5 =  Resources.Load <Sprite>("cards/Card_5@4x");      //FUL
static Sprite CARD_6 =  Resources.Load <Sprite>("cards/Card_6@4x");      //FUL
static Sprite CARD_7 =  Resources.Load <Sprite>("cards/Card_7@4x");      //FUL
static Sprite CARD_8 =  Resources.Load <Sprite>("cards/Card_8@4x");      //FUL
static Sprite CARD_9 =  Resources.Load <Sprite>("cards/Card_9@4x");      //FUL
static Sprite CARD_10 =  Resources.Load <Sprite>("cards/Card_10@4x");      //FUL
static Sprite CARD_11 =  Resources.Load <Sprite>("cards/Card_11@4x");      //FUL
static Sprite CARD_11_Golden =  Resources.Load <Sprite>("cards/Card_Golden_11@4x");      //FUL
static Sprite CARD_JOKER =  Resources.Load <Sprite>("cards/Card_Joker@4x");      //FUL
static Sprite CARD_PASS =  Resources.Load <Sprite>("cards/Card_Pass@4x");      //FUL

    public static Sprite GetSprite(int cardNumber)
    {

     if (cardNumber==1)   {return CARD_1;}
     if (cardNumber==2)   {return CARD_2;}
     if (cardNumber==3)   {return CARD_3;}
     if (cardNumber==4)   {return CARD_4;}
     if (cardNumber==5)   {return CARD_5;}
     if (cardNumber==6)   {return CARD_6;}
     if (cardNumber==7)   {return CARD_7;}
     if (cardNumber==8)   {return CARD_8;}
     if (cardNumber==9)   {return CARD_9;}
     if (cardNumber==10)   {return CARD_10;}
     if (cardNumber==11)   {return CARD_11;}
     if (cardNumber==12)   {return CARD_JOKER;}
     return CARD_11_Golden;


    }
}
 
}