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
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
//
//巧思 两分。
//如模拟，4e8 gg
class Solution {
public:
    vector<int> countPairs(int n, vector<vector<int>>& edges, vector<int>& queries) {
        vector<int> links(n,0);
        unordered_map<int, int> g;
        int a, b;
        for (auto& e : edges) {
            a = e[0]-1, b = e[1]-1;
            if (e[0] > e[1])a = e[1]-1, b = e[0]-1;
            g[a*n+b]++;
            links[a]++;
            links[b]++;
        }
        vector<int> orded = links;
        sort(orded.begin(), orded.end());

        vector<int> rst;
        int cnt,idx;
        for (int& q : queries){
            cnt = 0;
            for (int i = 0; i < n; i++) {
                idx = upper_bound(orded.begin()+i+1, orded.end(), q - orded[i]) - orded.begin();
                cnt += n - idx;
            }
            for (auto& e : g) {
                a = e.first / n;
                b = e.first % n;
                if (links[a] + links[b] > q && links[a] + links[b] - e.second <= q) {
                    cnt--;
                }
            }
            rst.push_back(cnt);
        }
        return rst;
    }
};