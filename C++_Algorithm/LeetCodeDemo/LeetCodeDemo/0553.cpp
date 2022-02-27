using namespace std;
#include <vector>
#include <string>
//找规律
//都大于1，所以分子连除
class Solution {
public:
    string optimalDivision(vector<int>& a) {
        int n = a.size();
        if (n == 1)return to_string(a[0]);
        if (n == 2)return to_string(a[0])+"/"+to_string(a[1]);
        string rst;
        int i = 0;
        for (int& e : a) {
            rst += to_string(e);
            if (i == n - 1)
                rst += ")";
            else
                rst += "/";
            if (i == 0)
                rst += "(";
            i++;
        }
        return rst;
    }
};