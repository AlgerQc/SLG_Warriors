using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SLGame
{
    public class Check 
    {
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
                int skill1 = skillCombo.Dequeue();
                int skill2 = skillCombo.Dequeue();
                for (int i = 0; i < ConfigReader.comboInfoDic.Count; i++)
                {
                    if (ConfigReader.comboInfoDic[i].skill1 == skill1 && ConfigReader.comboInfoDic[i].skill2 == skill2)
                    {
                        UtilityHelper.Log("We are in combo stage 2");
                        skillCombo.Enqueue(skill1);
                        skillCombo.Enqueue(skill2);
                        return i;
                    }
                }
            }
            else if (skillCombo.Count == 1)
            {
                int skill1 = skillCombo.Dequeue();
                for (int i = 0; i < ConfigReader.comboInfoDic.Count; i++)
                {
                    if (ConfigReader.comboInfoDic[i].skill1 == skill1)
                    {
                        UtilityHelper.LogFormat("We are in combo stage 1");
                        skillCombo.Enqueue(skill1);
                        return i;
                    }
                }
            }

            return -1;
        }

    }
}


