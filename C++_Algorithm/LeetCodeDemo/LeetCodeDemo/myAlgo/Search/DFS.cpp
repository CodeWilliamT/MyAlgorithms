using namespace std;
#include "..\myHeader.h"
typedef pair<int, int> pii;
class DFSBasic {
#define MAXN 6000
#define MAXM 2
public:
    void DFS()
    {
        vector<vector<int>> g;
        int n = g.size();
        int m = g[0].size();
        bool v[MAXN * MAXM + MAXM + 1]{};
        auto judge = [&](int& nd) {//��������߽�,����һ���򷵻�true
            return nd==n;
        };
        auto hash = [&](int& nd) {//��������߽�,����һ���򷵻�true
            return nd;
        };
        function<int(int)> dfs = [&](int cur) {
            if (v[hash(cur)] && !judge(cur))//�ж�״̬�����ԡ��߽硢ȥ�أ���״̬�����У�������
                return 0;
            if (cur== n)
                return 1;
            int next;
            for (int i = 0; i < 4; i++) {//����״̬
                next = cur;//������һ״̬��
                if (v[hash(next)] && !judge(next))//�ж�״̬�����ԣ���״̬�����У�������
                    continue;
                dfs(next); 
            }
            return -1;
        };
    }
};
class DFSNode {
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
        auto judge = [&](Node& nd) {//��������߽�,����һ���򷵻�true
            return nd.x >= 0 && nd.x <= MAXN && nd.y >= 0 && nd.y <= MAXM;
        };
        auto hash= [&](Node& nd) {//��������߽�,����һ���򷵻�true
            return nd.x*m+nd.y;
        };
        function<int(Node)> dfs = [&](Node cur) {
            if (v[hash(cur)] && !judge(cur))//�ж�״̬�����ԡ��߽硢ȥ�أ���״̬�����У�������
                return 0;
            if (cur.x == n)//�ִ��յ㣬�ж�
                return 1;
            Node next;
            for (int i = 0; i < 4; i++) {//����״̬ת�Ʋ���ѡ��,nΪ�ɽ��в�����
                next = cur;//������һ״̬��
                if (v[hash(next)] && !judge(next))//�ж�״̬�����ԣ���״̬�����У�������
                    continue;
                //״̬�ƽ�; ��֦��Ǽ�Ϊ1;
                dfs(next); //������һ�صݹ�
                //״̬��ԭ;�ָ����;״̬��ԭΪδ���ж���ʱ
            }
        };
    }
};
class DFSTree {
    struct RstNode {
        int selected = 0;
        int noSelected = 0;
    };
public:
    RstNode dfs(TreeNode* cur) {
        if (!cur)//�ж�״̬�����ԣ���״̬�����У�������
            return { 0,0 };
        RstNode l = dfs(cur->left);
        RstNode r = dfs(cur->right);
        return { cur->val + l.noSelected + r.noSelected,max(l.noSelected,l.selected) + max(r.noSelected,r.selected) };
    }
    int DFS(TreeNode* root) {
        RstNode rst = dfs(root);
        return max(rst.selected, rst.noSelected);
    }
};
class DFSMap {
    vector<bool> get_prime(int n) {
        vector<bool> rst(n + 1, true);
        rst[1] = false;
        for (int i = 2; i <= n; i++)
            for (int j = i * 2; j <= n; j += i)
                rst[j] = false;
        return rst;
    }
    long long countPaths(int n, vector<vector<int>>& edges) {
        typedef long long ll;
        typedef pair<ll, ll> pll;
        vector<vector<int> > g(n + 1);
        for (auto& e : edges) {//�����ڽӱ�
            g[e[0]].emplace_back(e[1]);
            g[e[1]].emplace_back(e[0]);
        }
        vector<bool> isp = get_prime(n);
        vector<int> d(n, 0);
        ll rst = 0;
        function<pll(int, int)> dfs = [&](int x, int p)->pll {//����0������Ĵ�,�ұ�����
            int w = isp[x];
            ll res0 = 0, res1 = 0;
            if (w)
                ++res1;
            else
                ++res0;
            for (auto y : g[x]) {
                if (y == p) continue;
                auto [a, b] = dfs(y, x);
                if (w) {
                    rst += res1 * a;
                    res1 += a;
                }
                else {
                    rst += res0 * b + res1 * a;
                    res0 += a;
                    res1 += b;
                }
            }
            return { res0,res1 };
        };
        dfs(1, -1);
        return rst;
    }
};