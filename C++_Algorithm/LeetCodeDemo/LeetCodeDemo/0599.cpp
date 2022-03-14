using namespace std;
#include <iostream>
#include <vector>
//简单模拟
//按长度配对过呗
class Solution {  
public:
    vector<string> findRestaurant(vector<string>& list1, vector<string>& list2) {
        int n1 = list1.size(), n2 = list2.size();
        vector<string> rst;
        for (int i = 0; i < n1 + n2; i++) {
            for (int j = max(i-n2+1,0); j < n1&&i-j>=0; j++) {
                if (list1[j] == list2[i-j]) {
                    rst.push_back(list1[j]);
                }
            }
            if (rst.size())break;
        }
        return rst;
    }
};