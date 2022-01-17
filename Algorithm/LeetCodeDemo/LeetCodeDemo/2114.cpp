using namespace std;
#include <iostream>
#include <vector>
//朴素实现
class Solution {
public:
    int mostWordsFound(vector<string>& sentences) {
        int rst = 0;
        int space = 0;
        for (auto& s : sentences)
        {
            space = 0;
            for (char& c : s)
            {
                if(c==' ')space++;
            }
            rst = max(rst, space + 1);
        }
        return rst;
    }
};