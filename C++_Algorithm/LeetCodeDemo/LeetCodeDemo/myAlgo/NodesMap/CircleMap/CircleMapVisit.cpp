using namespace std;
#include <vector>
#include <functional>
//����(����) ͼ
//����ͼ�У����� ������Ϊ��� �� ��·�������ʵ��� �� �����顣
//
//ÿ������Ϊ��㶼����һ�飬��¼�𰸡���Ϊ���������л��¼������ı������̣�����O(n)
//����ʱ�����л������ж���ͬһ��ڶ��η��ʼ�¼����ȡ�
//���㲻�ǻ���ͼҲû���⡣
//O(n)
class CircleMapVisit {
#define MAXN (int)1e5+1
public:
    //����ͼ�У����� ������Ϊ��� �� ��·�������ʵ��� �� �����顣
    //edges:����ָ����������㹹�ɵ����顣
    //return ÿ����������Ĳ��ֲ�·����������ඥ����
    vector<int> countVisitedNodes(vector<int>& edges) {
        int n = edges.size();
        vector<vector<int> > g(n);
        for (int i = 0; i < n; i++) {//�����ڽӱ�
            g[i].emplace_back(edges[i]);
        }
        vector<int>rst(n, 0);
        int v[MAXN]{};
        int loopflag = -1;
        function<int(int, int, int)> dfs = [&](int x, int p, int depth) {
            if (rst[x]) {
                return rst[x];
            }
            if (v[x]) {
                loopflag = x;
                return depth - v[x];
            }
            v[x] = depth;
            depth++;
            for (auto y : g[x]) {
                int dep = dfs(y, x, depth);
                rst[x] = max(rst[x], dep);
            }
            if (loopflag == -1) {
                rst[x]++;
            }
            if (loopflag == x) {
                loopflag = -1;
            }
            return rst[x];
        };
        for (int i = 0; i < n; i++) {
            if (rst[i])continue;
            loopflag = -1;
            dfs(i, -1, 1);
        }
        return rst;
    }
};