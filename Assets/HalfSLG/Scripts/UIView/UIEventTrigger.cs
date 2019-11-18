using UnityEngine;
using UnityEngine.EventSystems;

namespace SLGame
{
    public class UIeventTrigger : EventTrigger
    {

        public override void OnPointerEnter(PointerEventData eventData)//鼠标进入
        {
            base.OnPointerEnter(eventData);
            print("进入了");
        }
        public override void OnPointerExit(PointerEventData eventData)//鼠标移出
        {
            base.OnPointerExit(eventData);
            print("出来了");
        }

        Vector3 pos1;//开始的位置
        Vector3 pos2;//结束的位置
        float distance;//鼠标滑动的距离

        public override void OnBeginDrag(PointerEventData eventData)//开始拖拽
        {
            base.OnBeginDrag(eventData);
            print("开始拖拽时间：" + Time.time);
            pos1 = Input.mousePosition;
        }
        public bool isdrage;
        public override void OnDrag(PointerEventData eventData)//鼠标拖拽
        {
            base.OnDrag(eventData);
            print("拖得动吗");
            isdrage = true;
            print("拖。。。" + Time.time);
        }
        public override void OnEndDrag(PointerEventData eventData)//鼠标结束拖拽
        {
            base.OnEndDrag(eventData);
            pos2 = Input.mousePosition;
            distance = Vector3.Distance(pos1, pos2);
            Vector3 op = pos2 - pos1;
            print("在x方向拖动距离：" + (pos2.x - pos1.x));
            print("在y方向的距离：" + (pos2.y - pos1.y));
            print("鼠标拖拽的距离1：" + distance);
            print("鼠标拖拽的距离2：" + op.magnitude);
            print("结束拖拽时间：" + Time.time);
        }

        public override void OnPointerUp(PointerEventData eventData)//鼠标抬起
        {
            base.OnPointerUp(eventData);
            isdrage = false;
        }
        public override void OnDrop(PointerEventData eventData)//？还不清楚有什么用处
        {
            base.OnDrop(eventData);
        }
        public override void OnInitializePotentialDrag(PointerEventData eventData)//拖拽前的初始化
        {
            base.OnInitializePotentialDrag(eventData);
            print("初始化~~~拖拽" + Time.time);
        }
        public override void OnPointerClick(PointerEventData eventData)//鼠标点击
        {
            base.OnPointerClick(eventData);
        }

        public override void OnScroll(PointerEventData eventData)//鼠标滚轮
        {
            base.OnScroll(eventData);
            print("滚了么");
        }

        // Use this for initialization
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {
            if (isdrage == true)
            {
                this.transform.position = Input.mousePosition;
            }
        }

    }
}