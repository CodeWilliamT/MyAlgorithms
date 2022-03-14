using namespace std;
#include <vector>
#include <algorithm>
#include <functional>
//图 并查集 最小生成树
class Solution {
    int Kruskal(int n,vector<vector<int>>& edges)
    {
        int plus = 1;//编号从1开始为1，从0为0
        vector<int> f(n+ plus);
        for (int i = plus; i < n+ plus; i++)
            f[i] = i;
        function<int(int)> findp = [&](int x) {
            if (x == f[x])return x;
            return findp(f[x]);
        };
        function<void(int, int)> unionp = [&](int x, int y) {
            int a = findp(x);
            int b = findp(y);
            int p = min(a, b);
            f[a] = f[b] = f[x] = f[y] = p;
        };
        int rst = 0;
        sort(edges.begin(), edges.end(), [&](vector<int>& a, vector<int>& b) {return a[2]<b[2]; });//给边集按权值从小到大排序
        for (auto&e:edges){
            int x = findp(e[0]);
            int y = findp(e[1]);//找出集合头顶点编号
            if (x != y) { rst += e[2]; unionp(x, y); }//若在不同集合则合并
        }
        //遍历所有点判断是否在同一集合
        for (int i = 1+ plus; i < n + plus; i++) {
            if (findp(plus) != findp(i)) {
                return -1;
            }
        }
        return rst;
    }
public:
    int minimumCost(int n, vector<vector<int>>& connections) {
        return Kruskal(n, connections);
    }
};