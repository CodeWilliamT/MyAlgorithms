using namespace std;
#include<iostream>
#include<unordered_map>

struct LinkedNode
{
    int Key;
    int Value;
    LinkedNode* Prev,*Next;
    LinkedNode(int x, int y) :Key(x), Value(y), Prev(nullptr), Next(nullptr) {}
    LinkedNode(int x, int y, LinkedNode* z) :Key(x), Value(y), Prev(z),Next(nullptr) {}
    LinkedNode(int x, int y, LinkedNode* z, LinkedNode* k) :Key(x), Value(y), Prev(z), Next(k) {}

};

class LRUCache {
private:
    unordered_map<int, LinkedNode*> hashmap;
    int capacity, size;
    LinkedNode* head, *tail;
public:
    LRUCache(int _capacity) {
        capacity = _capacity;
        size = 0;
        head = nullptr;
        tail = nullptr;
    }
    void movetotail(int key) {
        if (hashmap[key] == tail)return;
        if (hashmap[key] == head)head = head->Next;
        else
            hashmap[key]->Prev->Next = hashmap[key]->Next;
        if (hashmap[key] != tail)
        {
            hashmap[key]->Next->Prev = hashmap[key]->Prev;
            tail->Next = hashmap[key];
            hashmap[key]->Prev = tail;
            tail = hashmap[key];
            hashmap[key]->Next = nullptr;
        }
    }
    int get(int key) {
        if (hashmap.find(key) == hashmap.end())return -1;
        movetotail(key);
        return hashmap[key]->Value;
    }
    void add(int key, int value) {
        LinkedNode* item = new LinkedNode(key,value,tail);
        hashmap[key] = item;
        if (tail == nullptr)head = item;
        else tail->Next = item;
        tail = item;
        size++;
    }
    void update(int key, int value) {
        if (hashmap.find(key) != hashmap.end())remove(key);
        add(key, value);
    }
    void remove(int key) {
        auto iter = hashmap.find(key);
        if(iter ==hashmap.end())return;
        if (iter->second == head)head = head->Next;
        else
            iter->second->Prev->Next = iter->second->Next;
        if (iter->second == tail)tail = tail->Prev;
        else
            iter->second->Next->Prev = iter->second->Prev;
        delete iter->second;
        hashmap.erase(iter);
        size--;
    }
    void put(int key, int value) {
        update(key, value);
        if (size > capacity)remove(head->Key);
    }
};