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
//模拟 细致条件分析 设计题
//
class ATM {
    typedef unsigned long long ull;
    vector<ull> coins;
    int nums[5] = { 20,50,100,200,500 };
public:
    ATM() {
        coins = vector<ull>(5, 0);
    }

    void deposit(vector<int> banknotesCount) {
        for (int i = 0; i < banknotesCount.size();i++) {
            coins[i] += banknotesCount[i];
        }
    }

    vector<int> withdraw(int amount) {
        vector<int> rst(5, 0);
        for (int i = 4; i>=0; i--) {
            if (amount >= nums[i]) {
                rst[i] = amount / nums[i];
                if (rst[i] > coins[i]) {
                    rst[i] = coins[i];
                }
                amount -= rst[i] * nums[i];
            }
        }
        if (amount > 0) {
            rst = { -1 };
        }
        else {
            for (int i = 4; i >= 0; i--) {
                coins[i] -= rst[i];
            }
        }
        return rst;
    }
};

/**
 * Your ATM object will be instantiated and called as such:
 * ATM* obj = new ATM();
 * obj->deposit(banknotesCount);
 * vector<int> param_2 = obj->withdraw(amount);
 */