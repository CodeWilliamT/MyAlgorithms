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
#include "myAlgo\Structs\TreeNode.cpp"
#define MAXN (int)1e5+1
#define MAXM (int)1e5+1
typedef pair<int, bool> pib;
typedef long long ll;
typedef pair<ll, ll> pll;
typedef pair<int, int> pii;
class Solution {
public:
    int differenceOfSums(int n, int m) {
        int rst=0;
        for (int i = 1; i <= n; i++) {
            rst += i % m ? i : -i;
        }
        return rst;
    }
};