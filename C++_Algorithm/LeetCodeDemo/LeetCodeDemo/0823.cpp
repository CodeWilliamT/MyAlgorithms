using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <unordered_set>
#include <unordered_map>
#include <set>
#include <map>
#include <queue>
#include <stack>
#include <functional>
#include <bitset>
typedef pair<int, int> pii;

//��ϣ
//ö�ٸ�������+����ö�ٸ�������������ܱ��������նԣ���+(���Ӳ�ͬ��2��1)*�������������Ŀ�ĳ˻���
class Solution {
    typedef long long ll;
public:
    int numFactoredBinaryTrees(vector<int>& arr) {
        unordered_map<int,ll> mp;
        int n = arr.size();
        sort(arr.begin(), arr.end());
        int rst = 0, num;
        ll delta;
        int mod = 1e9 + 7;
        for (int i = 0; i < n; i++) {
            rst++;
            mp[arr[i]]=(mp[arr[i]]+1) % mod;
            for (int j = 0; j <= i; j++) {
                if (arr[i] % arr[j]==0){
                    num = arr[i] / arr[j];
                    if (mp.count(num)) {
                        delta= (mp[arr[j]] * mp[num]) % mod;
                        mp[arr[i]]= (mp[arr[i]]+delta) % mod;
                        rst= (rst+delta)%mod;
                    }
                }
            }
        }
        return rst;
    }
};