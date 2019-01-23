using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Astar : MonoBehaviour
{
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private int _detectionLimitRange;  //이거 없으면 맵이 거대하거나 경계선이 없을때 겜 쥬금

    class 거리노드
    {
        public 거리노드() {}
        public 거리노드(Vector3Int _위치, Vector3Int _이전위치, float _이동한거리, float _이동할거리)
        {
            위치        = _위치;
            이전위치    = _이전위치;
            이동한거리  = _이동한거리;
            이동할거리  = _이동할거리;
        }

        public Vector3Int 위치;
        public Vector3Int 이전위치;
        public float 이동한거리;
        public float 이동할거리;
        public float 예상최종거리() { return 이동한거리 + 이동할거리; }
    }
    List<거리노드> 거리목록 = new List<거리노드>();
    Dictionary<Vector3Int, 거리노드> 확정거리목록 = new Dictionary<Vector3Int, 거리노드>();
    Vector3Int 출발지;
    Vector3Int 도착지;


    public class 최단경로노드
    {
        public 최단경로노드(Vector3Int _타일위치, Vector3 _월드위치)
        {
            타일위치 = _타일위치;
            월드위치 = _월드위치;
        }

        public Vector3Int 타일위치;
        public Vector3 월드위치;
    }
    List<최단경로노드> 최단경로목록 = new List<최단경로노드>();


    void 거리갱신(Vector3Int 이전지점, Vector3Int 현재지점, float 이동한거리)
    {
        float 이동할거리 = Vector3Int.Distance(현재지점, 도착지);
        float 예상최종거리 = 이동한거리 + 이동할거리;
        거리노드 정보 = new 거리노드(현재지점, 이전지점, 이동한거리, 이동할거리);

        //상술했지만 이거 없으면 맵이 거대하거나 경계선이 없을때 겜 쥬금
        if (이동할거리 > _detectionLimitRange)
            return;

        if (현재지점 == 출발지)
            return;
        //주변정보갱신() 에서 미리 처리되도록 되어있음.
        //구조 맘에안드는데
        //if (tilemap.GetTile(현재지점) != null)
        //    return;
        if (확정거리목록.ContainsKey(현재지점))
            return;

        for (int cccb = 0; cccb < 거리목록.Count; cccb++)
        {
            if (거리목록[cccb].위치 == 현재지점)
            {
                if (예상최종거리 < 거리목록[cccb].예상최종거리())
                    거리목록[cccb] = 정보;
                return;
            }
        }

        int 목록우선도 = 0;
        foreach (거리노드 목록정보 in 거리목록)
        {
            if (예상최종거리 <= 목록정보.예상최종거리())
                break;
            목록우선도++;
        }

        거리목록.Insert(목록우선도, 정보);
    }

    bool 최단거리지점(ref 거리노드 최단거리정보)
    {
        bool 더이상_이동할_수_있는_경로가_없습니다 = false;
        bool 다음_경로를_찾았습니다 = true;


        if (거리목록.Count == 0)
            return 더이상_이동할_수_있는_경로가_없습니다;

        최단거리정보 = 거리목록[0];
        거리목록.RemoveAt(0);
        return 다음_경로를_찾았습니다;
    }

    void 거리정보확정(거리노드 거리정보) { 확정거리목록.Add(거리정보.위치, 거리정보); }
    bool 목적지확인(거리노드 거리정보) { return 거리정보.위치 == 도착지; }
    void 주변정보갱신(Vector3Int 현재위치, float 이동한거리)
    {
        //으악
        bool l = tilemap.GetTile(현재위치 + Vector3Int.left ) == null;    if (l)                    거리갱신(현재위치, 현재위치 + Vector3Int.left , 이동한거리 + 1);
        bool r = tilemap.GetTile(현재위치 + Vector3Int.right) == null;    if (r)                    거리갱신(현재위치, 현재위치 + Vector3Int.right, 이동한거리 + 1);
        bool u = tilemap.GetTile(현재위치 + Vector3Int.up   ) == null;    if (u)                    거리갱신(현재위치, 현재위치 + Vector3Int.up   , 이동한거리 + 1);
        bool d = tilemap.GetTile(현재위치 + Vector3Int.down ) == null;    if (d)                    거리갱신(현재위치, 현재위치 + Vector3Int.down , 이동한거리 + 1);
        if (l && u && tilemap.GetTile(현재위치 + Vector3Int.left + Vector3Int.up     ) == null)     거리갱신(현재위치, 현재위치 + Vector3Int.left + Vector3Int.up     , 이동한거리 + 1.4145f);
        if (l && d && tilemap.GetTile(현재위치 + Vector3Int.left + Vector3Int.down   ) == null)     거리갱신(현재위치, 현재위치 + Vector3Int.left + Vector3Int.down   , 이동한거리 + 1.4145f);
        if (r && u && tilemap.GetTile(현재위치 + Vector3Int.right + Vector3Int.up    ) == null)     거리갱신(현재위치, 현재위치 + Vector3Int.right + Vector3Int.up    , 이동한거리 + 1.4145f);
        if (r && d && tilemap.GetTile(현재위치 + Vector3Int.right + Vector3Int.down  ) == null)     거리갱신(현재위치, 현재위치 + Vector3Int.right + Vector3Int.down  , 이동한거리 + 1.4145f);
    }

    bool 최단경로생성(Vector3Int _도착지, ref 거리노드 도착지정보)
    {
        bool 도달할_수_없는_지역입니다 = false;
        bool 최단경로를_찾았습니다 = true;


        거리목록.Clear();
        확정거리목록.Clear();
        출발지 = tilemap.WorldToCell(transform.position);
        도착지 = _도착지;



        if (출발지 == 도착지)
        {
            도착지정보 = null;
            return 최단경로를_찾았습니다;
        }


        주변정보갱신(출발지, 0);
        while (true)
        {
            거리노드 다음경로 = new 거리노드();
            if (!최단거리지점(ref 다음경로))
                return 도달할_수_없는_지역입니다;

            if (목적지확인(다음경로))
            {
                도착지정보 = 다음경로;
                return 최단경로를_찾았습니다;
            }
            거리정보확정(다음경로);
            주변정보갱신(다음경로.위치, 다음경로.이동한거리);
        }
    }

    public bool 경로검색(Vector3Int _도착지)
    {
        최단경로목록.Clear();

        거리노드 도착지정보 = new 거리노드();
        if (!최단경로생성(_도착지, ref 도착지정보))
            return false;

        if (도착지정보 == null)
            return true;
        최단경로목록.Insert(0, new 최단경로노드(도착지정보.위치, tilemap.CellToWorld(도착지정보.위치) + new Vector3(tilemap.cellSize.x * 0.5f, tilemap.cellSize.y * 0.5f, 0)));

        Vector3Int 현재위치 = 도착지정보.이전위치;
        거리노드 이전위치;
        while (확정거리목록.TryGetValue(현재위치, out 이전위치))
        {
            최단경로목록.Insert(0, new 최단경로노드(이전위치.위치, tilemap.CellToWorld(이전위치.위치) + new Vector3(tilemap.cellSize.x * 0.5f, tilemap.cellSize.y * 0.5f, 0)));
            현재위치 = 이전위치.이전위치;
        }
        return true;
    }
    public bool 경로검색(Vector3 _도착지) { return 경로검색(tilemap.WorldToCell(_도착지)); }


    public List<최단경로노드> GetPath() { return 최단경로목록; }



    private void Update()
    {
        Debug.ClearDeveloperConsole();
        if (경로검색(tilemap.WorldToCell(GameObject.FindWithTag("Player").transform.position)))
        {
            Vector3 prevPos = transform.position;
            Vector3 nextPos;
            foreach (최단경로노드 node in 최단경로목록)
            {
                nextPos = node.월드위치;
                Debug.DrawLine(prevPos, nextPos);
                prevPos = nextPos;
            }
        }
        else
            Debug.Log("경로탐색 실패");
    }
}

//가장 기름값 안드는 길 찾는 방법
// 1. 요약하기 힘들다 인터넷 봐라 http://egloos.zum.com/cozycoz/v/9748811
