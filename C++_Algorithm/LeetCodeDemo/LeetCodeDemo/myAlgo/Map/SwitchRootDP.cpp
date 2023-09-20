using namespace std;
#include <vector>
#include <functional>
typedef pair<int, int> pii;
//������ڵ�����������ڵ�ľ����
//On
class SwitchRootDP {
public:
    //����ͼ����ͨ��(��ȫ���б�����)�����Ը����ڵ�Ϊ�������������ܹ���ת���ٴ�������򵽴�ȫ��������顣
    vector<int> minEdgeReversals(int n, vector<vector<int>>& edges) {
        vector<vector<pair<int, int> > > g(n);
        for (auto& e : edges) {//�����ڽӱ��������бߣ�����Ϊ��Ȩ�����Ϊ��Ȩ
            g[e[0]].emplace_back(e[1], 1);
            g[e[1]].emplace_back(e[0], -1);
        }
        vector<int> f(n);
        function<void(int, int)> dfs = [&](int u, int p) {//��0�������������跴ת������
            for (auto [v, w] : g[u]) {
                if (v == p) continue;//��ͬ����ı߲���
                dfs(v, u);
                f[u] += f[v] + (w == -1);
            }
        };
        dfs(0, -1);
        function<void(int, int)> reroot = [&](int u, int p) {
            for (auto [v, w] : g[u]) {
                if (v == p) continue;
                f[v] += f[u] - (f[v] + (w == -1)) + (w == 1);
                reroot(v, u);
            }
        };
        reroot(0, -1);
        return f;
    }

    //����ͼ����ͨ��(��ȫ���б�����)�����Ը����ڵ�Ϊ����������������ľ�������顣
    vector<int> sumOfDistancesInTree(int n, vector<vector<int>>& edges) {
        vector<vector<int> > g(n);
        for (auto& e : edges) {//�����ڽӱ��������бߣ�����Ϊ��Ȩ�����Ϊ��Ȩ,
            g[e[0]].emplace_back(e[1]);
            g[e[1]].emplace_back(e[0]);
        }
        vector<int> d(n,0);
        vector<int> size(n, 1);
        function<void(int, int, int)> dfs = [&](int u, int p, int depth) {
            d[0] += depth;
            for (auto v : g[u]) {
                if (v == p) continue;
                dfs(v, u, depth + 1);
                size[u] += size[v];
            }
        };
        dfs(0, -1, 0);
        function<void(int, int)> reroot = [&](int u, int p) {
            for (auto v : g[u]) {
                if (v == p) continue;
                d[v] += d[u]+n - 2* size[v];
                reroot(v, u);
            }
        };
        reroot(0, -1);
        return d;
    }
};