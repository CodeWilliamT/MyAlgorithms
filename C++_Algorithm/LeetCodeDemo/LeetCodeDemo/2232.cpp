using namespace std;
#include <vector>
#include <string>
//枚举 模拟
//l可能性低于15,r可能性低于15。
class Solution {
public:
    string minimizeResult(string s) {
        string sl, sr;
        bool flag = 1;
        for (char& e : s) {
            if (e == '+') {
                flag = 0;
                continue;
            }
            if (flag)
                sl.push_back(e);
            else {
                sr.push_back(e);
            }
        }
        string a, b = sl, c = sr, d;
        string na, nb, nc, nd;
        int n = s.size();
        int val = stoi(sl) + stoi(sr),rst=val;
        for(int i=0;i<sl.size();i++)
        {
            for(int j=1;j<=sr.size();j++){
                na = sl.substr(0,i);
                nb = sl.substr(i, sl.size()-i);
                nc = sr.substr(0, j);
                nd = sr.substr(j, sr.size() - j);
                val = (na.size()?stoi(na):1) * (stoi(nb) + stoi(nc)) * (nd.size() ? stoi(nd) : 1);
                if (val < rst) {
                    rst = val;
                    a = na, b = nb, c = nc, d = nd;
                }
            }
        }
        return a + "(" + b + "+" + c + ")" + d;
    }
};