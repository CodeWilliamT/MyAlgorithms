using namespace std;
#include <iostream>
#include <vector>
#include <string>
#include <algorithm>
#include <set>
#include <map>
//设计题
//哈希
class StockPrice {
private:
    int mx, mn, cur;
    bool isInit;
    map<int, int> flow;
    multiset<int> dt;
public:
    StockPrice() {
        mx = 1;
        mn = 1000000001;
        cur = -1;
        isInit = false;
        flow.clear();
        dt.clear();
    }

    void update(int timestamp, int price) {
        if (flow.count(timestamp))
        {
            int prevPrice = flow[timestamp];
            dt.erase(dt.find(prevPrice));
        }
        flow[timestamp] = price;
        dt.insert(price);
        if (!isInit)
        {
            mx = price;
            mn = price;
            cur = price;
            isInit = true;
            return;
        }
        mx = *prev(dt.end());
        mn = *dt.begin();
        cur = (*prev(flow.end())).second;
    }

    int current() {
        return cur;
    }

    int maximum() {
        return mx;
    }

    int minimum() {
        return mn;
    }
};

/**
 * Your StockPrice object will be instantiated and called as such:
 * StockPrice* obj = new StockPrice();
 * obj->update(timestamp,price);
 * int param_2 = obj->current();
 * int param_3 = obj->maximum();
 * int param_4 = obj->minimum();
 */