using System.Collections.Generic;
using UnityEngine;

namespace ArcanaDungeon.Object
{
    public abstract class Thing : MonoBehaviour
    {
        protected int hp;
        public int maxhp = 100;
        protected int stamina;
        public int maxstamina = 100;
        public int power;

        public bool exhausted = false;
        protected int block;
        protected int vision_distance;
        public int isTurn;  //1 이상일 경우 이 객체의 턴이다, 0일 경우 단순히 이 객체의 턴이 아닌 것이며, 음수일 경우 기절 등의 이유로 턴이 생략될 것이다

        public List<int> route_pos = new List<int>();  //목적지까지의 이동 경로, 이동은 항상 route_pos[0]으로 이동해서 진행된다

        private Dictionary<int, int> condition;  //상태이상 및 버프 표시, key는 상태이상 종류이며 value는 지속시간, key에 따른 효과 : 0=연소 / 1=기절 / 2=급류 / 3=중독 / 4 = 풀묶기(구현필요) / 5 = 약화

        new public string name;

        public Thing()
        {
            condition = new Dictionary<int, int>();
            this.vision_distance = 6;
            this.hp = 0;
            this.stamina = 0;
        }

        public abstract void Spawn();

        //hp 관련 함수
        public int GetHp()
        {
            return this.hp;
        }

        public void HpChange(int val)
        {
            //new void HpChange(val){
            //  //체력을 1 잃을 때마다 힘 1 증가
            //  super.HpChange(val);
            if (val < 0)
            {
                BlockChange(val);
                if (block + val < 0) { val += block; } else { val = 0; }
            }

            if (val > 0)
            {
                this.hp = Mathf.Clamp(this.hp + val, 0, this.maxhp);
            }
            else
            {

                this.hp += val;
                if (this.hp <= 0)
                {
                    this.die();
                }
            }
            UI.uicanvas.GaugeChange();  //플레이어 체력/스태미나/방어도 갱신, player에 들어가야 하는데 반복되는 부분이 좀 많아 그냥 여기에 끼워넣음
        }
        public void be_hit(int val)
        {
            UI.uicanvas.blood(transform.position);
            HpChange(-val);
            if (this.condition.ContainsKey(5))
            {
                if (this.condition[5] > 0)
                {
                    this.HpChange(this.condition[5] / 10); // this.condition[5]를 십으로 나눈 몫이 약화 딜값
                }
            }
        }
        public void be_fired(int val)
        {
            UI.uicanvas.fire(transform.position);
            HpChange(-val);
        }
        public void be_poisoned(int val)
        {
            UI.uicanvas.poison(transform.position);
            HpChange(-val);
        }

        //stamina 관련 함수
        public int GetStamina()
        {
            return stamina;
        }

        public void StaminaChange(int val)
        {
            if (val > 0)
            {
                if (this.stamina + val > this.maxstamina)
                {
                    this.stamina = this.maxstamina;
                }
                else
                {
                    this.stamina += val;
                }
            }
            else
            {
                this.stamina += val;
                if (this.stamina < 0)
                {
                    this.stamina = 0;
                }
            }
            UI.uicanvas.GaugeChange();
        }

        //block 관련 함수
        public int GetBlock()
        {
            return this.block;
        }

        public void BlockChange(int val)
        {
            if (val > 0)
            {
                this.block += val;
            }
            else
            {
                if (this.block + val < 0)
                {
                    this.block = 0;
                }
                else
                {
                    this.block += val;
                }
            }
        }

        //★이동 관련 함수, 현재 필요 없어보이니 태종이가 검토 후 삭제할 것
        public void move() { }

