using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <numeric>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
#include "myAlgo\LCParse\TreeNode.cpp"
#define MAXN (int)1e5+1
#define MAXM (int)1e5+1
typedef pair<int, bool> pib;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//��������
class Solution {
public:
    //���ã���֪���������� nums ��һ������ k����������������Ԫ�صĳ˻��ϸ�С�� k ���������������Ŀ
    int numSubarrayProductLessThanK(vector<int>& nums, int k) {
        int n = nums.size();
        int v = 1;
        int rst = 0;
        int l = 0, r = 0;//�б����䣬��l����r��
        while (l < n) {
            if (v >= k&&l<r) {
                v /= nums[l];
                l++;
            }
            else{
                rst += r - l;//��r��β����������r-l��
                if (r < n) {
                    v *= nums[r];
                    r++;
                }
                else {
                    break;
                }
            }
        }
        return rst;
    }
};