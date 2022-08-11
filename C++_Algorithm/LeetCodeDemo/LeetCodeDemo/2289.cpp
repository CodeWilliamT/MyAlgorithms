using namespace std;
#include <iostream>
#include <vector>
//每次删相邻递减的数，统计使得无序数组变为非递减数组的操作数。
//单调栈
//构建一个栈,栈的每层记录 某个元素的值 和向后合并了几次。
//遍历到某元素，在栈中查询它恰好小于的位置，过程中 出栈，合并次数 取 过程中合并的次数较大那个。
//如果遍历到栈空了则合并的次数为0，否则合并次数+1；入栈 当前元素的值，合并次数
class Solution {
    typedef pair<int, int> pii;
public:
    int totalSteps(vector<int>& nums) {
        vector<pii> st;
        int rst = 0, maxT;
        for (int& e : nums) {
            maxT = 0;
            while (!st.empty() && e >= st.back().first) {
                maxT = max(maxT, st.back().second);
                st.pop_back();
            }
            maxT = st.empty() ? 0 : maxT + 1;
            rst = max(rst, maxT);
            st.push_back({ e,maxT });
        }
        return rst;
    }
};