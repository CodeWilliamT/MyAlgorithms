using namespace std;
#include <vector>
class BIT {//logN时间查询前缀和；logN时间查修改元素并更新前缀和。
private:
    vector<int> tree;
    int len;
public:
    BIT(int n) {
        this->len = n;
        tree = vector<int> (n + 1);
    }
    //单点更新
    //i     原始数组索引 i
    //delta 变化值 = 更新以后的值 - 原始值
    void update(int i, int delta) {
        while (i <= len) {// 从下到上更新，注意，预处理数组，比原始数组的 len 大 1，故 预处理索引的最大值为 len
            tree[i] += delta;
            i += lowbit(i);
        }
    }
    //查询前缀和
    //i 前缀的最大索引，即查询区间 [0, i] 的所有元素之和
    int query(int i) {
        int sum = 0;
        while (i > 0) {// 从右到左查询
            sum += tree[i];
            i -= lowbit(i);
        }
        return sum;
    }
    int lowbit(int x) {
        return x & (-x);
    }
};