        public void route_BFS(int dest_x, int dest_y)    //넓이 우선 탐색으로 목적지까지의 경로를 route_pos에 저장해주는 함수
        {
            //route_BFS에서의 좌표는 x+y*(맵 너비)로 나타낸다. BFS 알고리즘을 최대한 간략하게 구현하기 위해 부득이하게 좌표를 int 변수 1개로 나타낸 것이다
            int destination = dest_x + dest_y * Dungeon.dungeon.currentlevel.width;
            Queue<int> checking = new Queue<int>();
            int[] prev = new int[Dungeon.dungeon.currentlevel.length];
            int[] dir = new int[] { -1, Dungeon.dungeon.currentlevel.width, 1, -Dungeon.dungeon.currentlevel.width, -1 + Dungeon.dungeon.currentlevel.width, 1 + Dungeon.dungeon.currentlevel.width, 1 - Dungeon.dungeon.currentlevel.width, -1 - Dungeon.dungeon.currentlevel.width };

            checking.Enqueue((int)(transform.position.x + transform.position.y * Dungeon.dungeon.currentlevel.width));
            int temp, temp2;
            while (checking.Count > 0)
            {
                //주변 좌표 포함 시 확인해야 하는 것 : 자기 자신의 cur_pos가 아닌가, passable인가?, level의 length 범위 이내의 숫자인가, prev[i]==null (이미 체크한 칸)인가, 다른 몬스터와 겹치지 않는가
                for (int ii = 0; ii < 8; ii++)
                {
                    temp = checking.Peek() + dir[ii];
                    //Debug.Log((temp % Dungeon.dungeon.currentlevel.width) + " / " + (temp / Dungeon.dungeon.currentlevel.width));
                    if ((transform.position.x + transform.position.y * Dungeon.dungeon.currentlevel.width != temp) &
                            ((Terrain.thing_tag[Dungeon.dungeon.currentlevel.map[temp % Dungeon.dungeon.currentlevel.width, temp / Dungeon.dungeon.currentlevel.width]] & Terrain.passable) != 0) &
                            (temp > 0 & temp < Dungeon.dungeon.currentlevel.length) &
                            (prev[temp] == 0) &
                            (Dungeon.dungeon.find_enemy(temp % Dungeon.dungeon.currentlevel.width, temp / Dungeon.dungeon.currentlevel.width) == null))
                    {
                        checking.Enqueue(temp);
                        prev[temp] = checking.Peek();
                    }
                }

                //Plr_pos[0]이랑 같은 좌표인지 확인, 맞으면 prev 배열 쭉 타고올라가면서 route_pos에 저장
                if (checking.Peek() == destination)
                {
                    temp2 = checking.Peek();
                    route_pos.Clear();
                    while (prev[temp2] != 0)
                    {
                        route_pos.Insert(0, temp2);
                        temp2 = prev[temp2];
                    }
                    break;
                }
                checking.Dequeue();
            }

            return;
        }

        //상태이상 처리 관련 함수
        public void condition_process() // 번호에 각 상태이상 이름 및 효과 기재바람.
        {
            if (this.condition.ContainsKey(0))
            {    //연소 - 고정된 양의 피해를 일정 턴동안 받는다. 중독에 비해 초기 수치가 높아야 한다. 물에 접촉 시 즉시 해제되어야 한다.
                if (this.condition[0] > 0)
                {
                    be_fired(10);
                    this.condition[0] -= 1;
                }
            }
            if (this.condition.ContainsKey(1))
            {    //기절 - 1턴동안 행동할 수 없다.(무한스턴 방지용 대책이 필요할수 있음)
                if (this.condition[1] > 0)
                {
                    this.isTurn -= 1;
                    this.condition[1] -= 1;
                }
            }
            if (this.condition.ContainsKey(2))
            {    //급류
                if (this.condition[2] > 0)
                {
                    StaminaChange(15);
                    this.condition[2] -= 1;
                }
            }
            if (this.condition.ContainsKey(3))
            {    //중독 - 중첩형 상태이상. 중첩 횟수와 같은 양의 피해를 받고, 중첩이 1 감소한다. 이렇게 중첩이 0이 될 경우, 중독이 해제된다.
                if (this.condition[3] > 0)
                {
                    be_poisoned(condition[3]);
                    this.condition[3] -= 1;
                }
            }
            if (this.condition.ContainsKey(5))
            {    // 약화는 일의 자리는 남은 턴수 10으로 나눈 몫은 데미지값
                if (this.condition[5] > 0)
                {
                    if(this.condition[5] % 10 == 9)
                    {
                        this.condition[5] = 0;
                    }
                    if (this.condition[5] % 10 > 3) // 14 
                    {
                        this.condition[5] = (this.condition[5] / 10) * 10 + 3; // 12
                    }
                    else if (this.condition[5] % 10 > 0)
                    {
                        this.condition[5] -= 1; // 11 10
                    }
                    else
                        this.condition[5] = 0; // 0
                    Debug.Log("적 약화 값 " + this.condition[5]);
                }
            }
            List<int> temp = new List<int>(this.condition.Keys);
            foreach (int i in temp)
            {
                if (this.condition[i] <= 0)
                {
                    this.condition.Remove(i);
                }
            }
        }

