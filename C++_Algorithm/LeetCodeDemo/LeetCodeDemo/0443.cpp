using namespace std;
#include <vector>
#include <string>
//细致条件分析 模拟
class Solution {
public:
    int compress(vector<char>& cs) {
        char c = cs[0];
        int rst = 1, cnt = 1;
        string s;
        for (int i = 1; i < cs.size(); i++) {
            if (cs[i] == c) {
                cnt++;
            }
            else {
                if (cnt > 1) {
                    s = to_string(cnt);
                    for (int j = 0; j < s.size(); j++) {
                        cs[rst + j] = s[j];
                    }
                    rst += s.size();
                }
                c = cs[i];
                cs[rst] = c;
                rst++;
                cnt = 1;
            }
        }
        if (cnt > 1) {
            s = to_string(cnt);
            for (int j = 0; j < s.size(); j++) {
                cs[rst + j] = s[j];
            }
            rst += s.size();
        }
        return rst;
    }
};