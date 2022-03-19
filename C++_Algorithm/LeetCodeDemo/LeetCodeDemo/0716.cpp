using namespace std;
#include <vector>
#include <map>

//哈希 堆排序 链表 巧思负索引倒序
//O(1)修改,两分找位置O(logn)插入。
class MaxStack {
    map<int, vector<list<int>::iterator>> mp;
    list<int> stk;
public:
    MaxStack() {
        mp.clear();
        stk.clear();
    }

    void push(int x) {
        auto pos=stk.insert(stk.end(),x);
        mp[-x].push_back(pos);
    }

    int pop() {
        int pre = -stk.back();
        mp[pre].pop_back();
        if (mp[pre].size() == 0) {
            mp.erase(pre);
        }
        stk.pop_back();
        return -pre;
    }

    int top() {
        return stk.back();
    }

    int peekMax() {
        return -(*mp.begin()).first;
    }

    int popMax() {
        int key = (*mp.begin()).first;
        stk.erase(mp[key].back());
        mp[key].pop_back();
        if (mp[key].size() == 0) {
            mp.erase(key);
        }
        return -key;
    }
};