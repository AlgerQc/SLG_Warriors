﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace SLGame
{
    public enum LogColor
    {
        BLACK,
        RED,
        GREEN,
        BLUE,
        YELLOW,
        PURPLE,
    }

    public static class Helper
    {
        public static string ConvertToObsPath(string path)
        {
            if (path.StartsWith("Assets/"))
            {
                return path.Replace("Assets/", Application.dataPath + "/");
            }
            return path;
        }

        public static string ConverToRelativePath(string path)
        {
            return path.Replace(Application.dataPath + "/", "Assets/");
        }

        private static string GetColor(LogColor color)
        {
            switch (color)
            {
                case LogColor.BLACK:
                    return "<color=#000000>{0}</color>";
                case LogColor.RED:
                    return "<color=#FF0000>{0}</color>";
                case LogColor.GREEN:
                    return "<color=#00FF00>{0}</color>";
                case LogColor.BLUE:
                    return "<color=#0000FF>{0}</color>";
                case LogColor.YELLOW:
                    return "<color=#FFFF00>{0}</color>";
                case LogColor.PURPLE:
                    return "<color=#FF00FF>{0}</color>";
                default:
                    return "<color=#000000>{0}</color>";
            }
        }

        public static void Log(object msg, LogColor color = LogColor.BLACK)
        {
            if (color == LogColor.BLACK)
                Debug.Log(msg);
            else
                Debug.Log(string.Format(GetColor(color), msg.ToString()));
        }

        public static void LogError(object msg)
        {
            Debug.LogError(msg);
        }

        public static void LogWarning(object msg)
        {
            Debug.LogWarning(msg);
        }

        public static float CalcDistanceInXYAxis(float x1, float y1, float x2, float y2)
        {
            return Mathf.Sqrt(Mathf.Pow(x1 - x2, 2) + Mathf.Pow(y1 - y2, 2));
        }

        public static float CalcDistanceInXYAxis(Vector3 p1, Vector3 p2)
        {
            return CalcDistanceInXYAxis(p1.x, p1.z, p2.x, p2.z);
        }

        //检查两个Vector3的差异
        private static float changedThreshold = 0.03f;
        public static bool CheckVector3Changed(Vector3 v1, Vector3 v2)
        {
            if (Mathf.Abs(v1.x - v2.x) > changedThreshold)
                return true;

            if (Mathf.Abs(v1.y - v2.y) > changedThreshold)
                return true;

            if (Mathf.Abs(v1.z - v2.z) > changedThreshold)
                return true;

            return false;
        }

        public static string GetPersistentDataPath()
        {
            return Application.persistentDataPath;
        }

        //拷贝文件夹
        public static void CopyFolder(string _from, string _to)
        {
            // EUtilityHelperL.Log(string.Format("拷贝文件：从{0}到{1}", _from, _to));
            if (Directory.Exists(_from) == false)
            {
                Helper.LogError("拷贝文件失败：文件不存在(" + _from + ")");
                return;
            }

            if (Directory.Exists(_to) == false)
            {
                // EUtilityHelperL.Log(_to + "文件不存在，新建");
                Directory.CreateDirectory(_to);
            }

            //TODO:先这么写吧，能用就行，有机会再完善
            //拷贝.txt, .lua, .json
            string[] paths = Directory.GetFiles(_from, "*.*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < paths.Length; ++i)
            {
                paths[i] = paths[i].Replace('\\', '/');
                string fileName = paths[i].Remove(0, paths[i].LastIndexOf("/") + 1);
                File.Copy(paths[i], _to + "/" + fileName, true);
            }
            //            paths = Directory.GetFiles(_from, "*.json", SearchOption.TopDirectoryOnly);
            //            for(int i = 0; i < paths.Length; ++i)
            //            {
            //                paths[i] = paths[i].Replace('\\','/');
            //                string fileName = paths[i].Remove(0, paths[i].LastIndexOf("/") + 1);
            //                File.Copy(paths[i], _to + "/" + fileName, true);
            //            }
            //            paths = Directory.GetFiles(_from, "*.txt", SearchOption.TopDirectoryOnly);
            //            for(int i = 0; i < paths.Length; ++i)
            //            {
            //                paths[i] = paths[i].Replace('\\','/');
            //                string fileName = paths[i].Remove(0, paths[i].LastIndexOf("/") + 1);
            //                File.Copy(paths[i], _to + "/" + fileName, true);
            //            }

            //拷贝文件夹
            string[] folderPaths = Directory.GetDirectories(_from, "*", SearchOption.TopDirectoryOnly);
            for (int i = 0; i < folderPaths.Length; ++i)
            {
                folderPaths[i] = folderPaths[i].Replace('\\', '/');
                string folderName = folderPaths[i].Remove(0, folderPaths[i].LastIndexOf("/") + 1);
                //递归拷贝
                CopyFolder(folderPaths[i], _to + "/" + folderName);
            }
        }

        //获取文件名字
        public static string GetFileName(string _path, bool hideBack = true)
        {
            int start = _path.LastIndexOf('/');

            if (start >= 0)
            {
                string _temp = _path.Remove(0, start + 1);
                int end = _temp.IndexOf('.');
                if (end >= 0 && hideBack)
                    return _temp.Remove(end);
            }
            Helper.LogError("获取文件名称失败");
            return "";
        }

        //移除文件后缀
        public static string RemoveSuffix(string path)
        {
            return path.Substring(0, path.LastIndexOf('.'));
        }

        //去掉名字中的clone
        public static string RemoveNameClone(string oldName)
        {
            return oldName.Replace("(Clone)", "");
        }

        //返回是否设置成功
        public static bool SetClass<T>(ref T cls, object value)
            where T : Object
        {
            if (value == null)
            {
                if (cls == null)
                    return false;

                cls = null;
                return true;
            }
            else
            {
                if (!cls || cls.GetInstanceID() != ((T)value).GetInstanceID())
                {
                    cls = (T)value;
                    return true;
                }
                return false;
            }
        }

        //判断代码运行时间
        private static long timerStart = 0;
        public static void TimerStart()
        {
            timerStart = System.DateTime.Now.Ticks;
        }

        public static float TimerEnd()
        {
            float timeCost = (System.DateTime.Now.Ticks - timerStart) / 10000;
            timerStart = System.DateTime.Now.Ticks;
            //返回毫秒
            return timeCost;
        }

        public static string AssetPath
        {
            get
            {
                return Application.streamingAssetsPath;
            }
        }

        public static string CalcMD5(string path)
        {
            try
            {
                FileStream file = new FileStream(path, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (System.Exception ex)
            {
                throw new System.Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }

        public static void DeleteGameObject(GameObject gameObj)
        {
            if (gameObj == null)
                return;

        }

        public static long GetFileSize(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (fileInfo != null)
                return fileInfo.Length;

            return 0L;
        }

        public static void Normalize(this Transform transform)
        {
            transform.localPosition = Vector3.zero;
            transform.localScale = Vector3.one;
            transform.localRotation = Quaternion.identity;
        }

        public static void Normalize(this RectTransform rectTransform)
        {
            rectTransform.localPosition = Vector3.zero;
            rectTransform.localScale = Vector3.one;
            rectTransform.offsetMax = Vector3.zero;
            rectTransform.offsetMin = Vector3.zero;


        }
    }
}
