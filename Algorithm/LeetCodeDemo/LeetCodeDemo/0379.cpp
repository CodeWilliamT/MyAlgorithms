using namespace std;
#include <iostream>
#include <vector>
//设计题
//数组与下标
class PhoneDirectory {
private:
    vector<int> v;
    int idx;
    int n;

public:
    /** Initialize your data structure here
        @param maxNumbers - The maximum numbers that can be stored in the phone directory. */
    PhoneDirectory(int maxNumbers) {
        if (maxNumbers < 1)return;
        n = maxNumbers;
        for (int i = 0; i < n; i++)
        {
            v.push_back(i);
        }
        idx = 0;
    }

    /** Provide a number which is not assigned to anyone.
        @return - Return an available number. Return -1 if none is available. */
    int get() {
        int num = v[idx];
        v[idx] = -1;
        while (idx < n - 1 && v[idx] != idx)idx++;
        return num;
    }

    /** Check if a number is available or not. */
    bool check(int number) {
        if (number >= n || number < 0)return false;
        return v[number] == number;
    }

    /** Recycle or release a number. */
    void release(int number) {
        if (number >= n || number < 0)return;
        v[number] = number;
        idx = min(idx, number);
    }
};

/**
 * Your PhoneDirectory object will be instantiated and called as such:
 * PhoneDirectory* obj = new PhoneDirectory(maxNumbers);
 * int param_1 = obj->get();
 * bool param_2 = obj->check(number);
 * obj->release(number);
 */