        public void condition_add(int key, int val)
        { // key는 상태이상 번호, val은 상태이상 수치. 중독 2일 경우, key는 3, val은 2
            if (condition.ContainsKey(key))
            {
                condition[key] += val;
            }
            else
            {
                condition.Add(key, val);
            }
        }

        public Dictionary<int, int> GetCondition()
        {
            return this.condition;
        }



        public abstract void die();//★나중에 자기자신을 map[]에서 삭제하는 정도는 넣어두자

        public void Turnend()
        {
            if(this.gameObject.tag == "Player")
                Dungeon.dungeon.Plr.isturn_start = true;
            this.condition_process();
            this.StaminaChange(5);
            this.isTurn -= 1;
        }

        public List<float[]> range_check(float dest_x, float dest_y)
        {
            //이 몬스터의 현재 좌표부터 (dest_x,dest_y)까지 맞닿는 사각형 좌표들을 구해옴
            List<float[]> result = new List<float[]>();
            //목표 지점과 몬스터 사이가 수평 일직선이라면 x좌표만 바꿔가며 result에 저장
            if (dest_y - transform.position.y == 0)
            {
                int temp_var = dest_x - transform.position.x > 0 ? 1 : -1;
                for (float i = transform.position.x; i * temp_var <= dest_x * temp_var; i += temp_var)
                {
                    result.Add(new float[2] { i, dest_y });
                }
                //목표 지점과 몬스터 사이가 수직 일직선이라면 y좌표만 바꿔가며 result에 저장
            }
            else if (dest_x - transform.position.x == 0)
            {
                int temp_var = dest_y - transform.position.y > 0 ? 1 : -1;
                for (float i = transform.position.y; i * temp_var <= dest_y * temp_var; i += temp_var)
                {
                    result.Add(new float[2] { dest_x, i });
                }
                //일직선이 아니라면 
            }
            else
            {
                float x_cur = transform.position.x; float y_cur = transform.position.y; //현재 확인 중인 좌표, 타입 오류 나면 float으로 변경할 것
                float slope = (dest_x - x_cur) / (dest_y - y_cur);
                int x_gap = dest_x - x_cur > 0 ? 1 : -1;
                int y_gap = dest_y - y_cur > 0 ? 1 : -1;
                float y_changing_at_x = x_cur + y_gap * slope / 2 + x_gap * 0.5f; //y좌표가 변할 때의 x좌표값, 소수점 아래가 0.0 ~ 0.5 라면 아래 for문들 조건문에서 해당 칸의 정사각형을 지나지만 해당 칸의 좌표값보다는 작다, 따라서 기준인 y_changin_x에 0.5f를 더해 기준을 좀 높여 놓는다
                //첫 y좌표 0.5 변화하는 동안
                for (; x_cur * x_gap < y_changing_at_x * x_gap; x_cur += x_gap)
                {
                    result.Add(new float[2] { x_cur, y_cur });
                }
                y_cur += y_gap;
                //중간
                for (; y_cur * y_gap < dest_y * y_gap; y_cur += y_gap)
                {
                    if (y_changing_at_x % 1 != 0)
                    {
                        result.Add(new float[2] { x_cur - x_gap, y_cur });
                    }
                    y_changing_at_x += y_gap * slope;
                    for (; x_cur * x_gap < y_changing_at_x * x_gap; x_cur += x_gap)
                    {
                        result.Add(new float[2] { x_cur, y_cur });
                    }
                }
                //마지막 y좌표가 0.5 변화하는 동안
                if (y_changing_at_x % 1 != 0)
                {
                    result.Add(new float[2] { x_cur - x_gap, y_cur });
                }
                for (; x_cur * x_gap <= dest_x * x_gap; x_cur += x_gap)
                {
                    result.Add(new float[2] { x_cur, y_cur });
                }
            }
            return result;
        }
        public Thing range_attack(int dest_x, int dest_y, bool by_player)   //원거리 공격, ★벽을 사이에 두고 공격 시 공격이 불가능하도록 수정 필요
        {
            //이 몬스터의 현재 좌표부터 (dest_x,dest_y)까지 맞닿는 사각형 좌표들을 구해옴
            List<float[]> result = new List<float[]>();
            //목표 지점과 몬스터 사이가 수평 일직선이라면 x좌표만 바꿔가며 result에 저장
            if (dest_y - transform.position.y == 0)
            {
                int temp_var = dest_x - transform.position.x > 0 ? 1 : -1;
                for (float i = transform.position.x; i * temp_var <= dest_x * temp_var; i += temp_var)
                {
                    result.Add(new float[2] { i, dest_y });
                }
            }
            //목표 지점과 몬스터 사이가 수직 일직선이라면 y좌표만 바꿔가며 result에 저장
            else if (dest_x - transform.position.x == 0)
            {
                int temp_var = dest_y - transform.position.y > 0 ? 1 : -1;
                for (float i = transform.position.y; i * temp_var <= dest_y * temp_var; i += temp_var)
                {
                    result.Add(new float[2] { dest_x, i });
                }
            }
            //일직선이 아니라면 
            else
            {
                float x_cur = transform.position.x; float y_cur = transform.position.y; //현재 확인 중인 좌표, 타입 오류 나면 float으로 변경할 것
                float slope = (dest_x - x_cur) / (dest_y - y_cur);
                int x_gap = dest_x - x_cur > 0 ? 1 : -1;
                int y_gap = dest_y - y_cur > 0 ? 1 : -1;
                float y_changing_at_x = x_cur + y_gap * slope / 2 + x_gap * 0.5f; //y좌표가 변할 때의 x좌표값, 소수점 아래가 0.0 ~ 0.5 라면 아래 for문들 조건문에서 해당 칸의 정사각형을 지나지만 해당 칸의 좌표값보다는 작다, 따라서 기준인 y_changin_x에 0.5f를 더해 기준을 좀 높여 놓는다
                //첫 y좌표 0.5 변화하는 동안
                for (; x_cur * x_gap < y_changing_at_x * x_gap; x_cur += x_gap)
                {
                    result.Add(new float[2] { x_cur, y_cur });
                }
                y_cur += y_gap;
                //중간
                for (; y_cur * y_gap < dest_y * y_gap; y_cur += y_gap)
                {
                    if (y_changing_at_x % 1 != 0)
                    {
                        result.Add(new float[2] { x_cur - x_gap, y_cur });
                    }
                    y_changing_at_x += y_gap * slope;
                    for (; x_cur * x_gap < y_changing_at_x * x_gap; x_cur += x_gap)
                    {
                        result.Add(new float[2] { x_cur, y_cur });
                    }
                }
                //마지막 y좌표가 0.5 변화하는 동안
                if (y_changing_at_x % 1 != 0)
                {
                    result.Add(new float[2] { x_cur - x_gap, y_cur });
                }
                for (; x_cur * x_gap <= dest_x * x_gap; x_cur += x_gap)
                {
                    result.Add(new float[2] { x_cur, y_cur });
                }
            }

            //가장 가까운 Thing을 찾아 반환한다
            Thing closest = null;
            int closest_distance = 999;
            foreach (float[] r in result)
            {
                //몬스터 체크
                foreach (GameObject t in Dungeon.dungeon.enemies[Dungeon.dungeon.currentlevel.floor - 1])
                {
                    if (t != this.gameObject & r[0] == t.transform.position.x & r[1] == t.transform.position.y & Dungeon.distance_cal(transform, t.transform) < closest_distance)
                    {
                        closest = t.GetComponent<Enemy>();
                        closest_distance = Dungeon.distance_cal(transform, t.transform);
                    }
                }
                //플레이어 체크
                if (!by_player & r[0] == Dungeon.dungeon.Plr.transform.position.x & r[1] == Dungeon.dungeon.Plr.transform.position.y & Dungeon.distance_cal(transform, Dungeon.dungeon.Plr.transform) < closest_distance)
                {
                    closest = Dungeon.dungeon.Plr;
                    closest_distance = Dungeon.distance_cal(transform, Dungeon.dungeon.Plr.transform);
                }
                //passable 체크
                if ((Dungeon.dungeon.currentlevel.map[(int)r[0], (int)r[1]] & Terrain.passable) == 0)
                {
                    closest = null;
                }
            }

            if (closest != null) { UI.uicanvas.range_shot(this.gameObject, closest.gameObject); }
            return closest;
        }
    }
}
