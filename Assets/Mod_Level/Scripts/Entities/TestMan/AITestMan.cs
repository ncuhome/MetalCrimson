using ER.Entity2D;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

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
                if (entity.gameObject.tag != "Player") return;
                if (eye.record.Contains(entity))
                {
                    seePlayer = true;
                    if(player == null) player = entity;
                    Debug.Log("看见玩家");
                }
                else
                {
                    Debug.Log("玩家消失");
                    player = null;
                    for(int i=0;i<eye.record.Count;i++)
                    {
                        if (eye.record[i].gameObject.tag == "Player")
                        {
                            player = eye.record[i];
                            break;
                        }
                    }
                    if(player == null)
                    {
                        seePlayer =false;
                    }
                }
            };
        }
        System.Random random = new System.Random(DateTime.Now.Millisecond);
        protected override bool ParseToDo(ToDo toDo)
        {
            switch(toDo.name)
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
            Debug.Log($"waitTime:{waitTime}");
            if(waitTime <= 0)
            {
                waitTime = (float)random.NextDouble()*8 + 3;
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
                if(moveTime>0)
                {
                    if(actionManager.GetActionState("Move") == MDAction.ActionState.Disable)//非运动状态
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
                    if(actionManager.GetActionState("Move") != MDAction.ActionState.Disable)//停止移动, 并决定下一次移动
                    {
                        actionManager.Stop("Move");
                    }
                    if(random.NextDouble() > 0.5)
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
            if((p_pos-s_pos).magnitude < attack_distance)
            {
                actionManager.Stop("Move");
                ToDoList.Add(new ToDo()
                {
                    name = "attack",
                    infos = null
                });
                return true;
            }

            if (p_pos.x < s_pos.x)//玩家在左侧
            {
                actionManager.Action("Move", "left");
            }
            else
            {
                actionManager.Action("Move", "right");
            }
            return false;
        }
        private float attack_distance = 5f;//攻击距离;
        private bool DoAttack(Dictionary<string, object> infos)
        {
            if (state.Dead) return true;
            if (!state.ControlAct) return false;
            actionManager.Action("Attack");
            return true;
        }
    }
}