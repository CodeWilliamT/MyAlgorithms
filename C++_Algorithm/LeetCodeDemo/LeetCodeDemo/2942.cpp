﻿using namespace std;
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
typedef pair<int, bool> pib;
typedef pair<int, int> pii;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<ll, int> pli;
#define MAXN (int)(1e5+1)
#define MAXM (int)(1e5+1)
#define MOD (int)(1e9+7)
class Solution {
public:
    vector<int> findWordsContaining(vector<string>& words, char x) {
        vector<int> rst;
        int i = 0;
        for (string& s : words) {
            for (char& c : s) {
                if (c == x) {
                    rst.push_back(i); 
                    break;
                }
            }
            i++;
        }
        return rst;
    }
};