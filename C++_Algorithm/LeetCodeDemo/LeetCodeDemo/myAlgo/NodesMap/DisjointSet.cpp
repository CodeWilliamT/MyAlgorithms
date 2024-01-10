using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
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
    void DisJoin(int n, vector<vector<int>>& edges,int from1 =1) {
        f = vector<int>(n + from1);
        N = n;
        for (int i = from1; i < n + from1; i++)
            f[i] = i;
        int cost = 0;
        for (auto& e : edges) {
            int x = findp(e[0]);
            int y = findp(e[1]);//�ҳ�����ͷ������
            if (x != y) { cost += 1; unionp(x, y); }//���ڲ�ͬ������ϲ�
        }
    }
    //��������
    int CountSets(int from1 = 1) {
        unordered_set<int> st;
        int p;
        for (int i = 1 + from1; i < N + from1; i++) {
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
    int Kruskal(int n, vector<vector<int>>& edges, int from1 = 1)
    {
        sort(edges.begin(), edges.end(), [&](vector<int>& a, vector<int>& b) {return a[2] < b[2]; });//���߼���Ȩֵ��С��������
        f = vector<int>(n + from1);
        for (int i = from1; i < n + from1; i++)
            f[i] = i;
        int cost = 0;
        for (auto& e : edges) {
            int x = findp(e[0]);
            int y = findp(e[1]);//�ҳ�����ͷ������
            if (x != y) { cost += e[2]; unionp(x, y); }//���ڲ�ͬ������ϲ�
        }
        //�������е��ж��Ƿ���ͬһ����
        for (int i = 1 + from1; i < n + from1; i++) {
            if (findp(from1) != findp(i)) {
                return -1;
            }
        }
        return cost;
    }
};