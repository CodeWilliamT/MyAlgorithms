using namespace std;
#include "..\myHeader.h"
typedef pair<int, int> pii;

class DFSBasic {
#define MAXN 6000
#define MAXM 2
    struct Node {
        int x;
        int y;
        string path;
        bool operator==(Node const& a) const {
            return a.x == x && a.y == y;
        }
    };
public:
    void DFS()
    {
        vector<vector<int>> g;
        int n = g.size();
        int m = g[0].size();
        bool v[MAXN * MAXM + MAXM + 1]{};
        auto judge = [&](Node& nd) {//处理特殊边界,能下一步则返回true
            return nd.x >= 0 && nd.x <= MAXN && nd.y >= 0 && nd.y <= MAXM;
        };
        auto hash= [&](Node& nd) {//处理特殊边界,能下一步则返回true
            return nd.x*m+nd.y;
        };
        function<int(Node)> dfs = [&](Node cur) {
            //搜可行解到达递归边界便跳出
            if (v[hash(cur)] && !judge(cur))//判定状态可行性，若状态不可行，则跳过
                return 0;
            if (cur.x == n)//记方案数则答案++，遍历整棵解答树
                return 1;
            Node next;
            for (int i = 0; i < 4; i++) {//遍历状态转移操作选项,n为可进行操作数
                next = cur;//计算下一状态；
                if (v[hash(next)] && !judge(next))//判定状态可行性，若状态不可行，则跳过
                    continue;
                //状态推进; 剪枝标记记为1;
                dfs(next); //进行下一重递归
                //标记还原;准备下一动作尝试
                //状态还原;状态还原为未进行动作时
            }
        };
    }
};