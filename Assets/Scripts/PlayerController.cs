using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public enum PlayerState
    {
        JUMPING,
        STANDING,
        CROUCHING,
        REPLAYING
    }

    
    public PlayerState myState;

    private Rigidbody _rigidbody;

    public GameObject Visuals;

    Command cmd_W = new MoveForwardCommand();
    Command cmd_A = new MoveLeftCommand();
    Command cmd_S = new MoveBackwardCommand();
    Command cmd_D = new MoveRightCommand();

    Command _last_command = new DoNothingCommand();

    Stack<Command> _undo_commands = new Stack<Command>();

    private bool isReplaying = false;
    
    private Vector3 startPos;

    public float moveSpeed = 5f;
    [Min(0.01f)]public float jumpForce;


    #region Recall
    [Header("Recall")]
    public bool recallOnCooldown = false;
    public float recallCooldown = 7f;
    public float temp = 0;
    public Image cooldownFadeImage;
    public TextMeshProUGUI cd_txt;
    #endregion


    #region Dash
    private int maxDash = 3;
    private int currentDash = 3;
    [Header("Dash")]
    public float dashCooldown = 3f;
    public float dashTemp = 0;
    public TextMeshProUGUI dashAmountText;
    public Image dashCooldownIcon;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position;
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {      

        // Recall cooldown
        if (recallOnCooldown)
        {
            temp += Time.deltaTime;
            cd_txt.text = (recallCooldown - temp).ToString("F0");
            cooldownFadeImage.fillAmount = (recallCooldown - temp)/recallCooldown;

            if (temp > recallCooldown)
            {
                recallOnCooldown = false;
                temp = 0;
                cooldownFadeImage.gameObject.SetActive(false);
                cd_txt.gameObject.SetActive(false);
            }
        }

        // Dash cooldown
        dashAmountText.text = currentDash.ToString();

        if (currentDash < maxDash)
        {
            dashTemp += Time.deltaTime;          
            dashCooldownIcon.fillAmount = 1 - (dashCooldown - dashTemp)/dashCooldown;

            if(dashTemp >= dashCooldown)
            {
                currentDash++;
                dashTemp = 0;
            }
        }


        switch (myState)
        {
            case PlayerState.JUMPING:

                // WASD - commands
                MovementCommands();

                // Abilities etc.
                CommonCommands();

                checkStanding();
                break;

            case PlayerState.STANDING:
                // Jump
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    myState = PlayerState.JUMPING;
                    _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                }

                // WASD - commands
                MovementCommands();

                // Abilities etc.
                CommonCommands();

                // Crouch
                if (Input.GetKeyDown(KeyCode.LeftControl))
                {
                    transform.localScale -= new Vector3(0, 0.5f, 0);
                    myState = PlayerState.CROUCHING;
                }

                break;

            case PlayerState.CROUCHING:

                if (Input.GetKeyUp(KeyCode.LeftControl))
                {
                    transform.localScale += new Vector3(0, 0.5f, 0);
                    myState = PlayerState.STANDING;
                }

                // Abilities etc.
                CommonCommands();

                break;

            case PlayerState.REPLAYING:

                break;
        }      

        Debug.Log("Player state: " + myState);

    }

    public void SwapCommands(ref Command A, ref Command B)
    {
        Command tmp = A;
        A = B;
        B = tmp;
    }

    public void MovementCommands()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            cmd_W.Execute(_rigidbody, Visuals);
            _last_command = cmd_W;
            _undo_commands.Push(cmd_W);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            cmd_A.Execute(_rigidbody, Visuals);
            _last_command = cmd_A;
            _undo_commands.Push(cmd_A);
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            cmd_S.Execute(_rigidbody, Visuals);
            _last_command = cmd_S;
            _undo_commands.Push(cmd_S);
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            cmd_D.Execute(_rigidbody, Visuals);
            _last_command = cmd_D;
            _undo_commands.Push(cmd_D);
        }
    }

    public void CommonCommands()
    {
        // Undo commands "Recall"
        if (Input.GetKeyDown(KeyCode.E) && myState != PlayerState.REPLAYING)
        {           
            StartCoroutine(UndoCommands());

            #region test
            //while (_undo_commands.Count > 0)
            //{
            //    Command cmd = _undo_commands.Pop();
            //    cmd.Undo(_rigidbody);
            //}

            //_last_command.Undo(_rigidbody);
            //_last_command = new DoNothingCommand(); // Make it nothing
            #endregion
        }

        // Redo last command
        if (Input.GetKeyDown(KeyCode.R))
            _last_command.Execute(_rigidbody, Visuals);

        // Replay
        if (Input.GetKeyDown(KeyCode.T) && myState != PlayerState.REPLAYING)
        {
            isReplaying = true;
            myState = PlayerState.REPLAYING;
            transform.position = startPos;
            StartCoroutine(ReplayCommands());
        }

        // "Dash"
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentDash > 0)
        {
            currentDash--;
            for (int i = 0; i < 5; i++)
            {
                _last_command.Execute(_rigidbody, Visuals);
                _undo_commands.Push(_last_command);
            }
        }

        // Swap commands test
        if (Input.GetKeyDown(KeyCode.P))
            SwapCommands(ref cmd_A, ref cmd_D);
    }

    // "Recall" - Undo commands
    public IEnumerator UndoCommands()
    {
        if (!recallOnCooldown)
        {
            myState = PlayerState.REPLAYING;
            isReplaying = true;

            cooldownFadeImage.gameObject.SetActive(true);
            cd_txt.gameObject.SetActive(true);
            recallOnCooldown = true;

            yield return new WaitForSeconds(1f);

            while (_undo_commands.Count > 0)
            {
                Command cmd = _undo_commands.Pop();
                cmd.Undo(_rigidbody, Visuals);
                yield return new WaitForSeconds(0.01f);
            }

            myState = PlayerState.STANDING;
            isReplaying = false;
        }
    }

    public IEnumerator ReplayCommands()
    {
        yield return new WaitForSeconds(1f);

        Stack reverseStack = new Stack();

        foreach (Command cmd in _undo_commands)
            reverseStack.Push(cmd);

        foreach (Command cmd in reverseStack)
        {
            cmd.Execute(_rigidbody, Visuals);
            yield return new WaitForSeconds(0.01f);
        }

        myState = PlayerState.STANDING;
        isReplaying = false;
    }

    public void checkStanding()
    {
               
        //RaycastHit hit;
        //// Does the ray intersect any objects excluding the player layer
        //if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, Mathf.Infinity))
        //{
        //    if (Vector3.Distance(transform.position, hit.point) <= 0.2f)
        //        myState = PlayerState.STANDING;
               
        //    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.down) * hit.distance, Color.yellow);
        //    Debug.Log(Vector3.Distance(transform.position, hit.point));
        //}
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 3)
            myState = PlayerState.STANDING;
    }

}
