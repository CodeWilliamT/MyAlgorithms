using namespace std;
#include <vector>
//简单模拟
//标中等不理解。。
class Bank {
    vector<long long> b;
    int n;
public:
    Bank(vector<long long>& balance) {
        b = balance;
        n = b.size();
    }

    bool transfer(int account1, int account2, long long money) {
        if (account1<1|| account1>n|| account2<1|| account2>n||b[account1-1] < money) {
            return false;
        }
        b[account1-1] -= money;
        b[account2-1] += money;
        return true;
    }

    bool deposit(int account, long long money) {
        if (account<1 || account>n) {
            return false;
        }
        b[account-1] += money;
        return true;
    }

    bool withdraw(int account, long long money) {
        if (account<1 || account>n || b[account - 1] < money) {
            return false;
        }
        b[account-1] -= money;
        return true;
    }
};