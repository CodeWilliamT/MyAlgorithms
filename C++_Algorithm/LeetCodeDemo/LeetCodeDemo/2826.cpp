using namespace std;
#include <iostream>
#include <vector>
//找规律 巧思
//改变数组中元素，使得数组递增，答最小改动数。
//枚举以i,j为2组的左右边界时的1，3总数，其为改动值。最小值可能为答案。
//枚举不存在2，以i,n-1为3组的左右边界时的1，2总数，其为改动值。最小值可能为答案。
//枚举值存在1，即2，3总数为改动值。
//取三个可能答案的最小值
class Solution {
public:
    int minimumOperations(vector<int>& nums) {
        int n = nums.size();
        int cnt[3][100]{};
        for (int i = 0; i < n; i++) {
            cnt[0][i]= (nums[i] == 1)+(i>0?cnt[0][i - 1]:0);
            cnt[1][i] = (nums[i] == 2) + (i > 0 ? cnt[1][i - 1] : 0);
            cnt[2][i] = (nums[i] == 3) + (i > 0 ? cnt[2][i - 1] : 0);
        }
        int rst = INT32_MAX;
        for (int i = 0; i < n; i++) {
            for (int j = i; j < n; j++) {
                rst = min(rst, (i > 0 ? cnt[1][i - 1] : 0) +
                    (i > 0 ? cnt[2][i - 1] : 0) +
                    cnt[0][j]-(i>0?cnt[0][i-1]:0)+
                    cnt[2][j] - (i > 0 ? cnt[2][i - 1] : 0)+
                    cnt[0][n-1] - cnt[0][j]+
                    cnt[1][n-1] - cnt[1][j]
                
                );
            }
        }
        for (int i = 0; i < n; i++) {
            rst = min(rst, (i > 0 ? cnt[1][i - 1] : 0) +
                (i > 0 ? cnt[2][i - 1] : 0) + 
                cnt[0][n-1] - (i > 0 ? cnt[0][i - 1] : 0) +
                cnt[1][n-1] - (i > 0 ? cnt[1][i - 1] : 0));
        }
        return min(rst,cnt[1][n-1]+ cnt[2][n - 1]);
    }
};