using namespace std;
#include <iostream>
#include <unordered_set>
//回溯(深搜) 剪枝 哈希 大数相加
class Solution {
    const int prime=35;
private:
    string bigAdd(string a, string b) {
        string rst;
        int dig=0;
        for (int i = a.size() - 1, j = b.size() - 1;i>-1||j>-1||dig;i--,j--) {
            if(i>-1)
                dig += a[i]-'0';
            if(j>-1)
                dig += b[j]-'0';
            rst.push_back(dig%10+'0');
            dig = dig / 10;
        }
        reverse(rst.begin(), rst.end());
        return rst;
    }
    bool dfs(int a, int b, int c, int d, string& num, unordered_set<int>& v) {
        if (d > num.size())
            return false;
        int key = a*pow(prime,3)+ b * pow(prime, 2)+ c * prime+d;
        if (v.count(key))
            return false;
        v.insert(key);
        string x, y, z;
        x = num.substr(a, b - a);
        y = num.substr(b, c - b);
        z = num.substr(c, d - c);
        if ((x[0]-'0'||b-a==1)&& (y[0] - '0' || c - b == 1) && (z[0] - '0' || d - c == 1)&& z.compare(bigAdd(x, y)) == 0) {
            if (d == num.size())
                return true;
            if (dfs(b, c, d, d + 1, num,v))
                return true;
        }
        if (b < c - 1)
            if (dfs(a, b + 1, c, d, num, v))
                return true;
        if (c < d - 1)
            if (dfs(a, b, c + 1, d, num, v))
                return true;
        if (d <= num.size())
            if (dfs(a, b, c, d + 1, num, v))
                return true;
        return false;
    }
public:
    bool isAdditiveNumber(string num) {
        unordered_set<int> v;
        return dfs(0, 1, 2, 3,num,v);
    }
};