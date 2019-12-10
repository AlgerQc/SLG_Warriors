using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLGame
{
    public class Check 
    {
        /*300  力-力-力  攻击力提升  
          030  速-速-速  bp+3
          003  技-技-技  命中+闪避+暴击   

          201  2力 + 1技 攻击力小幅提升2回合 + 触发第三下特殊伤害：推拉、流血等等
          210  2力 + 1速 攻击力小幅提升2回合 + 触发第三下特殊伤害：眩晕、禁足、减速等等
          102  2技 + 1力 命中/闪避/暴击小幅提升 + 触发第三次技能改变为暴击伤害
          012  2技 + 1速 命中/闪避/暴击小幅提升 + 触发第三次技能改变为强制优势伤害
          120  2速 + 1力 bp+1 + 二次攻击
          021  2速 + 1技 bp+1 + 二次移动
          111  1力 + 1速 + 1技 伤害免疫盾
         */
        public static int checkCombo(Queue<int> skillCombo)
        {
            Queue<int> tmp = skillCombo;
            int skill1 = tmp.Dequeue();
            int skill2 = tmp.Dequeue();
            int skill3 = tmp.Dequeue();
            for (int i = 0; i < ConfigReader.comboInfoDic.Count; i++)
            {
                if (ConfigReader.comboInfoDic[i].skill1 == skill1 && ConfigReader.comboInfoDic[i].skill2 == skill2 &&
                    ConfigReader.comboInfoDic[i].skill3 == skill3)
                {
                    return i;
                }
            }

            return 0;
        }

        public static int checkComboEarly(Queue<int> skillCombo)
        {
            if (skillCombo.Count == 2)
            {   
                /*
                int skill1 = skillCombo.Dequeue();
                int skill2 = skillCombo.Dequeue();
                for (int i = 0; i < ConfigReader.comboInfoDic.Count; i++)
                {
                    if (ConfigReader.comboInfoDic[i].skill1 == skill1 && ConfigReader.comboInfoDic[i].skill2 == skill2)
                    {

                        skillCombo.Enqueue(skill1);
                        skillCombo.Enqueue(skill2);
                        return i;
                    }
                }
                */
                UtilityHelper.Log("We are in combo stage 2");
            }
            else if (skillCombo.Count == 1)
            {
                /*
                int skill1 = skillCombo.Dequeue();
                for (int i = 0; i < ConfigReader.comboInfoDic.Count; i++)
                {
                    if (ConfigReader.comboInfoDic[i].skill1 == skill1)
                    {

                        skillCombo.Enqueue(skill1);
                        return i;
                    }
                }
                */
                UtilityHelper.LogFormat("We are in combo stage 1");
            }

            return -1;
        }

    }
}


