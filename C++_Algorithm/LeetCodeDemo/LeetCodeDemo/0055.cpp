using namespace std;
#include <iostream>
#include <vector>

//��˼
//���յ����
//���¿ɴ����
class Solution {
public:
    bool canJump(vector<int>& nums) {
        int n = nums.size();
        if (n < 2)return true;
        int len = 0;
        for (int i = n - 2; i > -1; i--)
        {
            if (i + nums[i] > n - len - 2)len = n - 1 - i;
        }
        return len == n - 1;
    }
};