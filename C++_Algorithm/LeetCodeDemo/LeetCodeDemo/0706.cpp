using namespace std;
#include <iostream>
//设计题 朴素实现
//数组实现
class MyHashMap {
    int mp[1000001]{};
public:
    MyHashMap() {
        memset(mp,0,sizeof(mp));
    }

    void put(int key, int value) {
        mp[key] = value+1;
    }

    int get(int key) {
        return mp[key] - 1;
    }

    void remove(int key) {
        if (mp[key])mp[key] = 0;
    }
};

/**
 * Your MyHashMap object will be instantiated and called as such:
 * MyHashMap* obj = new MyHashMap();
 * obj->put(key,value);
 * int param_2 = obj->get(key);
 * obj->remove(key);
 */