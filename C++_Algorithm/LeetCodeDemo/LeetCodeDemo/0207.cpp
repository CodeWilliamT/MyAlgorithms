using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <queue>
#include <unordered_map>
#include <unordered_set>
//图论
//拓扑排序判环
class Solution {
public:
    bool canFinish(int numCourses, vector<vector<int>>& prerequisites) {
        queue<int> q;
        vector<vector<int>> edges(numCourses);
        vector<int> in(numCourses);
        int count=0;
        for (auto& e : prerequisites)
        {
            edges[e[1]].push_back(e[0]);
            in[e[0]]++;
        }
        for (int i=0;i<numCourses;i++)
        {
            if (!in[i])
                q.push(i);
        }
        while (!q.empty())
        {
            int cur = q.front();
            count++;
            q.pop();
            for (auto& e : edges[cur])
            {
                in[e]--;
                if (!in[e])
                    q.push(e);
            }
        }

        return count==numCourses;
    }
};

//图论
//哈希图判环,哈希表去重
//class Solution {
//    bool findF(int x,int y, unordered_map<int, vector<int>>& mp, unordered_set<int>& st)
//    {
//        if (x == y)return true;
//        if (!mp.count(x))
//        {
//            return false;
//        }
//        if (st.count(x))
//        {
//            return false;
//        }
//        st.insert(x);
//        for (int i = 0; i < mp[x].size(); i++)
//        {
//            if (findF(mp[x][i], y, mp,st))
//            {
//                return true;
//            }
//        }
//        return false;
//    }
//public:
//    bool canFinish(int numCourses, vector<vector<int>>& prerequisites) {
//        unordered_map<int, vector<int>> mp;
//        unordered_set<int> st;
//        for (auto e : prerequisites)
//        {
//            st.clear();
//            if(findF(e[1], e[0], mp,st))return false;
//            mp[e[0]].push_back(e[1]);
//        }
//        return true;
//    }
//};