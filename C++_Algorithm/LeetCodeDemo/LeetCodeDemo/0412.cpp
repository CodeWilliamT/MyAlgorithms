using namespace std;
#include <iostream>
#include <vector>
#include <string>
//简单题 朴素实现
class Solution {
public:
    vector<string> fizzBuzz(int n) {
        vector<string> rst;
        for (int i = 1; i <= n; i++)
        {
            if (i % 3 == 0 && i % 5 == 0)
                rst.push_back("FizzBuzz");
            else if (i % 3 == 0)
                rst.push_back("Fizz");
            else if (i % 5 == 0)
                rst.push_back("Buzz");
            else
                rst.push_back(to_string(i));
        }
        return rst;
    }
};