using namespace std;
#include "myHeader.h"

class DisjoinSet {
private:
    vector<int> f;
    int N;
public:
    //��ĳ�㼯Ⱥ
    int findp(int x) {
        if (x == f[x])return x;
        return findp(f[x]);
    }
    void unionp(int x, int y) {
        int a = findp(x);
        int b = findp(y);
        int p = min(a, b);
        f[a] = f[b] = f[x] = f[y] = p;
    }
    /// <summary>
    /// ���ݱ߼��������鼯
    /// </summary>
    /// <param name="n">����</param>
    /// <param name="edges">�߼���({��㣬�յ�})</param>
    /// <param name="plus">��С���</param>
    void DisJoin(int n, vector<vector<int>>& edges,int plus=1) {
        f = vector<int>(n + plus);
        N = n;
        for (int i = plus; i < n + plus; i++)
            f[i] = i;
        int cost = 0;
        for (auto& e : edges) {
            int x = findp(e[0]);
            int y = findp(e[1]);//�ҳ�����ͷ������
            if (x != y) { cost += 1; unionp(x, y); }//���ڲ�ͬ������ϲ�
        }
    }
    //��������
    int CountSets(int plus = 1) {
        set<int> st;
        int p;
        for (int i = 1 + plus; i < N + plus; i++) {
            p = findp(i);
            if (!st.count(p)) {
                st.insert(p);
            }
        }
        return st.size();
    }
    /// <summary>
    /// �����Ѵ�С��������ߣ��ñ������㲻��һ�����鼯�������Ӽ��ϡ�����+=�ñ�Ȩ�ء�
    /// </summary>
    /// <param name="n">�ڵ���Ŀ</param>
    /// <param name="edges">�߼���({��㣬�յ㣬����})</param>
    /// <param name="plus">��С���</param>
    /// <returns>����ͨ���е����С���ѣ�������ͨ����-1</returns>
    int Kruskal(int n, vector<vector<int>>& edges, int plus = 1)
    {
        sort(edges.begin(), edges.end(), [&](vector<int>& a, vector<int>& b) {return a[2] < b[2]; });//���߼���Ȩֵ��С��������
        f = vector<int>(n + plus);
        for (int i = plus; i < n + plus; i++)
            f[i] = i;
        int cost = 0;
        for (auto& e : edges) {
            int x = findp(e[0]);
            int y = findp(e[1]);//�ҳ�����ͷ������
            if (x != y) { cost += e[2]; unionp(x, y); }//���ڲ�ͬ������ϲ�
        }
        //�������е��ж��Ƿ���ͬһ����
        for (int i = 1 + plus; i < n + plus; i++) {
            if (findp(plus) != findp(i)) {
                return -1;
            }
        }
        return cost;
    }
};