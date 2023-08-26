using namespace std;
#include <iostream>
#include <vector>
#include <string>
//¼òµ¥Ä£Äâ
//
class Solution {
public:
    vector<string> summaryRanges(vector<int>& nums) {
        vector<string> rst;
        int head,last = 0;
        for (int& e : nums) {
            if (rst.empty()||last+1!=e) {
                rst.push_back(to_string(e));
                head = e;
            }
            else {
                if (rst.back().size()==1)
                    rst.back()+="->"+ to_string(e);
                else
                    rst.back() = to_string(head)+"->"+to_string(e);
            }
            last = e;
        }
        return rst;
    }
};