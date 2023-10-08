using namespace std;
#include <vector>
#include <set>
#include <unordered_map>
//设计题 哈希 可重复集合 三刷
class StockPrice {
    unordered_map<int, int> mp;
    multiset<int> st;
    int cur,mx,mn;
public:
    StockPrice() {
        mp.clear(); 
        cur = -1;
    }

    void update(int t, int p) {
        if (mp.count(t)) {
            st.erase(st.find(mp[t]));
        }
        mp[t] = p;
        st.insert(mp[t]);
        cur = max(cur, t);
        mn = *st.begin();
        mx = *(--st.end());
    }

    int current() {
        return mp[cur];
    }

    int maximum() {
        return mx;
    }

    int minimum() {
        return mn;
    }
};

//设计题 哈希 每日一题二刷
//class StockPrice {
//private:
//    unordered_map<int, int> ump;
//    map<int, int> mp;
//    int cur = 0;
//public:
//    StockPrice() {
//        ump.clear();
//        mp.clear();
//        cur = 0;
//    }
//
//    void update(int timestamp, int price) {
//        if (ump.count(timestamp)) {
//            --mp[ump[timestamp]];
//            if (mp[ump[timestamp]] == 0) {
//                mp.erase(ump[timestamp]);
//            }
//            ump.erase(timestamp);
//        }
//        mp[price]++;
//        ump[timestamp]= price;
//        cur = max(timestamp,cur);
//    }
//
//    int current() {
//        return ump[cur];
//    }
//
//    int maximum() {
//        return (*prev(mp.end())).first;
//    }
//
//    int minimum() {
//        return (*mp.begin()).first;
//    }
//};

//设计题 哈希 可重复集合
//class StockPrice {
//    unordered_map<int, int> mp;
//    multiset<int> st;
//    int curt, curp, mx, mn;
//public:
//    StockPrice() {
//        mp.clear();
//        curt = -1;
//    }
//
//    void update(int t, int p) {
//        if (mp.count(t)) {
//            st.erase(st.find(mp[t]));
//        }
//        st.insert(p);
//        mp[t] = p;
//        if (t >= curt)
//            curt = t, curp = p;
//        mn = *st.begin();
//        mx = *(--st.end());
//    }
//
//    int current() {
//        return curp;
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