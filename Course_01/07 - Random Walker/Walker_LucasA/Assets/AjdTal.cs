using System;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

class AjdTal : IRandomWalker
{
    //Add your own variables here.
    //Do not use processing variables like width or height

    private Dictionary<Char, List<Vector2>> Alphabet = new()
    {
        {'A',  new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.down,
        Vector2.down,

        Vector2.left,
        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,


        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right
    } },
        {'B',  new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.down,
        Vector2.down,


        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.down,
        Vector2.down,

        Vector2.left,
        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.down,



        Vector2.right,
        Vector2.right

    } },
        {'C',  new List<Vector2>()
    {
        Vector2.right,
        Vector2.up,
        Vector2.up,
        Vector2.left,

        Vector2.up,
        Vector2.up,
        Vector2.right,
        Vector2.up,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.down,

        Vector2.up,
        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.left,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.down,
        Vector2.down,


        Vector2.right,
        Vector2.right
    } },
        {'D',  new List<Vector2>()
    {

        Vector2.up,
        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.up,
        Vector2.right,

        Vector2.up,
        Vector2.up,
        Vector2.left,
        Vector2.up,

        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.down,
        Vector2.right,


        Vector2.right,
        Vector2.right
    } },
        {'E',  new List<Vector2>()
    {


        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.left,
        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.down,

        Vector2.right,
        Vector2.right
    } },
        {'F',  new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.left,
        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,


        Vector2.right,
        Vector2.right
    } },
        {'G', new List<Vector2>()
    {
        Vector2.right,
        Vector2.up,
        Vector2.up,
        Vector2.left,

        Vector2.up,
        Vector2.up,
        Vector2.right,
        Vector2.up,

        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.left,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.up,
        Vector2.left,
        Vector2.right,

        Vector2.down,
        Vector2.down,
        Vector2.down,


        Vector2.right,
        Vector2.right
    } },
        {'H', new List<Vector2>()
    {


        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,


        Vector2.right,
        Vector2.right
    } },
        {'I', new List<Vector2>()
    {

        Vector2.right,
        Vector2.right,


        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,


        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,


        Vector2.right,
        Vector2.right
    } },
        {'J', new List<Vector2>()
    {


        Vector2.up,
        Vector2.up,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,


        Vector2.right,
        Vector2.right
    } },
        {'K', new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.right,
        Vector2.up,

        Vector2.down,
        Vector2.left,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.down,
        Vector2.right,
        Vector2.down,



        Vector2.right,
        Vector2.right
    } },
        {'L', new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.down,

        Vector2.right,
        Vector2.right
    } },
        {'M', new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.right,
        Vector2.down,
        Vector2.right,
        Vector2.down,

        Vector2.up,
        Vector2.right,
        Vector2.up,
        Vector2.right,
        Vector2.up,

        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right
    } },
        {'N', new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.right,
        Vector2.down,
        Vector2.right,
        Vector2.down,
        Vector2.right,
        Vector2.down,
        Vector2.right,

        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right
    } },
        {'O', new List<Vector2>()
    {
        Vector2.right,
        Vector2.up,

        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.right,

        Vector2.up,
        Vector2.up,

        Vector2.left,
        Vector2.up,

        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.left,

        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.down,

        Vector2.right,
        Vector2.right,

        Vector2.down,
        Vector2.right,

        Vector2.right,
        Vector2.right
    } },
        {'P', new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.down,
        Vector2.right,

        Vector2.down,
        Vector2.left,
        Vector2.down,
        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,


        Vector2.right,
        Vector2.right
    } },
        {'Q', new List<Vector2>()
    {

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.left,

        Vector2.down,
        Vector2.right,
        Vector2.down,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.down,
        Vector2.down,


        Vector2.right,
        Vector2.right
    } },//Uses small "q"
        {'R', new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.down,
        Vector2.down,

        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.right,
        Vector2.down,
        Vector2.right,
        Vector2.down,
        Vector2.right,
        Vector2.down,

        Vector2.right,
        Vector2.right
    } },
        {'S', new List<Vector2>()
    {
        Vector2.up,

        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.right,
        Vector2.left,
        Vector2.up,

        Vector2.left,
        Vector2.left,

        Vector2.up,
        Vector2.left,
        Vector2.right,
        Vector2.up,


        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,

        Vector2.down,
        Vector2.down,

        Vector2.down,
        Vector2.right,


        Vector2.right,
        Vector2.right
    } },
        {'T', new List<Vector2>()
    {
        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.left,
        Vector2.left,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.left,
        Vector2.left,

        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,


        Vector2.right,
        Vector2.right
    } },
        {'U', new List<Vector2>()
    {
        Vector2.right,
        Vector2.up,

        Vector2.up,
        Vector2.left,

        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.down,

        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.right,

        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.left,

        Vector2.down,
        Vector2.down,
        Vector2.right,


        Vector2.right,
        Vector2.right
    } },
        {'V', new List<Vector2>()
    {
        Vector2.right,
        Vector2.right,
        Vector2.up,

        Vector2.up,
        Vector2.left,

        Vector2.up,
        Vector2.left,

        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,
        Vector2.right,
        Vector2.down,

        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.right,

        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,

        Vector2.left,
        Vector2.down,
        Vector2.left,

        Vector2.down,
        Vector2.down,
        Vector2.right,
        Vector2.right,

        Vector2.right,
        Vector2.right
    } },
        {'W', new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,

        Vector2.up,
        Vector2.right,

        Vector2.up,


        Vector2.down,
        Vector2.right,
        Vector2.down,
        Vector2.right,

        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right
    } },
        {'X', new List<Vector2>()
    {
        Vector2.up,
        Vector2.right,

        Vector2.up,
        Vector2.right,

        Vector2.up,

        Vector2.up,
        Vector2.right,


        Vector2.up,
        Vector2.right,




        Vector2.left,
        Vector2.down,
        Vector2.left,



        Vector2.left,


        Vector2.up,
        Vector2.left,


        Vector2.right,
        Vector2.down,

        Vector2.right,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.down,

        Vector2.right,
        Vector2.down,


        Vector2.right,
        Vector2.right
    } },
        {'Y', new List<Vector2>()
    {
        Vector2.right,
        Vector2.right,

        Vector2.up,
        Vector2.up,
        Vector2.up,


        Vector2.left,
        Vector2.up,
        Vector2.left,
        Vector2.up,

        Vector2.down,
        Vector2.right,
        Vector2.down,
        Vector2.right,

        Vector2.right,
        Vector2.up,
        Vector2.right,
        Vector2.up,

        Vector2.down,
        Vector2.left,
        Vector2.down,
        Vector2.left,

        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,

        Vector2.right,
        Vector2.right
    } },
        {'Z', new List<Vector2>()
    {
        Vector2.up,
        Vector2.right,

        Vector2.up,
        Vector2.right,

        Vector2.up,
        Vector2.right,

        Vector2.up,
        Vector2.right,

        Vector2.up,
        Vector2.right,

        Vector2.left,
        Vector2.left,
        Vector2.left,
        Vector2.left,

        Vector2.right,
        Vector2.right,


        Vector2.down,
        Vector2.down,

        Vector2.left,

        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.down,

        Vector2.right,
        Vector2.right
    } },
        {' ', new List<Vector2>()
    {
        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.right,
        Vector2.right
    } },
        {'\'', new List<Vector2>()
    {

    } },
        {',', new List<Vector2>()
    {
        Vector2.up,
        Vector2.up,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,


        Vector2.right,
        Vector2.right,


    } },
        {'.', new List<Vector2>()
    {

        Vector2.up,
        Vector2.down,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.right,
        Vector2.right,


    } },
        {'Å', new List<Vector2>()
    {

        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,
        Vector2.up,

        Vector2.right,
        Vector2.right,
        Vector2.right,
        Vector2.right,

        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,
        Vector2.down,

        Vector2.right,
        Vector2.right,


    } },

    };

    Vector2 currentPosition, currentMove, originalStartingPoint;

    int nextRowDistance = 10;

    int currentIndex = 0;

    int startPointY, startPointX;
    int screenWidth, screenHight;

    int unWritableArea;


    bool startingNewRow = true;
    bool startWriting = true;
    bool firstCheck = false;






    List<Vector2> Characters = new List<Vector2>() { };

    public string GetName()
    {
        return "Ajdins Epic Typewriter";
    }

    //Add epic text below,
    //A-Z , Space, Comma and Dot works.
    //Apostrofe works but does not add anything due to the difficulty of making it.
    //Å returns a box shape.
    public AjdTal()
    {
        string sentance = "WE'RE NO STRANGERS TO LOVE, YOU KNOW THE RULES AND SO DO I. A FULL COMMITMENT'S WHAT I'M THINKING OF, YOU WOULDN'T GET THIS FROM ANY OTHER GUY. I JUST WANNA TELL YOU HOW I'M FEELING GOTTA MAKE YOU UNDERSTAND, NEVER GONNA GIVE YOU UP NEVER GONNA LET YOU DOWN NEVER GONNA RUN AROUND AND DESERT YOU NEVER GONNA MAKE YOU CRY NEVER GONNA SAY GOODBYE NEVER GONNA TELL A LIE AND HURT YOU. WE'VE KNOWN EACH OTHER FOR SO LONG, YOUR HEART'S BEEN ACHING, BUT YOU'RE TOO SHY TO SAY IT INSIDE, WE BOTH KNOW WHAT'S BEEN GOING ON, WE KNOW THE GAME AND WE'RE GONNA PLAY IT AND IF YOU ASK ME HOW I'M FEELING DON'T TELL ME YOU'RE TOO BLIND TO SEE. NEVER GONNA GIVE YOU UP NEVER GONNA LET YOU DOWN NEVER GONNA RUN AROUND AND DESERT YOU NEVER GONNA MAKE YOU CRY NEVER GONNA SAY GOODBYE NEVER GONNA TELL A LIE AND HURT YOU, NEVER GONNA GIVE YOU UP NEVER GONNA LET YOU DOWN NEVER GONNA RUN AROUND AND DESERT YOU NEVER GONNA MAKE YOU CRY NEVER GONNA SAY GOODBYE NEVER GONNA TELL A LIE AND HURT YOU  OOOOOH, GIVE YOU UP  OOOOOH, GIVE YOU UP  NEVER GONNA GIVE, NEVER GONNA GIVE  GIVE YOU UP NEVER GONNA GIVE, NEVER GONNA GIVE GIVE YOU UP WE'VE KNOWN EACH OTHER FOR SO LONG YOUR HEART'S BEEN ACHING, BUT YOU'RE TOO SHY TO SAY IT INSIDE, WE BOTH KNOW WHAT'S BEEN GOING ON WE KNOW THE GAME AND WE'RE GONNA PLAY IT, I JUST WANNA TELL YOU HOW I'M FEELING GOTTA MAKE YOU UNDERSTAND NEVER GONNA GIVE YOU UP NEVER GONNA LET YOU DOWN NEVER GONNA RUN AROUND AND DESERT YOU NEVER GONNA MAKE YOU CRY NEVER GONNA SAY GOODBYE NEVER GONNA TELL A LIE AND HURT YOU, NEVER GONNA GIVE YOU UP NEVER GONNA LET YOU DOWN NEVER GONNA RUN AROUND AND DESERT YOU NEVER GONNA MAKE YOU CRY NEVER GONNA SAY GOODBYE NEVER GONNA TELL A LIE AND HURT YOU, NEVER GONNA GIVE YOU UP NEVER GONNA LET YOU DOWN NEVER GONNA RUN AROUND AND DESERT YOU NEVER GONNA MAKE YOU CRY NEVER GONNA SAY GOODBYE NEVER GONNA TELL A LIE AND HURT YOU. " +
            "IF THERE'S ANYTHING YOU NEED, ALL YOU HAVE TO DO IS SAY, YOU KNOW YOU SATISFY EVERYTHING IN ME, WE SHOULDN'T WASTE A SINGLE DAY, SO DON'T STOP ME FALLING IT'S DESTINY CALLING, A POWER I JUST CAN'T DENY, IT'S NEVERCHANGING CAN'T YOU HEAR ME, I'M SAYING I WANT YOU FOR THE REST OF MY LIFE, TOGETHER FOREVER AND NEVER TO PART TOGETHER FOREVER WE TWO AND DON'T YOU KNOW I WOULD MOVE HEAVEN AND EARTH, TO BE TOGETHER FOREVER WITH YOU IF THEY EVER GET YOU DOWN THERE'S ALWAYS SOMETHING I CAN DO BECAUSE I WOULDN'T EVER WANNA SEE YOU FROWN I'LL ALWAYS DO WHAT'S BEST FOR YOU THERE AIN'T NO MISTAKING IT'S TRUE LOVE WE'RE MAKING SOMETHING TO LAST FOR ALL TIME, IT'S NEVERCHANGING CAN'T YOU HEAR ME, I'M SAYING I WANT YOU FOR THE REST OF MY LIFE, TOGETHER FOREVER AND NEVER TO PART TOGETHER FOREVER WE TWO AND DON'T YOU KNOW I WOULD MOVE HEAVEN AND EARTH TO BE TOGETHER FOREVER WITH YOU, SO DON'T STOP ME FALLING IT'S DESTINY CALLING A POWER I JUST CAN'T DENY, IT'S NEVERCHANGING CAN'T YOU HEAR ME, I'M SAYING I WANT YOU FOR THE REST OF MY LIFE, TOGETHER FOREVER AND NEVER TO PART TOGETHER FOREVER WE TWO AND DON'T YOU KNOW, I WOULD MOVE HEAVEN AND EARTH, TO BE TOGETHER FOREVER WITH YOU TOGETHER FOREVER AND NEVER TO PART  TOGETHER FOREVER WE TWO AND DON'T YOU KNOW, I WOULD MOVE HEAVEN AND EARTH TO BE TOGETHER FOREVER WITH YOU, TOGETHER FOREVER AND NEVER TO PART TOGETHER FOREVER WE TWO AND DON'T YOU KNOW, I WOULD MOVE HEAVEN AND EARTH TO BE TOGETHER FOREVER WITH YOU";

        foreach (char letter in sentance)
        {
            Characters.AddRange(Alphabet[letter]);
        }

    }


    public Vector2 GetStartPosition(int playAreaWidth, int playAreaHeight)
    {
        screenHight = playAreaHeight;
        screenWidth = playAreaWidth;

        startPointX = 4;
        startPointY = screenHight - 7;

        //8 is offset from both left and right aka 4+4
        //Takes screen with and sees how many boundles of 6 fits (Due to one character being 6 long), 
        //takes that diffrence and using it checks when a new row should be started to avoid cutting characters halfway to start a new row
        unWritableArea = screenWidth - 8 - (((int)Mathf.Floor((screenWidth - 8) / 6)) * 6);

        originalStartingPoint = currentPosition = new Vector2(startPointX, startPointY);

        return currentPosition;
    }


    public Vector2 Movement()
    {

        
        if (currentPosition.y - 5 < 0)
        {
            startingNewRow = false;
            startWriting = false;

            //Resetting the startpont to not bug when starting new row
            startPointX = 4;
            startPointY = screenHight - 7;

        }
        else if (currentPosition.x + unWritableArea + 4 >= screenWidth)
        {
            firstCheck = false;

            startWriting = false;
        }

        if (startWriting)
        {
            currentMove = Characters[currentIndex];
            if (currentIndex < Characters.Count - 1)
                currentIndex++;

        }
        else if (startingNewRow)
        {
            startNewRow();
        }
        else
        {
            returnToStart();
        }

        currentPosition += currentMove;

        return currentMove;
    }


    void startNewRow()
    {
        if (currentPosition.y > startPointY)
        {
            currentMove = Vector2.down;
        }
        else if (currentPosition.x > startPointX)
        {
            currentMove = Vector2.left;
        }
        else if (!firstCheck)
        {
            startPointY -= nextRowDistance;
            firstCheck = true;
        }
        else
        {
            currentMove = Vector2.right;
            startWriting = true;
        }
    }


    void returnToStart()
    {
        //ABSOLUTELY NO CLUE WHAT'S GOING ON
        // WARNING: Mega fiddely and changing in any way will cause it to go too high or too low!!


        Debug.Log("Start point is" + originalStartingPoint);
        if (currentPosition.y < originalStartingPoint.y)
        {
            currentMove = Vector2.up;
            Debug.Log("YPostest" + currentPosition);
        }
        //else if (currentPosition.x < startPointX)
        //{
        //    currentMove = Vector2.right;
        //}
        else
        {
            currentMove = Vector2.right;
            Debug.Log("YPostestEnd" + currentPosition);
            startingNewRow = true;
            startWriting = true;
        }
    }

}