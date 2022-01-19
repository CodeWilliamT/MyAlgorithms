using namespace std;
#include<iostream>
#include<vector>
#include<set>
#include<algorithm>
//思路找出城市道路数最大的俩，数目加下，连着就-1，不连不减。
//邻接矩阵查联通
class Solution {
public:
    int maximalNetworkRank(int n, vector<vector<int>>& r) {
        vector<vector<bool>> s(n, vector<bool>(n));
        vector<vector<int>> f(n, vector<int>(2));
        for (int i = 0; i < r.size(); i++)
        {
            s[r[i][0]][r[i][1]] = true;
            s[r[i][1]][r[i][0]] = true;
            f[r[i][0]][0]++;
            f[r[i][1]][0]++;
            f[r[i][0]][1] = r[i][0];
            f[r[i][1]][1] = r[i][1];
        }

        sort(f.begin(), f.end());
        int temp;
        int ans = f[n - 1][0] + f[n - 2][0] - 1;
        for (int i = n - 1; i > -1; i--)
        {
            for (int j = i - 1; j > -1; j--)
            {
                temp = (s[f[i][1]][f[j][1]] || s[f[j][1]][f[i][1]]) ? f[i][0] + f[j][0] - 1 : f[i][0] + f[j][0];
                if (temp < ans)
                {
                    break;
                }
                if (temp > ans)
                {
                    return ans + 1;
                }
            }
        }

        return ans;
    }
};

////邻接表查联通
//class Solution {
//    struct Edge {
//        int index;//该顶点该边对应邻接点表头的数组索引
//        Edge(int x) :index(x){}
//    };
//    bool isConnected(int a,int b)
//    {
//        for (int i = 0; i < s[a].size();i++)
//        {
//            if (s[a][i]->index == b)return true;
//        }
//        return false;
//    }
//    vector<vector<Edge*>> s;
//public:
//    int maximalNetworkRank(int n, vector<vector<int>>& r) {
//        s = vector<vector<Edge*>>(n);
//        vector<vector<int>> f(n, vector<int>(2));
//        for (int i = 0; i < r.size(); i++)
//        {
//            s[r[i][0]].push_back(new Edge(r[i][1]));
//            s[r[i][1]].push_back(new Edge(r[i][0]));
//            f[r[i][0]][0]++;
//            f[r[i][1]][0]++;
//            f[r[i][0]][1] = r[i][0];
//            f[r[i][1]][1] = r[i][1];
//        }
//
//        sort(f.begin(), f.end());
//        int temp;
//        int ans = f[n - 1][0] + f[n - 2][0] - 1;
//        for (int i = n - 1; i > -1; i--)
//        {
//            for (int j = i - 1; j > -1; j--)
//            {
//                temp = (isConnected(f[i][1], f[j][1])|| isConnected(f[j][1], f[i][1])) ? f[i][0] + f[j][0] - 1 : f[i][0] + f[j][0];
//                if (temp < ans)
//                {
//                    break;
//                }
//                if (temp > ans)
//                {
//                    return ans + 1;
//                }
//            }
//        }
//
//        return ans;
//    }
//};

//哈希查联通
//class Solution {
//public:
//    int maximalNetworkRank(int n, vector<vector<int>>& r) {
//        set<pair<int, int> > s;
//        vector<vector<int>> f(n, vector<int>(2));
//        for (int i = 0; i < r.size(); i++)
//        {
//            s.insert(make_pair(r[i][0], r[i][1]));
//            s.insert(make_pair(r[i][1], r[i][0]));
//            f[r[i][0]][0]++;
//            f[r[i][1]][0]++;
//            f[r[i][0]][1] = r[i][0];
//            f[r[i][1]][1] = r[i][1];
//        }
//
//        sort(f.begin(), f.end());
//        int temp;
//        int ans = f[n - 1][0] + f[n - 2][0] - 1;
//        for (int i = n - 1; i > -1; i--)
//        {
//            for (int j = i - 1; j > -1; j--)
//            {
//                temp = (s.count(make_pair(f[i][1], f[j][1])) > 0) || (s.count(make_pair(f[j][1], f[i][1])) > 0) ? f[i][0] + f[j][0] - 1 : f[i][0] + f[j][0];
//                if (temp < ans)
//                {
//                    break;
//                }
//                if (temp > ans)
//                {
//                    return ans + 1;
//                }
//            }
//        }
//
//        return ans;
//    }
//};