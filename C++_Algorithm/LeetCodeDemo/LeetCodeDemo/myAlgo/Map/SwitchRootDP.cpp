using namespace std;
#include <vector>
#include <functional>
typedef pair<int, int> pii;
//������ڵ�����������ڵ�ľ����
//�ӡ��� 0 Ϊ������������ 2 Ϊ����ʱ��ԭ�� 2 ���ӽڵ㻹�� 2 ���ӽڵ㣬ԭ��  ���ӽڵ㻹��  ���ӽڵ㣬Ψһ�ı���� 0 �� 2 �ĸ��ӹ�ϵ
//ͨ����һ�仯������
//O(n)
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
        function<void(int, int)> dfs = [&](int x, int p) {//����0������Ĵ�,�ұ�����
            for (auto [y, w] : g[x]) {
                if (y == p) continue;//��ͬ����ı߲���
                dfs(y, x);
                f[x] += f[y] + (w == -1);
            }
        };
        dfs(0, -1);
        function<void(int, int)> reroot = [&](int x, int p) {//���ݱ仯����������ڵ�Ĵ�
            for (auto [y, w] : g[x]) {
                if (y == p) continue;
                f[y] += f[x] - (f[y] + (w == -1)) + (w == 1);
                reroot(y, x);
            }
        };
        reroot(0, -1);
        return f;
    }

    //����ͼ����ͨ��(��ȫ���б�����)�����Ը����ڵ�Ϊ����������������ľ�������顣
    vector<int> sumOfDistancesInTree(int n, vector<vector<int>>& edges) {
        vector<vector<int> > g(n);
        for (auto& e : edges) {//�����ڽӱ�
            g[e[0]].emplace_back(e[1]);
            g[e[1]].emplace_back(e[0]);
        }
        vector<int> d(n,0);
        vector<int> size(n, 1);
        function<void(int, int, int)> dfs = [&](int x, int p, int depth) {//����0������Ĵ�,�ұ�����
            d[0] += depth;
            for (auto y : g[x]) {
                if (y == p) continue;
                dfs(y, x, depth + 1);
                size[x] += size[y];
            }
        };
        dfs(0, -1, 0);
        function<void(int, int)> reroot = [&](int x, int p) {//���ݱ仯����������ڵ�Ĵ�
            for (auto y : g[x]) {
                if (y == p) continue;
                d[y] += d[x]+n - 2* size[y];
                reroot(y, x);
            }
        };
        reroot(0, -1);
        return d;
    }
};