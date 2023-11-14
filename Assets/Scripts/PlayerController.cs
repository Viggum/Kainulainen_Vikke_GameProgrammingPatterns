using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
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
    public bool onCooldown = false;
    public float c_t = 7;
    public float temp = 0;

    public UnityEngine.UI.Image cooldownFadeImage;
    public TextMeshProUGUI cd_txt;
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
        if (onCooldown)
        {
            temp += Time.deltaTime;
            cd_txt.text = (c_t - temp).ToString("F0");
            cooldownFadeImage.fillAmount = (c_t - temp)/c_t;

            if (temp > c_t)
            {
                onCooldown = false;
                temp = 0;
                cooldownFadeImage.gameObject.SetActive(false);
                cd_txt.gameObject.SetActive(false);
            }
        }

        if (!isReplaying)
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

            // Undo commands "Recall"
            if (Input.GetKeyDown(KeyCode.E))
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
            if (Input.GetKeyDown(KeyCode.T))
            {               
                isReplaying = true;
                transform.position = startPos;
                StartCoroutine(ReplayCommands());
            }

            // Jump
            if (Input.GetKeyDown(KeyCode.Space))
                _rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);

        
            // "Dash"
            if(Input.GetKeyDown(KeyCode.LeftShift))
                for(int i = 0; i < 5; i++)
                {
                    _last_command.Execute(_rigidbody, Visuals);
                    _undo_commands.Push(_last_command);
                }
                    

            // Swap commands test
            if (Input.GetKeyDown(KeyCode.P))
                SwapCommands(ref cmd_A, ref cmd_D);
        }

    }

    public void SwapCommands(ref Command A, ref Command B)
    {
        Command tmp = A;
        A = B;
        B = tmp;
    }

    // "Recall" - Undo commands
    public IEnumerator UndoCommands()
    {
        if (!onCooldown)
        {
            isReplaying = true;

            cooldownFadeImage.gameObject.SetActive(true);
            cd_txt.gameObject.SetActive(true);
            onCooldown = true;

            yield return new WaitForSeconds(1f);

            while (_undo_commands.Count > 0)
            {
                Command cmd = _undo_commands.Pop();
                cmd.Undo(_rigidbody, Visuals);
                yield return new WaitForSeconds(0.01f);
            }

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

        isReplaying = false;
    }
}
