using ER.Entity2D;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Mod_Level
{
    public class AITestMan : BaseAI
    {
        private ATActionManager actionManager;
        private ATCharacterState state;
        private ATEye eye;
        private Entity player;//看见的玩家实体对象
        private bool seePlayer = false;
        /*
         * attack:攻击
         * chase:追击
         * swing:巡逻
         * idle:待机
         */

        public override void Initialize()
        {
            actionManager = owner.GetAttribute<ATActionManager>();
            state = owner.GetAttribute<ATCharacterState>();
            eye = owner.GetAttribute<ATEye>();

            eye.modifyEyeEvent += (entity) =>
            {
                if (entity.gameObject.tag != GameTagText.L_PLAYER) return;
                Debug.Log("实体进入/离开视野");
                if (eye.record.Contains(entity))
                {
                    seePlayer = true;
                    if (player == null) player = entity;
                    Debug.Log("看见玩家");
                }
                else
                {
                    Debug.Log("玩家消失");
                    player = null;
                    for (int i = 0; i < eye.record.Count; i++)
                    {
                        if (eye.record[i].gameObject.tag == GameTagText.L_PLAYER)
                        {
                            player = eye.record[i];
                            break;
                        }
                    }
                    if (player == null)
                    {
                        seePlayer = false;
                    }
                }
            };
        }

        private System.Random random = new System.Random(DateTime.Now.Millisecond);

        protected override bool ParseToDo(ToDo toDo)
        {
            switch (toDo.name)
            {
                case "idle":
                    DoDefault();
                    return false;

                case "swing":
                    return DoSwing(toDo.infos);

                case "chase":
                    return DoChase(toDo.infos);

                case "attack":
                    return DoAttack(toDo.infos);

                case "wait":
                    return DoWait(toDo.infos);

                case "attack_cd":
                    return DoAttackCD(toDo.infos);
            }
            return false;
        }

        private float waitTime = 0f;

        protected override void DoDefault()
        {
            if (state.Dead) return;
            if (!state.ControlAct) return;
            waitTime -= Time.deltaTime;
            if (actionManager.GetActionState("Move") != MDAction.ActionState.Disable)//停止移动, 并决定下一次移动
            {
                actionManager.Stop("Move");
            }

            if (seePlayer)
            {
                ToDoList.Add(new ToDo()
                {
                    name = "chase",
                    infos = null
                });
                return;
            }
            //Debug.Log($"waitTime:{waitTime}");
            if (waitTime <= 0)
            {
                waitTime = (float)random.NextDouble() * 8 + 3;
                ToDoList.Add(new ToDo()
                {
                    name = "swing",
                });
            }
        }

        private float moveTime = 0f;
        private ATCharacterState.Direction direction;

        private bool DoSwing(Dictionary<string, object> infos)
        {
            if (state.Dead) return true;
            if (!state.ControlAct) return false;
            waitTime -= Time.deltaTime;
            moveTime -= Time.deltaTime;
            if (seePlayer)
            {
                ToDoList.Add(new ToDo()
                {
                    name = "chase",
                    infos = null
                });
                return true;
            }
            if (waitTime <= 0)
            {
                waitTime = (float)random.NextDouble() * 2 + 1;
                ToDoList.Clear();
                return true;
            }
            else
            {
                if (moveTime > 0)
                {
                    if (actionManager.GetActionState("Attack") == MDAction.ActionState.Disable && actionManager.GetActionState("Move") == MDAction.ActionState.Disable)//非运动状态
                    {
                        switch (direction)
                        {
                            case ATCharacterState.Direction.Left:
                                actionManager.Action("Move", "left");
                                break;

                            case ATCharacterState.Direction.Right:
                                actionManager.Action("Move", "right");
                                break;
                        }
                    }
                }
                else
                {
                    if (actionManager.GetActionState("Move") != MDAction.ActionState.Disable)//停止移动, 并决定下一次移动
                    {
                        actionManager.Stop("Move");
                    }
                    if (random.NextDouble() > 0.5)
                    {
                        direction = ATCharacterState.Direction.Right;
                    }
                    else
                    {
                        direction = ATCharacterState.Direction.Left;
                    }
                    moveTime = (float)random.NextDouble() * 0.5f + 0.3f;
                }
            }
            return false;
        }

        private bool DoChase(Dictionary<string, object> infos)
        {
            if (state.Dead) return true;
            if (!state.ControlAct) return false;
            if (player == null) return true;
            Vector2 p_pos = player.transform.position;//玩家位置
            Vector2 s_pos = owner.transform.position;//自己位置


            if (actionManager.GetActionState("Attack") == MDAction.ActionState.Disable)
            {
                if (p_pos.x < s_pos.x)//玩家在左侧
                {
                    if (actionManager.GetActionState("Move") == MDAction.ActionState.Disable)
                    {
                        actionManager.Action("Move", "left");
                    }
                    if(state.direction == ATCharacterState.Direction.Left)
                    {
                        if ((p_pos - s_pos).magnitude < attack_distance)
                        {
                            actionManager.Stop("Move");
                            ToDoList.Add(new ToDo()
                            {
                                name = "attack",
                                infos = null
                            });
                            return true;
                        }
                    }
                }
                else
                {
                    if (actionManager.GetActionState("Move") == MDAction.ActionState.Disable)
                    {
                        actionManager.Action("Move", "right");
                    }
                    if (state.direction == ATCharacterState.Direction.Right)
                    {
                        if ((p_pos - s_pos).magnitude < attack_distance)
                        {
                            actionManager.Stop("Move");
                            ToDoList.Add(new ToDo()
                            {
                                name = "attack",
                                infos = null
                            });
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private float attack_distance = 5f;//攻击距离;

        private bool DoAttack(Dictionary<string, object> infos)
        {
            if (state.Dead) return true;
            if (!state.ControlAct) return false;
            Vector2 p_pos = player.transform.position;//玩家位置
            Vector2 s_pos = owner.transform.position;//自己位置
            if (p_pos.x < s_pos.x)//玩家在左侧
            {
                if (state.direction == ATCharacterState.Direction.Left)
                {
                    actionManager.Action("Attack");

                    waitTime = (float)random.NextDouble() * 1f + 0.5f;
                    Dictionary<string, object> ifs = new Dictionary<string, object>();
                    ifs.Add("cd", 0.5f);
                    ifs.Add("next_name", "attack_cd");
                    ifs.Add("next_infos", null);
                    ToDoList.Add(new ToDo()
                    {
                        name = "wait",
                        infos = ifs
                    });
                }
                else
                {
                    ToDoList.Add(new ToDo()
                    {
                        name = "chase",
                        infos = null
                    });
                }
            }
            else
            {
                if (state.direction == ATCharacterState.Direction.Right)
                {
                    actionManager.Action("Attack");

                    waitTime = (float)random.NextDouble() * 1f + 0.5f;
                    Dictionary<string, object> ifs = new Dictionary<string, object>();
                    ifs.Add("cd", 0.5f);
                    ifs.Add("next_name", "attack_cd");
                    ifs.Add("next_infos", null);
                    ToDoList.Add(new ToDo()
                    {
                        name = "wait",
                        infos = ifs
                    });
                }
                else
                {
                    ToDoList.Add(new ToDo()
                    {
                        name = "chase",
                        infos = null
                    });
                }
            }
            return true;
        }

        private bool DoWait(Dictionary<string, object> infos)
        {
            if (state.Dead) return true;
            if (!state.ControlAct) return false;
            float timer = (float)infos["cd"];
            timer -= Time.deltaTime;
            infos["cd"] = timer;
            if(timer<=0)
            {
                string name = (string)infos["next_name"];
                if (name == "none")
                    return true;
                ToDoList.Add(new ToDo()
                {
                    name = name,
                    infos = (Dictionary<string, object>)infos["next_infos"]
                }) ;
                return true;
            }
            return false;

        }

        private bool DoAttackCD(Dictionary<string, object> infos)
        {
            if (state.Dead) return true;
            if (!state.ControlAct) return false;
            waitTime -= Time.deltaTime;
            moveTime -= Time.deltaTime;
            if (waitTime <= 0)
            {
                actionManager.Stop("Move");
                ToDoList.Add(new ToDo()
                {
                    name = "chase",
                    infos = null
                });
                return true;
            }
            else
            {
                if (moveTime > 0)
                {
                    if (actionManager.GetActionState("Attack") == MDAction.ActionState.Disable && actionManager.GetActionState("Move") == MDAction.ActionState.Disable)//非运动状态
                    {
                        switch (direction)
                        {
                            case ATCharacterState.Direction.Left:
                                actionManager.Action("Move", "left");
                                break;

                            case ATCharacterState.Direction.Right:
                                actionManager.Action("Move", "right");
                                break;
                        }
                    }
                }
                else
                {
                    if (actionManager.GetActionState("Move") != MDAction.ActionState.Disable)//停止移动, 并决定下一次移动
                    {
                        actionManager.Stop("Move");
                    }
                    if (random.NextDouble() > 0.5)
                    {
                        direction = ATCharacterState.Direction.Right;
                    }
                    else
                    {
                        direction = ATCharacterState.Direction.Left;
                    }
                    moveTime = (float)random.NextDouble() * 0.2f + 0.2f;
                }
            }
            return false;
        }
    }
}