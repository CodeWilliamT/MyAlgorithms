using namespace std;
#include <iostream>
#include <vector>
#include <set>

//重新思考
//动态规划
class Solution {
public:
    int minimumWhiteTiles(string floor, int numCarpets, int carpetLen) {

    }
};
//堆排序 贪心 前缀和
//让每块地毯遮挡的位置尽量多
//统计i位置开始len长度地毯能遮盖的白块数。
//开始遮盖最多的位置(相同则取靠左)，更新跟受影响的位置，直到用完。
class Solution {
    using pii = pair<int, int>;
public:
    int minimumWhiteTiles(string s, int num, int len) {
        int n = s.size();
        if (n <= len*num)return 0;
        vector<int> d(n, 0);
        set<pii> st;
        int idx,cnt=0;
        set<pii>::iterator pos;
        pii cur;
        int rst = 0;
    _solve:
        cnt = 0;
        for (int i = 0; i < len; i++) {
            cnt += s[i] == '1';
        }
        for (int i = 0; i < n; i++) {
            d[i] = cnt;
            if(cnt)
                st.insert({ -cnt,i });
            cnt -= s[i] == '1';
            if (i + len < n)
                cnt += s[i + len]=='1';
        }
        while (num&&!st.empty()) {
            pos = st.begin();
            cur = *pos;
            idx = cur.second;
            st.erase(pos);
            for (int i = idx; i < idx+len&&i<n; i++) {
                s[i] = '0';
            }
            for (int i = 1; i < len; i++) {
                if (idx - i >= 0 && st.count({ -d[idx - i],idx - i })) {
                    pos = st.find({ -d[idx - i],idx - i });
                    st.erase(pos);
                }
                if (idx + i < n && st.count({ -d[idx + i],idx + i })) {
                    pos = st.find({ -d[idx + i],idx + i });
                    st.erase(pos);
                }
            }
            num--;
        }
        rst = 0;
        for (int i = 0; i < n; i++) {
            rst += s[i] == '1';
        }
        if (num > 0&&rst>0) {
            goto _solve;
        }
        return rst;
    }
};