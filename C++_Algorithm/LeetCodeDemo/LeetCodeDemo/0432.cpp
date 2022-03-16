using namespace std;
#include <iostream>
#include <unordered_map>
//哈希 链表
//字符串指向数量，数量指向字符串
//查询O(1)哈希，两分插入,使有序O(logn)list
class AllOne {
    using pis = pair<int, string>;
    list<pis> ls;
    unordered_map<string,list<pis>::iterator> mp;
public:
    AllOne() {
        ls.clear();
        mp.clear();
    }

    void inc(string key) {
        if (!mp.count(key)) {
            ls.push_front({ 1,key });
            mp[key] = ls.begin();
        }
        else {
            auto tmp = mp[key];
            pis val = make_pair((*tmp).first + 1, key);
            auto pos = lower_bound(mp[key], ls.end(), val);
            mp[key]=ls.insert(pos, val);
            ls.erase(tmp);
        }
    }

    void dec(string key) {
        if (mp.count(key)) {
            auto tmp = mp[key];
            if ((*tmp).first > 1) {
                pis val = make_pair((*tmp).first - 1, key);
                auto pos = lower_bound(ls.begin(), mp[key], val);
                mp[key] = ls.insert(pos, val);
            }
            else {
                mp.erase(key);
            }
            ls.erase(tmp);
        }
    }

    string getMaxKey() {
        return ls.size()?ls.back().second:"";
    }

    string getMinKey() {
        return ls.size() ? ls.front().second : "";
    }
};
