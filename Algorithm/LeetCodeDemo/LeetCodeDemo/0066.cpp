using namespace std;
#include <iostream>
#include <vector>
//简单题
class Solution {
public:
    vector<int> plusOne(vector<int>& digits) {
        reverse(digits.begin(), digits.end());
        int n = digits.size();
        int i = 0;
        digits[i] += 1;
        while (digits[i] == 10)
        {
            digits[i] = 0;
            i++;
            if (i >= n) { digits.push_back(1); break; }
            digits[i] += 1;
        }
        reverse(digits.begin(), digits.end());
        return digits;
    }
};