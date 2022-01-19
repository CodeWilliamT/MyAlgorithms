using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
class Solution {
private:
    static bool cmp(string a, string b)
    {
        
        return a+b>b+a;
    }
public:
    string largestNumber(vector<int>& nums) {
        string rst;
        int n = nums.size();
        vector<string> strnums;
        for (auto itm : nums)
        {
            strnums.push_back(to_string(itm));
        }
        sort(strnums.begin(), strnums.end(), cmp);
        for (auto itm : strnums)
        {
            rst += itm;
        }
        if (rst[0] == '0')return "0";
        return rst;
    }
};
//int main()
//{
//    Solution s;
//    vector<int> matrix = {8308,830,830};
//    cout<<s.largestNumber(matrix)<<endl;
//    return 0;
//}