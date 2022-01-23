using namespace std;
#include <vector>
#include <map>
#include <unordered_map>
//设计题 哈希 新解
class StockPrice {
private:
    unordered_map<int, int> ump;
    map<int, int> mp;
    int cur = 0;
public:
    StockPrice() {
        ump.clear();
        mp.clear();
        cur = 0;
    }

    void update(int timestamp, int price) {
        if (ump.count(timestamp)) {
            --mp[ump[timestamp]];
            if (mp[ump[timestamp]] == 0) {
                mp.erase(ump[timestamp]);
            }
            ump.erase(timestamp);
        }
        mp[price]++;
        ump[timestamp]= price;
        cur = max(timestamp,cur);
    }

    int current() {
        return ump[cur];
    }

    int maximum() {
        return (*prev(mp.end())).first;
    }

    int minimum() {
        return (*mp.begin()).first;
    }
};

//设计题 哈希
//class StockPrice {
//private:
//    int mx, mn, cur;
//    bool isInit;
//    map<int, int> flow;
//    multiset<int> dt;
//public:
//    StockPrice() {
//        mx = 1;
//        mn = 1000000001;
//        cur = -1;
//        isInit = false;
//        flow.clear();
//        dt.clear();
//    }
//
//    void update(int timestamp, int price) {
//        if (flow.count(timestamp))
//        {
//            int prevPrice = flow[timestamp];
//            dt.erase(dt.find(prevPrice));
//        }
//        flow[timestamp] = price;
//        dt.insert(price);
//        if (!isInit)
//        {
//            mx = price;
//            mn = price;
//            cur = price;
//            isInit = true;
//            return;
//        }
//        mx = *prev(dt.end());
//        mn = *dt.begin();
//        cur = (*prev(flow.end())).second;
//    }
//
//    int current() {
//        return cur;
//    }
//
//    int maximum() {
//        return mx;
//    }
//
//    int minimum() {
//        return mn;
//    }
//};
