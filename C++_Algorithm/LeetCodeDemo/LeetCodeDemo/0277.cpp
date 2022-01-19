using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
// 巧思
// 利用图找规律
// 先定位可能的，然后对可能的进行判定。
//class Solution {
//public:
//    int findCelebrity(int n) {
//        if (n < 2)return 0;
//        int i = 0;
//        for(int j=1;j < n;j++)
//        {
//            if(knows(i, j))
//                i = j;
//        }
//        for (int j = 0; j < i; j++)
//        {
//            if (knows(i, j))return -1;
//        }
//        for (int j = 0; j < n; j++)
//        {
//            if (i == j)continue;
//            if (!knows(j, i))return -1;
//        }
//        return i;
//    }
//};
//朴素实现
//利用集合，若不是则剔除，验证剩下的是不是
//class Solution {
//public:
//    int findCelebrity(int n) {
//        if (n < 2)return 0;
//        unordered_set<int> st;
//        for (int i = 0; i < n; i++)
//        {
//            st.insert(i);
//        }
//        auto cur = st.begin();
//        auto other = next(cur);
//        while (!st.empty())
//        {
//            if (cur == other)other = next(cur);
//            if (other == st.end())
//                break;
//            if (knows(*cur, *other))
//            {
//                st.erase(cur);
//                cur = other;
//                other = st.begin();
//            }
//            else
//            {
//                st.erase(other);
//                other = st.begin();
//            }
//        }
        //for (int i = 0; i < n; i++)
        //{
        //    if (i == *cur)continue;
        //    if (knows(*cur, i))return -1;
        //    if (knows(i, *cur))return -1;
        //}
//        return *cur;
//    }
//};