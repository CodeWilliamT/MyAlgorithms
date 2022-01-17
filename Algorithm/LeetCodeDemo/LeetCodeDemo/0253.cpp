using namespace std;
#include <iostream>
#include <vector>
#include <algorithm>
#include <set>
//贪心 哈希自序集
//排序，然后记录索引有序哈希，根据ivl[x][1]<=ivl[y][0]来归纳个体y进入x的集合
class Solution {
public:
    int minMeetingRooms(vector<vector<int>>& ivl) {
        set<int> st;
        sort(ivl.begin(), ivl.end());
        int n = ivl.size();
        for (int i = 0; i < n; i++)
        {
            st.insert(i);
        }
        int prev;
        set<int>::iterator cur;
        int rst = 0;
        while (!st.empty())
        {
            rst++;
            prev = *st.begin();
            st.erase(prev);
            for (auto e = st.begin(); e != st.end(); )
            {
                cur = e;
                e = next(e);
                if (ivl[*cur][0] >= ivl[prev][1])
                {
                    prev = *cur;
                    st.erase(cur);
                }
            }
        }
        return rst;
    }
};